using Splunk.Api.Models.Introspection;
using Splunk.Api.Test.Support.Platform;
using System.Net;

namespace Splunk.Api.Test.Groups;

public class MonitoringConsoleBookmarksTests
{
	private const string Path = "/services/saved/bookmarks/monitoring_console";

	[Fact]
	public async Task ListAsync_SendsGet()
		=> await Calls.AssertAsync(c => c.MonitoringConsoleBookmarks.ListAsync(null, Calls.Token), HttpMethod.Get, Path, Calls.JsonQuery, null);

	[Fact]
	public async Task CreateAsync_PostsTheNameAndUrl()
		=> await Calls.AssertAsync(
			c => c.MonitoringConsoleBookmarks.CreateAsync(
				new MonitoringConsoleBookmarkCreateRequest { Name = "prod", Url = "https://mc.example.com/splunk_monitoring_console" },
				Calls.Token),
			HttpMethod.Post, Path, Calls.JsonQuery, "name=prod&url=https%3A%2F%2Fmc.example.com%2Fsplunk_monitoring_console");

	[Fact]
	public async Task DeleteAsync_SendsDeleteForTheBookmark()
		=> await Calls.AssertAsync(c => c.MonitoringConsoleBookmarks.DeleteAsync("prod", Calls.Token), HttpMethod.Delete, Path + "/prod", Calls.JsonQuery, null);

	[Fact]
	public async Task CreateAsync_MapsTheUrl()
	{
		// Captured from Splunk 10.6.0.5.
		var feed = await Calls.MapAsync(
			c => c.MonitoringConsoleBookmarks.CreateAsync(new MonitoringConsoleBookmarkCreateRequest { Name = "prod", Url = "u" }, Calls.Token),
			Feed.Of("prod", """{"eai:acl":null,"url":"https://mc.example.com/splunk_monitoring_console"}"""));

		feed.Entries.Should().ContainSingle().Which.Content!.Url.Should().Be("https://mc.example.com/splunk_monitoring_console");
	}

	[Fact]
	public async Task CreateAsync_Error_RaisesSplunkApiException()
		=> await Calls.AssertErrorAsync(
			c => c.MonitoringConsoleBookmarks.CreateAsync(new MonitoringConsoleBookmarkCreateRequest { Name = "prod", Url = "u" }, Calls.Token),
			HttpStatusCode.BadRequest);
}
