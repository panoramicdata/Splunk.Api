using Splunk.Api.Models;
using Splunk.Api.Test.Support;

namespace Splunk.Api.Test.Groups;

public class DirectoryEntriesTests
{
	// Captured from Splunk Enterprise 10.6.0.5 (GET directory/{name}).
	private static readonly string EntryJson = KnowledgeTestKit.Feed("directory", "access-extractions", """
		{
			"disabled": false,
			"eai:acl": null,
			"eai:location": "/data/transforms/extractions",
			"eai:orphaned": false,
			"eai:type": "transforms-extract"
		}
		""");

	[Fact]
	public async Task ListAsync_SendsGetToDirectory()
		=> (await KnowledgeTestKit.SendInSearchAppAsync(c => c.DirectoryEntries.ListAsync(new ListOptions { Offset = 30 }, TestContext.Current.CancellationToken)))
			.ShouldBe(HttpMethod.Get, "/servicesNS/nobody/search/directory", "?offset=30&output_mode=json");

	[Fact]
	public async Task GetAsync_SendsGetToTheEntry()
		=> (await KnowledgeTestKit.SendAsync(c => c.DirectoryEntries.GetAsync("access-extractions", TestContext.Current.CancellationToken)))
			.ShouldBe(HttpMethod.Get, "/services/directory/access-extractions");

	[Fact]
	public async Task GetAsync_MapsEveryModelledField()
	{
		var entry = await KnowledgeTestKit.SingleEntryAsync(c => c.DirectoryEntries.GetAsync("access-extractions", TestContext.Current.CancellationToken), EntryJson);

		entry.ShouldBeTheCapturedEntry("access-extractions");
		entry.Content!.Type.Should().Be("transforms-extract");
		entry.Content.Location.Should().Be("/data/transforms/extractions");
		entry.Content.Orphaned.Should().BeFalse();
	}

	[Fact]
	public Task GetAsync_NotFound_RaisesSplunkApiException()
		=> KnowledgeTestKit.ShouldRaiseNotFoundAsync(c => c.DirectoryEntries.GetAsync("missing", TestContext.Current.CancellationToken));
}
