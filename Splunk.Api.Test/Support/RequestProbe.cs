using System.Net;

namespace Splunk.Api.Test.Support;

/// <summary>Sends one call through a stubbed <see cref="SplunkClient"/> and checks what went over the wire or came back.</summary>
internal static class RequestProbe
{
	/// <summary>The query every JSON request carries.</summary>
	public const string Json = "?output_mode=json";

	/// <summary>An empty feed, as Splunk returns from control actions.</summary>
	public const string EmptyFeed = """{"links":{},"entry":[],"paging":{"total":0,"perPage":30,"offset":0},"messages":[]}""";

	/// <summary>Runs <paramref name="call"/> against a client that answers <paramref name="json"/>; returns the one request sent.</summary>
	public static Task<RecordedCall> SendAsync(Func<SplunkClient, CancellationToken, Task> call, string json = EmptyFeed)
		=> TestClient.CaptureAsync(call, json);

	/// <summary>Runs <paramref name="call"/> against a client that answers <paramref name="json"/>; returns the result.</summary>
	public static Task<T> ReadAsync<T>(Func<SplunkClient, CancellationToken, Task<T>> call, string json)
		=> TestClient.ReadAsync(call, json);

	/// <summary>Asserts that <paramref name="call"/> raises <see cref="SplunkApiException"/> with the given status and message.</summary>
	public static Task FailsAsync(Func<SplunkClient, CancellationToken, Task> call, HttpStatusCode status, string body, string message)
		=> TestClient.ShouldFailAsync(call, status, body, message);

	/// <summary>Asserts the request's method, path, query and form body.</summary>
	public static void ShouldBeProbed(this RecordedCall call, HttpMethod method, string path, string query = Json, string? body = null)
		=> call.ShouldMatch(method, path, query, body);

	/// <summary>
	/// Wraps one entry's <paramref name="content"/> (a JSON object) in Splunk's feed envelope, as a 10.6 server returns it.
	/// </summary>
	public static string Feed(string name, string content)
		=> $$"""
			{
				"links": {},
				"origin": "https://splunk.test:8089/services/endpoint",
				"updated": "2026-10-08T13:49:25+00:00",
				"generator": { "build": "86587d4e3b27", "version": "10.6.0.5" },
				"entry": [
					{
						"name": "{{name}}",
						"id": "https://splunk.test:8089/services/endpoint/{{name}}",
						"updated": "1970-01-01T00:00:00+00:00",
						"links": { "alternate": "/services/endpoint/{{name}}", "list": "/services/endpoint/{{name}}" },
						"author": "system",
						"acl": {
							"app": "", "can_list": true, "can_write": true, "modifiable": false, "owner": "system",
							"perms": { "read": ["admin"], "write": ["admin"] }, "removable": false, "sharing": "system"
						},
						"content": {{content}}
					}
				],
				"paging": { "total": 1, "perPage": 30, "offset": 0 },
				"messages": []
			}
			""";

	/// <summary>Reads a one-entry feed built by <see cref="Feed"/> and returns the entry's content.</summary>
	public static async Task<T> ReadContentAsync<T>(Func<SplunkClient, CancellationToken, Task<Models.SplunkFeed<T>>> call, string name, string content)
	{
		var feed = await ReadAsync(call, Feed(name, content));
		var entry = feed.Entries.Should().ContainSingle().Subject;
		entry.Name.Should().Be(name);
		return entry.Content!;
	}
}
