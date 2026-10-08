using System.Net;

namespace Splunk.Api.Test.Support;

/// <summary>
/// One-line request pinning for endpoint tests: send one call through a stubbed client and assert the exact request, or
/// assert how an error response surfaces.
/// </summary>
internal static class EndpointRequests
{
	/// <summary>An empty feed, the response of most write operations.</summary>
	public const string EmptyFeed = """{"links":{},"entry":[],"paging":{"total":0,"perPage":30,"offset":0},"messages":[]}""";

	/// <summary>The query every JSON request carries when the call adds none of its own.</summary>
	public const string JsonQuery = "?output_mode=json";

	/// <summary>Sends <paramref name="call"/> to a client whose transport answers <paramref name="response"/>, and returns the one request.</summary>
	public static Task<RecordedCall> SendAsync(Func<SplunkClient, CancellationToken, Task> call, string response = EmptyFeed)
		=> TestClient.CaptureAsync(call, response);

	/// <summary>Sends <paramref name="call"/> to a client whose transport answers <paramref name="response"/>, and returns the result.</summary>
	public static Task<T> ReadAsync<T>(Func<SplunkClient, CancellationToken, Task<T>> call, string response)
		=> TestClient.ReadAsync(call, response);

	/// <summary>Asserts the request's method, path, query and form body (<see langword="null"/> for none).</summary>
	public static void ShouldBeEndpointRequest(this RecordedCall call, HttpMethod method, string path, string? body = null, string query = JsonQuery)
	{
		call.ShouldMatch(method, path, query, body);
		if (body is not null)
		{
			call.ContentType.Should().Be("application/x-www-form-urlencoded");
		}
	}

	/// <summary>Asserts that <paramref name="call"/> raises <see cref="SplunkApiException"/> carrying Splunk's error message.</summary>
	public static Task ShouldRaiseSplunkErrorAsync(Func<SplunkClient, CancellationToken, Task> call, HttpStatusCode status = HttpStatusCode.NotFound)
	{
		const string Text = "Could not find object id=missing";
		return TestClient.ShouldFailAsync(call, status, $$"""{"messages":[{"type":"ERROR","text":"{{Text}}"}]}""", Text);
	}
}
