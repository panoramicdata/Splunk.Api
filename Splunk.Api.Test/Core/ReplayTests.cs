using Splunk.Api.Test.Support;
using System.Net;

namespace Splunk.Api.Test.Core;

/// <summary>
/// Request bodies built by Refit must be replayable through the real client, or POSTs are never retried and never resent
/// after a re-login.
/// </summary>
public class ReplayTests
{
	private static readonly Dictionary<string, string?> Fields = new() { ["name"] = "a b", ["search"] = "index=_internal | head 1" };

	private const string ExpectedBody = "name=a+b&search=index%3D_internal+%7C+head+1";

	private static Task PostAsync(SplunkClient client)
		=> client.For<IProbe>().PostAsync("services/saved/searches", Fields, TestContext.Current.CancellationToken);

	[Fact]
	public async Task FormPost_IsRetriedOn503_WithAnIdenticalBody()
	{
		var stub = new StubHandler();
		stub.Enqueue(HttpStatusCode.ServiceUnavailable);
		stub.Enqueue(HttpStatusCode.Created);
		using var client = TestClient.Create(stub, o => (o.MaxRetries, o.RetryBaseDelay) = (1, TimeSpan.Zero));

		await PostAsync(client);

		stub.Calls.Should().HaveCount(2);
		stub.Calls.Select(c => c.Body).Should().AllBe(ExpectedBody);
		stub.Calls.Select(c => c.ContentType).Should().AllBe("application/x-www-form-urlencoded");
	}

	[Fact]
	public async Task FormPost_IsResentOnceAfterALoginOn401_WithAnIdenticalBody()
	{
		var stub = new StubHandler();
		stub.Enqueue(HttpStatusCode.OK, TestClient.LoginJson("key-1"));
		stub.Enqueue(HttpStatusCode.Unauthorized);
		stub.Enqueue(HttpStatusCode.OK, TestClient.LoginJson("key-2"));
		stub.Enqueue(HttpStatusCode.Created);
		using var client = TestClient.Create(stub, TestClient.UseSession);

		await PostAsync(client);

		stub.Calls.Select(c => c.Uri.AbsolutePath).Should().Equal(
			"/services/auth/login", "/services/saved/searches", "/services/auth/login", "/services/saved/searches");
		stub.Calls[1].Authorization.Should().Be("Splunk key-1");
		stub.Calls[3].Authorization.Should().Be("Splunk key-2");
		stub.Calls[1].Body.Should().Be(ExpectedBody);
		stub.Calls[3].Body.Should().Be(ExpectedBody);
	}
}
