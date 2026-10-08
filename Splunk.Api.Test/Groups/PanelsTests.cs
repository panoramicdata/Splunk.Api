using Splunk.Api.Models.Knowledge;
using Splunk.Api.Test.Support;

namespace Splunk.Api.Test.Groups;

public class PanelsTests
{
	// Captured from Splunk Enterprise 10.6.0.5 (GET data/ui/panels), names changed.
	private static readonly string PanelJson = KnowledgeTestKit.Feed("data/ui/panels", "errors_panel", """
		{
			"disabled": false,
			"eai:acl": null,
			"eai:appName": "search",
			"eai:data": "<panel><label>Errors</label><title>Errors by host</title></panel>",
			"eai:digest": "bb708de2212d30a29a2bcbb8238b1654",
			"eai:userName": "nobody",
			"label": "Errors",
			"panel.title": "Errors by host",
			"rootNode": "panel"
		}
		""");

	[Fact]
	public async Task ListAsync_SendsGetInTheNamespace()
		=> (await KnowledgeTestKit.SendInSearchAppAsync(c => c.Panels.ListAsync(null, TestContext.Current.CancellationToken)))
			.ShouldBe(HttpMethod.Get, "/servicesNS/nobody/search/data/ui/panels");

	[Fact]
	public async Task CreateAsync_PostsNameAndXml()
		=> (await KnowledgeTestKit.SendInSearchAppAsync(c => c.Panels.CreateAsync(new PanelCreateRequest { Name = "errors_panel", Data = "<panel><label>Errors</label></panel>" }, TestContext.Current.CancellationToken)))
			.ShouldBe(HttpMethod.Post, "/servicesNS/nobody/search/data/ui/panels", body: "name=errors_panel&eai%3Adata=%3Cpanel%3E%3Clabel%3EErrors%3C%2Flabel%3E%3C%2Fpanel%3E");

	[Fact]
	public async Task ListAsync_MapsEveryModelledField()
	{
		var entry = await KnowledgeTestKit.SingleEntryAsync(c => c.Panels.ListAsync(null, TestContext.Current.CancellationToken), PanelJson);

		entry.ShouldBeTheCapturedEntry("errors_panel");
		var panel = entry.Content!;
		panel.Data.Should().Be("<panel><label>Errors</label><title>Errors by host</title></panel>");
		panel.Digest.Should().Be("bb708de2212d30a29a2bcbb8238b1654");
		panel.Label.Should().Be("Errors");
		panel.Title.Should().Be("Errors by host");
		panel.RootNode.Should().Be("panel");
	}

	[Fact]
	public Task ListAsync_Error_RaisesSplunkApiException()
		=> KnowledgeTestKit.ShouldRaiseNotFoundAsync(c => c.Panels.ListAsync(null, TestContext.Current.CancellationToken));
}
