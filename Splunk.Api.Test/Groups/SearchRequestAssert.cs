using Splunk.Api.Test.Support;
using System.Net;

namespace Splunk.Api.Test.Groups;

/// <summary>Shared assertions and stubs for the search, SPL2 and metrics catalog tests.</summary>
internal static class SearchRequestAssert
{
	/// <summary>Asserts that the stub received exactly one request, and that it matches.</summary>
	public static void ShouldHaveSent(this StubHandler stub, HttpMethod method, string path, string query, string? body = null)
	{
		var call = stub.Calls.Should().ContainSingle().Subject;
		call.Method.Should().Be(method);
		call.Uri.AbsolutePath.Should().Be(path);
		call.Uri.Query.Should().Be(query);
		call.Body.Should().Be(body);
	}

	/// <summary>A stub answering one request with a plain-text or other non-JSON body.</summary>
	public static StubHandler TextStub(string text, string mediaType)
	{
		var stub = new StubHandler();
		stub.Enqueue(HttpStatusCode.OK, text, r => r.Content = new StringContent(text, System.Text.Encoding.UTF8, mediaType));
		return stub;
	}

	/// <summary>A stub answering one request with 204 No Content.</summary>
	public static StubHandler NoContentStub()
	{
		var stub = new StubHandler();
		stub.Enqueue(HttpStatusCode.NoContent, string.Empty, r => r.Content = new ByteArrayContent([]));
		return stub;
	}

	/// <summary>Asserts that the call raises <see cref="SplunkApiException"/> with the status and message.</summary>
	public static async Task ShouldFailWith(this Func<Task> act, HttpStatusCode status, string message)
	{
		var thrown = await act.Should().ThrowAsync<SplunkApiException>();
		thrown.Which.StatusCode.Should().Be(status);
		thrown.Which.Message.Should().Be(message);
	}

	/// <summary>The feed envelope around one entry's content, as Splunk returns it.</summary>
	public static string Feed(string name, string content, string author = "admin")
		=> $$"""
			{
				"links": {},
				"origin": "https://splunk.test:8089/services/x",
				"updated": "2026-10-08T13:47:16+00:00",
				"generator": { "build": "86587d4e3b27", "version": "10.6.0.5" },
				"entry": [
					{
						"name": {{System.Text.Json.JsonSerializer.Serialize(name)}},
						"id": "https://splunk.test:8089/services/x/{{name}}",
						"updated": "2026-10-08T13:47:16.894+00:00",
						"links": { "alternate": "/services/x/{{name}}" },
						"author": "{{author}}",
						"acl": { "app": "search", "owner": "{{author}}", "sharing": "user", "perms": null },
						"content": {{content}}
					}
				],
				"paging": { "total": 1, "perPage": 30, "offset": 0 },
				"messages": []
			}
			""";
}
