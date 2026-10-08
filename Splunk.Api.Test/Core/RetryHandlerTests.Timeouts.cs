using Microsoft.Extensions.Logging;
using Splunk.Api.Handlers;
using Splunk.Api.Test.Support;
using System.Net;

namespace Splunk.Api.Test.Core;

public partial class RetryHandlerTests
{
	/// <summary>A transport that never answers: it waits until its cancellation token fires.</summary>
	private static DelegateHandler Hanging()
		=> new(async (_, token) =>
		{
			await Task.Delay(Timeout.InfiniteTimeSpan, token);
			throw new InvalidOperationException("unreachable");
		});

	[Fact]
	public async Task AttemptTimeout_RaisesTimeoutException()
	{
		using var harness = new Harness(o => o.Timeout = TimeSpan.FromMilliseconds(1), Hanging());

		var act = () => harness.SendAsync(HttpMethod.Get);

		var thrown = await act.Should().ThrowAsync<TimeoutException>();
		thrown.Which.Message.Should().Be("Splunk did not respond within 00:00:00.0010000.");
		thrown.Which.InnerException.Should().BeAssignableTo<OperationCanceledException>();
	}

	[Fact]
	public async Task CallerCancellation_RaisesOperationCanceled()
	{
		using var cts = new CancellationTokenSource();
		using var invoker = new HttpMessageInvoker(new RetryHandler(new SplunkClientOptions()) { InnerHandler = Hanging() });

		var sending = invoker.SendAsync(new HttpRequestMessage(HttpMethod.Get, Url), cts.Token);
		await cts.CancelAsync();
		var act = () => sending;

		await act.Should().ThrowAsync<OperationCanceledException>();
	}

	[Fact]
	public async Task CallerCancellation_DuringBackOff_RaisesOperationCanceled()
	{
		using var cts = new CancellationTokenSource();
		var stub = new StubHandler();
		stub.Enqueue(HttpStatusCode.ServiceUnavailable);
		var handler = new RetryHandler(new SplunkClientOptions { MaxRetries = 1 })
		{
			InnerHandler = stub,
			Delay = (_, token) =>
			{
				cts.Cancel();
				return Task.Delay(Timeout.InfiniteTimeSpan, token);
			}
		};
		using var invoker = new HttpMessageInvoker(handler);

		var act = () => invoker.SendAsync(new HttpRequestMessage(HttpMethod.Get, Url), cts.Token);

		await act.Should().ThrowAsync<OperationCanceledException>();
		stub.Calls.Should().ContainSingle();
	}

	[Fact]
	public async Task Client_DefaultDelay_RetriesWithoutWaitingForAZeroBackOff()
	{
		var stub = new StubHandler();
		stub.Enqueue(HttpStatusCode.ServiceUnavailable);
		stub.Enqueue(HttpStatusCode.OK, """{"entry":[]}""");
		using var client = TestClient.Create(stub, o => (o.MaxRetries, o.RetryBaseDelay) = (1, TimeSpan.Zero));

		var feed = await client.ServerInfo.GetAsync(TestContext.Current.CancellationToken);

		feed.Entries.Should().BeEmpty();
		stub.Calls.Should().HaveCount(2);
	}

	[Fact]
	public async Task Logging_NamesMethodPathAndAttempt_ButNeverTheQuery()
	{
		using var harness = new Harness().Respond(HttpStatusCode.ServiceUnavailable, HttpStatusCode.OK);

		using var response = await harness.SendAsync(HttpMethod.Get);

		harness.Logger.Entries.Should().Equal(
			(LogLevel.Debug, "Splunk GET https://splunk.test:8089/services/search/jobs (attempt 1)"),
			(LogLevel.Warning, "Splunk returned 503 for GET https://splunk.test:8089/services/search/jobs; retrying in 00:00:01"),
			(LogLevel.Debug, "Splunk GET https://splunk.test:8089/services/search/jobs (attempt 2)"));
		harness.Logger.Messages.Should().NotContain(m => m.Contains("secret") || m.Contains('?'));
	}

	[Fact]
	public async Task WithoutALogger_StillRetries()
	{
		using var harness = new Harness(o => o.Logger = null).Respond(HttpStatusCode.ServiceUnavailable, HttpStatusCode.OK);

		using var response = await harness.SendAsync(HttpMethod.Get);

		response.StatusCode.Should().Be(HttpStatusCode.OK);
		harness.Logger.Entries.Should().BeEmpty();
	}

	[Fact]
	public async Task Client_Timeout_RaisesTimeoutException()
	{
		using var client = new SplunkClient(new SplunkClientOptions { BaseUrl = TestClient.BaseUrl, Token = "t", Timeout = TimeSpan.FromMilliseconds(1) }, Hanging());

		var act = () => client.ServerInfo.GetAsync(TestContext.Current.CancellationToken);

		await act.Should().ThrowAsync<TimeoutException>();
	}

	[Fact]
	public async Task Client_CallerCancellation_RaisesOperationCanceled()
	{
		using var cts = new CancellationTokenSource();
		using var client = new SplunkClient(new SplunkClientOptions { BaseUrl = TestClient.BaseUrl, Token = "t" }, Hanging());

		var sending = client.ServerInfo.GetAsync(cts.Token);
		await cts.CancelAsync();
		var act = () => sending;

		await act.Should().ThrowAsync<OperationCanceledException>();
	}
}
