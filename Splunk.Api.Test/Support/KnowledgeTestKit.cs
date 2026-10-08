using Splunk.Api.Models;
using System.Net;

namespace Splunk.Api.Test.Support;

/// <summary>Shared request recording, envelope building and assertions for the knowledge endpoint tests.</summary>
internal static class KnowledgeTestKit
{
	/// <summary>The query every JSON request carries when no other parameters are sent.</summary>
	public const string JsonQuery = "?output_mode=json";

	/// <summary>The feed Splunk returns after a delete, or for an empty collection.</summary>
	public const string EmptyFeed = """{"links":{},"origin":"https://splunk.test:8089/services/x","entry":[],"paging":{"total":0,"perPage":30,"offset":0},"messages":[]}""";

	/// <summary>Sends one request through a stubbed client and returns what was sent.</summary>
	public static async Task<RecordedCall> SendAsync(Func<SplunkClient, Task> act, string json = EmptyFeed)
	{
		var stub = TestClient.Stub(json);
		using var client = TestClient.Create(stub);
		await act(client);
		return stub.Calls.Should().ContainSingle().Subject;
	}

	/// <summary>Sends one request through a stubbed client in the <c>nobody/search</c> namespace and returns what was sent.</summary>
	public static async Task<RecordedCall> SendInSearchAppAsync(Func<SplunkClient, Task> act, string json = EmptyFeed)
	{
		var stub = TestClient.Stub(json);
		using var client = TestClient.Create(stub);
		using var app = client.InNamespace("nobody", "search");
		await act(app);
		return stub.Calls.Should().ContainSingle().Subject;
	}

	/// <summary>Reads the single entry of a stubbed response.</summary>
	public static async Task<SplunkEntry<T>> SingleEntryAsync<T>(Func<SplunkClient, Task<SplunkFeed<T>>> act, string json)
	{
		using var client = TestClient.Create(TestClient.Stub(json));
		var feed = await act(client);
		return feed.Entries.Should().ContainSingle().Subject;
	}

	/// <summary>Asserts that a 404 response raises <see cref="SplunkApiException"/> carrying Splunk's message.</summary>
	public static async Task ShouldRaiseNotFoundAsync(Func<SplunkClient, Task> act)
	{
		using var client = TestClient.Create(TestClient.Stub("""{"messages":[{"type":"ERROR","text":"Could not find object id=missing"}]}""", HttpStatusCode.NotFound));

		var thrown = await FluentActions.Awaiting(() => act(client)).Should().ThrowAsync<SplunkApiException>();

		thrown.Which.StatusCode.Should().Be(HttpStatusCode.NotFound);
		thrown.Which.Message.Should().Be("Could not find object id=missing");
	}

	/// <summary>
	/// Wraps one entry's content in the envelope Splunk 10.6 returns, with the links and ACL captured from the live
	/// instance (host names replaced).
	/// </summary>
	public static string Feed(string endpoint, string name, string content)
		=> $$"""
			{
				"links": { "create": "/servicesNS/nobody/search/{{endpoint}}/_new", "_reload": "/servicesNS/nobody/search/{{endpoint}}/_reload", "_acl": "/servicesNS/nobody/search/{{endpoint}}/_acl" },
				"origin": "https://splunk.test:8089/servicesNS/nobody/search/{{endpoint}}",
				"updated": "2026-10-08T13:53:31+00:00",
				"generator": { "build": "86587d4e3b27", "version": "10.6.0.5" },
				"entry": [
					{
						"name": "{{name}}",
						"id": "https://splunk.test:8089/servicesNS/nobody/search/{{endpoint}}/x",
						"updated": "2026-10-08T13:53:31+00:00",
						"links": { "alternate": "/servicesNS/nobody/search/{{endpoint}}/x", "list": "/servicesNS/nobody/search/{{endpoint}}/x", "edit": "/servicesNS/nobody/search/{{endpoint}}/x", "remove": "/servicesNS/nobody/search/{{endpoint}}/x" },
						"author": "admin",
						"acl": {
							"app": "search", "can_change_perms": true, "can_list": true, "can_share_app": true, "can_share_global": true,
							"can_share_user": true, "can_write": true, "modifiable": true, "owner": "admin",
							"perms": { "read": ["*"], "write": ["admin", "power"] }, "removable": true, "sharing": "global"
						},
						"fields": { "required": [], "optional": [], "wildcard": [] },
						"content": {{content}}
					}
				],
				"paging": { "total": 1, "perPage": 30, "offset": 0 },
				"messages": []
			}
			""";

	/// <summary>Asserts the envelope fields <see cref="Feed"/> writes.</summary>
	public static void ShouldBeTheCapturedEntry<T>(this SplunkEntry<T> entry, string name)
	{
		entry.Name.Should().Be(name);
		entry.Author.Should().Be("admin");
		entry.Acl!.App.Should().Be("search");
		entry.Acl.Sharing.Should().Be("global");
		entry.Acl.Permissions!.Write.Should().Equal("admin", "power");
	}
}
