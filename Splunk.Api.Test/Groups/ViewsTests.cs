using Splunk.Api.Models;
using Splunk.Api.Models.Knowledge;
using Splunk.Api.Test.Support;

namespace Splunk.Api.Test.Groups;

public class ViewsTests
{
	private const string ViewsPath = "/servicesNS/nobody/search/data/ui/views";
	private const string Dashboard = "<dashboard version=\"1.1\"><label>Errors</label></dashboard>";
	private const string EncodedDashboard = "%3Cdashboard+version%3D%221.1%22%3E%3Clabel%3EErrors%3C%2Flabel%3E%3C%2Fdashboard%3E";

	// Captured from Splunk Enterprise 10.6.0.5 (GET data/ui/views/{name}), names changed; description and suite added.
	private static readonly string ViewJson = KnowledgeTestKit.Feed("data/ui/views", "errors", """
		{
			"applicationSuite": "core",
			"dashboardType": 0,
			"description": "Errors by host",
			"disabled": false,
			"eai:acl": null,
			"eai:appName": "search",
			"eai:data": "<dashboard version=\"1.1\"><label>Errors</label></dashboard>",
			"eai:digest": "f2d963edd61136dd5009fb281f8e5285",
			"eai:type": "views",
			"eai:userName": "nobody",
			"embed.enabled": false,
			"embed.expiry": 0,
			"isDashboard": true,
			"isVisible": true,
			"label": "Errors",
			"rootNode": "dashboard",
			"version": "1.1"
		}
		""");

	// Captured from Splunk Enterprise 10.6.0.5 (GET data/ui/views/{name}/revision).
	private static readonly string RevisionJson = KnowledgeTestKit.Feed("data/ui/views", "revision", """
		{
			"eai:acl": null,
			"eai:data": "<dashboard version=\"1.1\"><label>Errors</label></dashboard>",
			"email": "admin",
			"message": "second",
			"sha": "c5c2eb05bfbbf370409bf47143bbe5ee9cf8f88f",
			"time": "2026-10-08T13:58:42+00:00",
			"user": "admin"
		}
		""");

	private static Task<RecordedCall> SendAsync(Func<Interfaces.IViews, Task> act)
		=> KnowledgeTestKit.SendInSearchAppAsync(c => act(c.Views));

	[Fact]
	public async Task ListAsync_SendsGetWithTheFilter()
		=> (await SendAsync(v => v.ListAsync(new ListOptions { Search = "isDashboard=1" }, TestContext.Current.CancellationToken)))
			.ShouldBe(HttpMethod.Get, ViewsPath, "?search=isDashboard%3D1&output_mode=json");

	[Fact]
	public async Task CreateAsync_PostsNameAndSource()
		=> (await SendAsync(v => v.CreateAsync(new ViewCreateRequest { Name = "errors", Data = Dashboard }, TestContext.Current.CancellationToken)))
			.ShouldBe(HttpMethod.Post, ViewsPath, body: "name=errors&eai%3Adata=" + EncodedDashboard);

	[Fact]
	public async Task GetAsync_SendsGetToTheView()
		=> (await SendAsync(v => v.GetAsync("errors", TestContext.Current.CancellationToken)))
			.ShouldBe(HttpMethod.Get, ViewsPath + "/errors");

	[Fact]
	public async Task UpdateAsync_PostsSourceAndChangeLog()
		=> (await SendAsync(v => v.UpdateAsync("errors", new ViewUpdateRequest { Data = Dashboard, ChangeLog = "Second version" }, TestContext.Current.CancellationToken)))
			.ShouldBe(HttpMethod.Post, ViewsPath + "/errors", body: "eai%3Adata=" + EncodedDashboard + "&eai%3Achangelog=Second+version");

	[Fact]
	public async Task DeleteAsync_SendsDeleteWithoutBody()
		=> (await SendAsync(v => v.DeleteAsync("errors", TestContext.Current.CancellationToken)))
			.ShouldBe(HttpMethod.Delete, ViewsPath + "/errors");

