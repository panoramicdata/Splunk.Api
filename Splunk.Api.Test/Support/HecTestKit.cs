using System.Net;

namespace Splunk.Api.Test.Support;

/// <summary>Helpers for <see cref="SplunkHecClient"/> tests: a client over a stub, and one-request capture.</summary>
internal static class HecTestKit
{
	public const string BaseUrl = "https://splunk.test:8088";
	public const string Token = "11111111-2222-3333-4444-555555555555";
	public const string Channel = "aaaaaaaa-bbbb-cccc-dddd-eeeeeeeeeeee";

	/// <summary>The collector's reply to accepted events, captured from Splunk 10.6.0.5.</summary>
	public const string SuccessJson = """{"text":"Success","code":0,"ackId":3}""";

	public static SplunkHecClient Create(StubHandler stub, string? channel)
		=> new(new SplunkHecClientOptions { BaseUrl = BaseUrl, Token = Token, Channel = channel, MaxRetries = 0 }, stub);

	public static StubHandler Stub(string json, HttpStatusCode status) => TestClient.Stub(json, status);

	/// <summary>Sends one call through a client with the default channel, answering <paramref name="response"/>; returns the request.</summary>
	public static async Task<RecordedCall> CaptureAsync(Func<SplunkHecClient, CancellationToken, Task> call, string response)
	{
		var stub = Stub(response, HttpStatusCode.OK);
		using var client = Create(stub, Channel);
		await call(client, TestContext.Current.CancellationToken);
		return stub.Calls.Should().ContainSingle().Subject;
	}

	/// <summary>Asserts the verb, path, query, JSON or text body and the authorization of a collector request.</summary>
	public static void ShouldBeHec(this RecordedCall call, HttpMethod method, string path, string query, string? body)
	{
		call.ShouldMatch(method, path, query, body);
		call.Headers.Authorization!.Scheme.Should().Be("Splunk");
		call.Headers.Authorization.Parameter.Should().Be(Token);
	}
}
