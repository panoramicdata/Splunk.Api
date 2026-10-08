using Splunk.Api.Models;
using Splunk.Api.Models.Knowledge;
using Splunk.Api.Test.Support;

namespace Splunk.Api.Test.Groups;

public class MonitoringConsoleBookmarksTests
{
	private const string Url = "https://deployment-2.example.com:8000/en-US/app/splunk_monitoring_console";

	// Captured from Splunk Enterprise 10.6.0.5 (GET saved/bookmarks/monitoring_console), names changed.
	private static readonly string BookmarkJson = KnowledgeTestKit.Feed("saved/bookmarks/monitoring_console", "deployment-2", $$"""
		{
			"eai:acl": null,
			"url": "{{Url}}"
		}
		""");

	[Fact]
	public async Task ListAsync_SendsSorting()
		=> (await KnowledgeTestKit.SendAsync(c => c.MonitoringConsoleBookmarks.ListAsync(new ListOptions { SortKey = "url", SortDirection = SortDirection.Descending }, TestContext.Current.CancellationToken)))
			.ShouldBe(HttpMethod.Get, "/services/saved/bookmarks/monitoring_console", "?sort_key=url&sort_dir=desc&output_mode=json");

	[Fact]
	public async Task CreateAsync_PostsNameAndUrl()
		=> (await KnowledgeTestKit.SendAsync(c => c.MonitoringConsoleBookmarks.CreateAsync(new MonitoringConsoleBookmarkCreateRequest { Name = "deployment-2", Url = Url }, TestContext.Current.CancellationToken)))
			.ShouldBe(
				HttpMethod.Post,
				"/services/saved/bookmarks/monitoring_console",
				body: "name=deployment-2&url=https%3A%2F%2Fdeployment-2.example.com%3A8000%2Fen-US%2Fapp%2Fsplunk_monitoring_console");

	[Fact]
	public async Task DeleteAsync_SendsDeleteToTheBookmark()
		=> (await KnowledgeTestKit.SendAsync(c => c.MonitoringConsoleBookmarks.DeleteAsync("deployment-2", TestContext.Current.CancellationToken)))
			.ShouldBe(HttpMethod.Delete, "/services/saved/bookmarks/monitoring_console/deployment-2");

	[Fact]
	public async Task ListAsync_MapsEveryModelledField()
	{
		var entry = await KnowledgeTestKit.SingleEntryAsync(c => c.MonitoringConsoleBookmarks.ListAsync(null, TestContext.Current.CancellationToken), BookmarkJson);

		entry.ShouldBeTheCapturedEntry("deployment-2");
		entry.Content!.Url.Should().Be(Url);
	}

	[Fact]
	public Task DeleteAsync_NotFound_RaisesSplunkApiException()
		=> KnowledgeTestKit.ShouldRaiseNotFoundAsync(c => c.MonitoringConsoleBookmarks.DeleteAsync("missing", TestContext.Current.CancellationToken));
}
