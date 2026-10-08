using System.Net;
using System.Text.Json;

namespace Splunk.Api.Test.Support;

/// <summary>Shared helpers for the input and output endpoint tests: capture one request, map one response, assert one error.</summary>
internal static class InputsTestKit
{
	/// <summary>The query every JSON request carries when it sets no other parameter.</summary>
	public const string JsonQuery = "?output_mode=json";

	/// <summary>A feed with no entries, as Splunk answers many writes.</summary>
	public const string EmptyFeed = """{"links":{},"origin":"https://splunk.test:8089/services/x","updated":"2026-10-08T14:00:00+00:00","generator":{"build":"86587d4e3b27","version":"10.6.0.5"},"entry":[],"paging":{"total":0,"perPage":30,"offset":0},"messages":[]}""";

	/// <summary>Sends one call through a stubbed client answering <see cref="EmptyFeed"/>, and returns the request it made.</summary>
	public static Task<RecordedCall> CaptureAsync(Func<SplunkClient, CancellationToken, Task> call)
		=> CaptureAsync(call, EmptyFeed);

	/// <summary>Sends one call through a stubbed client answering <paramref name="response"/>, and returns the request it made.</summary>
	public static Task<RecordedCall> CaptureAsync(Func<SplunkClient, CancellationToken, Task> call, string response)
		=> TestClient.CaptureAsync(call, response);

	/// <summary>Sends one call through a stubbed client answering <paramref name="response"/>, and returns what it read.</summary>
	public static Task<T> MapAsync<T>(Func<SplunkClient, CancellationToken, Task<T>> call, string response)
		=> TestClient.ReadAsync(call, response);

	/// <summary>Reads the single entry of a feed.</summary>
	public static async Task<Models.SplunkEntry<T>> MapEntryAsync<T>(Func<SplunkClient, CancellationToken, Task<Models.SplunkFeed<T>>> call, string response)
	{
		var feed = await MapAsync(call, response);
		feed.Generator!.Version.Should().Be("10.6.0.5");
		return feed.Entries.Should().ContainSingle().Subject;
	}

	/// <summary>Asserts that a call raises <see cref="SplunkApiException"/> with Splunk's message when Splunk answers 404.</summary>
	public static Task ShouldRaiseNotFoundAsync(Func<SplunkClient, CancellationToken, Task> call)
		=> TestClient.ShouldFailAsync(call, HttpStatusCode.NotFound, """{"messages":[{"type":"ERROR","text":"Not Found"}]}""", "Not Found");

	/// <summary>Asserts a GET with no parameters and no body.</summary>
	public static void ShouldBeGet(this RecordedCall call, string path) => call.ShouldBe(HttpMethod.Get, path, JsonQuery, null);

	/// <summary>Asserts a DELETE with no parameters and no body.</summary>
	public static void ShouldBeDelete(this RecordedCall call, string path) => call.ShouldBe(HttpMethod.Delete, path, JsonQuery, null);

	/// <summary>Asserts a POST with the given form body, or with none.</summary>
	public static void ShouldBePost(this RecordedCall call, string path, string? body)
	{
		call.ShouldBe(HttpMethod.Post, path, JsonQuery, body);
		call.ContentType.Should().Be(body is null ? null : "application/x-www-form-urlencoded");
	}

	/// <summary>
	/// A feed of one entry with the given name and content, in the envelope Splunk 10.6 sends (captured from the test
	/// instance, host names replaced).
	/// </summary>
	public static string Feed(string name, string content)
		=> $$"""
			{
				"links": { "create": "/services/x/_new", "_reload": "/services/x/_reload" },
				"origin": "https://splunk.test:8089/services/x",
				"updated": "2026-10-08T14:00:00+00:00",
				"generator": { "build": "86587d4e3b27", "version": "10.6.0.5" },
				"entry": [
					{
						"name": {{JsonSerializer.Serialize(name)}},
						"id": "https://splunk.test:8089/servicesNS/nobody/system/x",
						"updated": "1970-01-01T00:00:00+00:00",
						"links": { "alternate": "/servicesNS/nobody/system/x", "list": "/servicesNS/nobody/system/x" },
						"author": "nobody",
						"acl": {
							"app": "system", "can_list": true, "can_write": true, "modifiable": false, "owner": "nobody",
							"perms": { "read": ["admin", "power"], "write": ["admin"] }, "removable": true, "sharing": "system"
						},
						"content": {{content}}
					}
				],
				"paging": { "total": 1, "perPage": 30, "offset": 0 },
				"messages": []
			}
			""";
}
