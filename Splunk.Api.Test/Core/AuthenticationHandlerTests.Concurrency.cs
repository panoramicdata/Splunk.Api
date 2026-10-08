using Splunk.Api.Handlers;
using Splunk.Api.Test.Support;
using System.Net;

namespace Splunk.Api.Test.Core;

public partial class AuthenticationHandlerTests
{
	private const int Concurrent = 8;

	/// <summary>
	/// A Splunk stand-in: each login issues the next key (<c>key-1</c>, <c>key-2</c>...) once <paramref name="loginGate"/>
	/// opens; other requests are answered by <paramref name="answer"/> given the Authorization header they carried.
	/// </summary>
	private static DelegateHandler FakeSplunk(Task loginGate, Func<string?, Task<HttpStatusCode>> answer)
	{
		var logins = 0;
		return new DelegateHandler(async (request, _) =>
		{
			if (request.RequestUri!.AbsolutePath == "/services/auth/login")
			{
				await loginGate;
				return new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent(Login($"key-{Interlocked.Increment(ref logins)}")) };
			}

			return new HttpResponseMessage(await answer(request.Headers.Authorization?.ToString())) { Content = new StringContent("{}") };
		});
	}

	private static async Task<HttpStatusCode[]> SendConcurrentlyAsync(HttpMessageHandler transport, int count, Action? started = null)
	{
		using var invoker = new HttpMessageInvoker(new AuthenticationHandler(SessionOptions(), new Uri(Base)) { InnerHandler = transport });
		var sends = Enumerable.Range(0, count)
			.Select(_ => invoker.SendAsync(new HttpRequestMessage(HttpMethod.Get, Info), TestContext.Current.CancellationToken))
			.ToList();
		started?.Invoke();
		var responses = await Task.WhenAll(sends);
		var statuses = responses.Select(r => r.StatusCode).ToArray();
		Array.ForEach(responses, r => r.Dispose());
		return statuses;
	}

	private static SplunkClientOptions SessionOptions()
	{
		var options = new SplunkClientOptions();
		TestClient.UseSession(options);
		return options;
	}

	private static int LoginCount(DelegateHandler transport)
		=> transport.Requests.Count(r => r.RequestUri!.AbsolutePath == "/services/auth/login");

	[Fact]
	public async Task Session_ConcurrentFirstRequests_LogInOnce()
	{
		var gate = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
		var transport = FakeSplunk(gate.Task, _ => Task.FromResult(HttpStatusCode.OK));

		var statuses = await SendConcurrentlyAsync(transport, Concurrent, gate.SetResult);

		statuses.Should().AllBeEquivalentTo(HttpStatusCode.OK);
		LoginCount(transport).Should().Be(1);
		transport.Authorizations.Where(a => a is not null).Should().HaveCount(Concurrent).And.AllBe("Splunk key-1");
	}

	[Fact]
	public async Task Session_ConcurrentRequestsThatSeeTheSame401_LogInAgainOnce()
	{
		// Every request first arrives with key-1; none is answered until all have, then each gets 401 and must log in again.
		var allArrived = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
		var arrivals = 0;
		var transport = FakeSplunk(Task.CompletedTask, async authorization =>
		{
			if (authorization != "Splunk key-1")
			{
				return HttpStatusCode.OK;
			}

			if (Interlocked.Increment(ref arrivals) == Concurrent)
			{
				allArrived.SetResult();
			}

			await allArrived.Task;
			return HttpStatusCode.Unauthorized;
		});

		var statuses = await SendConcurrentlyAsync(transport, Concurrent);

		statuses.Should().AllBeEquivalentTo(HttpStatusCode.OK);
		LoginCount(transport).Should().Be(2);
		transport.Authorizations.Count(a => a == "Splunk key-2").Should().Be(Concurrent);
	}

	[Fact]
	public async Task Session_CancelledWhileWaitingForLogin_RaisesOperationCanceled()
	{
		var gate = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
		using var cts = new CancellationTokenSource();
		var transport = FakeSplunk(gate.Task, _ => Task.FromResult(HttpStatusCode.OK));
		using var invoker = new HttpMessageInvoker(new AuthenticationHandler(SessionOptions(), new Uri(Base)) { InnerHandler = transport });

		var first = invoker.SendAsync(new HttpRequestMessage(HttpMethod.Get, Info), TestContext.Current.CancellationToken);
		var waiting = invoker.SendAsync(new HttpRequestMessage(HttpMethod.Get, Info), cts.Token);
		await cts.CancelAsync();
		var act = () => waiting;

		await act.Should().ThrowAsync<OperationCanceledException>();
		gate.SetResult();
		using var response = await first;
		response.StatusCode.Should().Be(HttpStatusCode.OK);
		LoginCount(transport).Should().Be(1);
	}
}
