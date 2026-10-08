using Splunk.Api.Models;
using Splunk.Api.Models.Knowledge;

namespace Splunk.Api.IntegrationTest.Knowledge;

/// <summary>Reads the directory service and round trips a monitoring console bookmark.</summary>
[Collection(SplunkTestGroup.Name)]
public sealed class DirectoryAndBookmarksIntegrationTests(SplunkFixture fixture)
{
	private static CancellationToken Token => TestContext.Current.CancellationToken;

	[Fact]
	public async Task Directory_ListsAndGetsAnEntry()
	{
		using var system = fixture.Client.InNamespace("nobody", "system");

		var listed = await system.DirectoryEntries.ListAsync(new ListOptions { Count = 5 }, Token);
		listed.Entries.Should().HaveCount(5).And.OnlyContain(e => e.Content!.Type != null && e.Content.Location != null);

		var entry = (await system.DirectoryEntries.GetAsync("access-extractions", Token)).Entries.Should().ContainSingle().Subject;
		entry.Content!.Type.Should().Be("transforms-extract");
		entry.Content.Location.Should().Be("/data/transforms/extractions");
		entry.Content.Orphaned.Should().BeFalse();
	}

	[Fact]
	public async Task Bookmark_CreateListDelete()
	{
		var name = $"{SplunkFixture.Prefix}{Guid.NewGuid():N}"[..25];
		const string Url = "https://deployment-2.example.com:8000/en-US/app/splunk_monitoring_console";
		try
		{
			var created = await fixture.Client.MonitoringConsoleBookmarks.CreateAsync(new MonitoringConsoleBookmarkCreateRequest { Name = name, Url = Url }, Token);
			created.Entries.Should().ContainSingle().Which.Content!.Url.Should().Be(Url);

			var listed = await fixture.Client.MonitoringConsoleBookmarks.ListAsync(new ListOptions { Search = name, SortKey = "url", SortDirection = SortDirection.Ascending }, Token);
			listed.Entries.Should().ContainSingle().Which.Name.Should().Be(name);

			await fixture.Client.MonitoringConsoleBookmarks.DeleteAsync(name, Token);
		}
		finally
		{
			await Cleanup.IgnoreMissingAsync(() => fixture.Client.MonitoringConsoleBookmarks.DeleteAsync(name, CancellationToken.None));
		}

		(await fixture.Client.MonitoringConsoleBookmarks.ListAsync(new ListOptions { Search = name }, Token)).Entries.Should().BeEmpty();
	}
}
