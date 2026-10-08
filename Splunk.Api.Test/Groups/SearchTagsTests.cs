using Splunk.Api.Models.Knowledge;
using Splunk.Api.Test.Support;

namespace Splunk.Api.Test.Groups;

public class SearchTagsTests
{
	// Captured from Splunk Enterprise 10.6.0.5 (GET search/tags/{tag_name}).
	private const string TagJson = """
		{
			"links": {},
			"origin": "/services/search/tags/account",
			"updated": "2026-10-08T13:52:30+00:00",
			"generator": { "build": "86587d4e3b27", "version": "10.6.0.5" },
			"entry": [ { "name": "eventtype::cim:audit_account", "id": "/services/search/tags/account#eventtype::cim:audit_account", "updated": "1970-01-01T00:00:00+00:00" } ]
		}
		""";

	// Captured from Splunk Enterprise 10.6.0.5 (POST search/tags/{tag_name}, 201).
	private const string ProcessedJson = """{"links":{},"origin":"/services/search/tags/production","updated":"2026-10-08T13:57:03+00:00","generator":{"build":"86587d4e3b27","version":"10.6.0.5"},"entry":[],"messages":[{"type":"INFO","text":"Processed adds/deletes for tag"}]}""";

	[Fact]
	public async Task ListAsync_SendsGetToTags()
		=> (await KnowledgeTestKit.SendAsync(c => c.SearchTags.ListAsync(TestContext.Current.CancellationToken)))
			.ShouldBe(HttpMethod.Get, "/services/search/tags");

	[Fact]
	public async Task GetAsync_SendsGetToTheTag()
		=> (await KnowledgeTestKit.SendAsync(c => c.SearchTags.GetAsync("account", TestContext.Current.CancellationToken)))
			.ShouldBe(HttpMethod.Get, "/services/search/tags/account");

	[Fact]
	public async Task UpdateAsync_PostsRepeatedPairs()
		=> (await KnowledgeTestKit.SendAsync(
				c => c.SearchTags.UpdateAsync("production", new TagUpdateRequest { Add = ["host::web01", "eventtype::web"], Delete = ["host::web02"] }, TestContext.Current.CancellationToken),
				ProcessedJson))
			.ShouldBe(HttpMethod.Post, "/services/search/tags/production", body: "add=host%3A%3Aweb01&add=eventtype%3A%3Aweb&delete=host%3A%3Aweb02");

	[Fact]
	public async Task DeleteAsync_SendsDelete()
		=> (await KnowledgeTestKit.SendAsync(c => c.SearchTags.DeleteAsync("production", TestContext.Current.CancellationToken)))
			.ShouldBe(HttpMethod.Delete, "/services/search/tags/production");

	[Fact]
	public async Task GetAsync_MapsTheFieldValuePairs()
	{
		var entry = await KnowledgeTestKit.SingleEntryAsync(c => c.SearchTags.GetAsync("account", TestContext.Current.CancellationToken), TagJson);

		entry.Name.Should().Be("eventtype::cim:audit_account");
		entry.Id.Should().Be("/services/search/tags/account#eventtype::cim:audit_account");
		entry.Content.Should().BeNull();
	}

	[Fact]
	public async Task UpdateAsync_ReturnsSplunksMessage()
	{
		using var client = TestClient.Create(TestClient.Stub(ProcessedJson));

		var feed = await client.SearchTags.UpdateAsync("production", new TagUpdateRequest { Add = ["host::web01"] }, TestContext.Current.CancellationToken);

		feed.Entries.Should().BeEmpty();
		feed.Messages.Should().ContainSingle().Which.Text.Should().Be("Processed adds/deletes for tag");
	}

	[Fact]
	public Task DeleteAsync_NotFound_RaisesSplunkApiException()
		=> KnowledgeTestKit.ShouldRaiseNotFoundAsync(c => c.SearchTags.DeleteAsync("missing", TestContext.Current.CancellationToken));
}