	[Fact]
	public async Task DeleteAsync_WithChangeLog_SendsItAsAFormBody()
		=> (await SendAsync(v => v.DeleteAsync("errors", new ViewDeleteRequest { ChangeLog = "Removed" }, TestContext.Current.CancellationToken)))
			.ShouldBe(HttpMethod.Delete, ViewsPath + "/errors", body: "eai%3Achangelog=Removed");

	[Fact]
	public async Task DisableAsync_PostsToDisable()
		=> (await SendAsync(v => v.DisableAsync("errors", TestContext.Current.CancellationToken)))
			.ShouldBe(HttpMethod.Post, ViewsPath + "/errors/disable");

	[Fact]
	public async Task EnableAsync_PostsToEnable()
		=> (await SendAsync(v => v.EnableAsync("errors", TestContext.Current.CancellationToken)))
			.ShouldBe(HttpMethod.Post, ViewsPath + "/errors/enable");

	[Fact]
	public async Task ListHistoryAsync_SendsGetToHistory()
		=> (await SendAsync(v => v.ListHistoryAsync("errors", TestContext.Current.CancellationToken)))
			.ShouldBe(HttpMethod.Get, ViewsPath + "/errors/history");

	[Fact]
	public async Task ListHistoryWithMessagesAsync_SendsTheEmptyFlag()
		=> (await SendAsync(v => v.ListHistoryWithMessagesAsync("errors", TestContext.Current.CancellationToken)))
			.ShouldBe(HttpMethod.Get, ViewsPath + "/errors/history", "?with_message=&output_mode=json");

	[Fact]
	public async Task GetRevisionAsync_SendsTheRevisionIdInTheQuery()
		=> (await SendAsync(v => v.GetRevisionAsync("errors", "c5c2eb05bfbbf370409bf47143bbe5ee9cf8f88f", TestContext.Current.CancellationToken)))
			.ShouldBe(HttpMethod.Get, ViewsPath + "/errors/revision", "?revision_id=c5c2eb05bfbbf370409bf47143bbe5ee9cf8f88f&output_mode=json");

	[Fact]
	public async Task GetAsync_MapsEveryModelledField()
	{
		var entry = await KnowledgeTestKit.SingleEntryAsync(c => c.Views.GetAsync("errors", TestContext.Current.CancellationToken), ViewJson);

		entry.ShouldBeTheCapturedEntry("errors");
		var view = entry.Content!;
		view.Data.Should().Be(Dashboard);
		view.Digest.Should().Be("f2d963edd61136dd5009fb281f8e5285");
		view.Type.Should().Be("views");
		view.DashboardType.Should().Be(0);
		view.Label.Should().Be("Errors");
		view.Description.Should().Be("Errors by host");
		view.IsDashboard.Should().BeTrue();
		view.IsVisible.Should().BeTrue();
		view.RootNode.Should().Be("dashboard");
		view.Version.Should().Be("1.1");
		view.ApplicationSuite.Should().Be("core");
		view.EmbedEnabled.Should().BeFalse();
		view.EmbedExpiry.Should().Be(0);
		view.Disabled.Should().BeFalse();
	}

	[Fact]
	public async Task GetRevisionAsync_MapsEveryModelledField()
	{
		var entry = await KnowledgeTestKit.SingleEntryAsync(c => c.Views.GetRevisionAsync("errors", "c5c2", TestContext.Current.CancellationToken), RevisionJson);

		entry.Name.Should().Be("revision");
		var revision = entry.Content!;
		revision.Sha.Should().Be("c5c2eb05bfbbf370409bf47143bbe5ee9cf8f88f");
		revision.Message.Should().Be("second");
		revision.User.Should().Be("admin");
		revision.Email.Should().Be("admin");
		revision.Time.Should().Be(new DateTimeOffset(2026, 10, 8, 13, 58, 42, TimeSpan.Zero));
		revision.Data.Should().Be(Dashboard);
	}

	[Fact]
	public Task GetAsync_NotFound_RaisesSplunkApiException()
		=> KnowledgeTestKit.ShouldRaiseNotFoundAsync(c => c.Views.GetAsync("missing", TestContext.Current.CancellationToken));
}
