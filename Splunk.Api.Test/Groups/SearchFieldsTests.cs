using Splunk.Api.Models.Knowledge;
using Splunk.Api.Test.Support;

namespace Splunk.Api.Test.Groups;

public class SearchFieldsTests
{
	// Captured from Splunk Enterprise 10.6.0.5 (GET search/fields/{field_name}): the content is a string.
	private const string FieldJson = """
		{
			"links": {},
			"origin": "/services/search/fields",
			"updated": "2026-10-08T13:52:31+00:00",
			"generator": { "build": "86587d4e3b27", "version": "10.6.0.5" },
			"entry": [
				{
					"name": "sourcetype",
					"id": "/services/search/fields/sourcetype",
					"updated": "1970-01-01T00:00:00+00:00",
					"links": { "alternate": "/services/search/fields/sourcetype" },
					"content": "PropertiesMap: {INDEXED -> 'True' INDEXED_VALUE -> 'False' TOKENIZER -> ''}"
				}
			]
		}
		""";

	// Captured from Splunk Enterprise 10.6.0.5 (GET search/fields/{field_name}/tags), names changed.
	private const string FieldTagsJson = """
		{
			"links": {},
			"origin": "/services/search/fields/host/tags",
			"updated": "2026-10-08T13:57:04+00:00",
			"generator": { "build": "86587d4e3b27", "version": "10.6.0.5" },
			"entry": [ { "name": "web01::production", "id": "/services/search/fields/host/tags/web01::production", "updated": "1970-01-01T00:00:00+00:00" } ]
		}
		""";

	[Fact]
	public async Task ListAsync_SendsGetToFields()
		=> (await KnowledgeTestKit.SendAsync(c => c.SearchFields.ListAsync(TestContext.Current.CancellationToken)))
			.ShouldBe(HttpMethod.Get, "/services/search/fields");

	[Fact]
	public async Task GetAsync_SendsGetToTheField()
		=> (await KnowledgeTestKit.SendAsync(c => c.SearchFields.GetAsync("sourcetype", TestContext.Current.CancellationToken)))
			.ShouldBe(HttpMethod.Get, "/services/search/fields/sourcetype");

	[Fact]
	public async Task ListTagsAsync_SendsGetToTheFieldsTags()
		=> (await KnowledgeTestKit.SendAsync(c => c.SearchFields.ListTagsAsync("host", TestContext.Current.CancellationToken)))
			.ShouldBe(HttpMethod.Get, "/services/search/fields/host/tags");

	[Fact]
	public async Task UpdateTagsAsync_PostsValueAndRepeatedTags()
		=> (await KnowledgeTestKit.SendAsync(c => c.SearchFields.UpdateTagsAsync(
				"host",
				new FieldTagsUpdateRequest { Value = "web01", Add = ["production", "web"], Delete = ["staging"] },
				TestContext.Current.CancellationToken)))
			.ShouldBe(HttpMethod.Post, "/services/search/fields/host/tags", body: "value=web01&add=production&add=web&delete=staging");

	[Fact]
	public async Task GetAsync_ReadsTheContentAsText()
	{
		using var client = TestClient.Create(TestClient.Stub(FieldJson));

		var feed = await client.SearchFields.GetAsync("sourcetype", TestContext.Current.CancellationToken);

		var entry = feed.Entries.Should().ContainSingle().Subject;
		entry.Name.Should().Be("sourcetype");
		entry.Content.Should().Be("PropertiesMap: {INDEXED -> 'True' INDEXED_VALUE -> 'False' TOKENIZER -> ''}");
		feed.Paging.Should().BeNull();
	}

	[Fact]
	public async Task ListTagsAsync_MapsTheValueTagPairs()
	{
		var entry = await KnowledgeTestKit.SingleEntryAsync(c => c.SearchFields.ListTagsAsync("host", TestContext.Current.CancellationToken), FieldTagsJson);

		entry.Name.Should().Be("web01::production");
		entry.Content.Should().BeNull();
	}

	[Fact]
	public Task GetAsync_NotFound_RaisesSplunkApiException()
		=> KnowledgeTestKit.ShouldRaiseNotFoundAsync(c => c.SearchFields.GetAsync("missing", TestContext.Current.CancellationToken));
}
