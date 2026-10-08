using System.Net;

namespace Splunk.Api.Test.Support.Platform;

/// <summary>Sends one call through a stubbed client and asserts what went over the wire.</summary>
internal static class Calls
{
	/// <summary>The query every JSON call carries when it sets nothing else.</summary>
	public const string JsonQuery = "?output_mode=json";

	/// <summary>A stub answering with an empty feed.</summary>
	public const string EmptyFeed = """{"entry":[],"messages":[]}""";

	/// <summary>Runs <paramref name="call"/> against a client answering <paramref name="json"/> and returns the one recorded request.</summary>
	public static Task<RecordedCall> RecordAsync(Func<SplunkClient, Task> call, string json)
		=> TestClient.CaptureAsync((client, _) => call(client), json);

	/// <summary>Runs <paramref name="call"/> and asserts the request's method, unescaped path, raw query and form body.</summary>
	public static async Task<RecordedCall> AssertAsync(
		Func<SplunkClient, Task> call,
		HttpMethod method,
		string path,
		string query,
		string? body)
	{
		var recorded = await RecordAsync(call, EmptyFeed);
		recorded.ShouldMatch(method, path, query, body);
		return recorded;
	}

	/// <summary>Runs <paramref name="call"/> against a client that answers <paramref name="json"/> and returns the result.</summary>
	public static Task<T> MapAsync<T>(Func<SplunkClient, Task<T>> call, string json)
		=> TestClient.ReadAsync((client, _) => call(client), json);

	/// <summary>Asserts that a Splunk error response raises <see cref="SplunkApiException"/> carrying its message.</summary>
	public static Task AssertErrorAsync(Func<SplunkClient, Task> call, HttpStatusCode status)
		=> TestClient.ShouldFailAsync((client, _) => call(client), status, """{"messages":[{"type":"ERROR","text":"Splunk says no"}]}""", "Splunk says no");

	/// <summary>The test's cancellation token.</summary>
	public static CancellationToken Token => TestContext.Current.CancellationToken;
}
