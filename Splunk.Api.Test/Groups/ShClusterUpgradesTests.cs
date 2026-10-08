using Splunk.Api.Test.Support;
using System.Net;

namespace Splunk.Api.Test.Groups;

public class ShClusterUpgradesTests
{
	private const string Path = "/services/upgrade/shc";

	// From the reference's JSON examples (props layout, not the usual feed).
	private const string ResultJson = """
		{
			"updated": "2022-11-24T17:25:54+0000",
			"author": "Splunk",
			"layout": "props",
			"entry": [
				{
					"title": "upgrade",
					"id": "/services/upgrade/shc/upgrade",
					"updated": "2022-11-24T17:25:54+0000",
					"links": { "alternate": { "href": "shc/upgrade" } },
					"content": { "message": "Upgrade initiated", "status": "succeeded" }
				}
			]
		}
		""";

	private const string StatusJson = """
		{
			"updated": "2022-11-24T17:33:28+0000",
			"author": "Splunk",
			"layout": "props",
			"entry": [
				{
					"title": "status",
					"id": "/services/upgrade/shc/status",
					"updated": "2022-11-24T17:33:28+0000",
					"links": { "alternate": { "href": "shc/status" } },
					"content": {
						"message": {
							"upgrade_status": "completed",
							"statistics": { "peers_to_upgrade": 3, "overall_peers_upgraded": 3, "overall_peers_upgraded_percentage": 100 },
							"peers": [{ "name": "sh2", "status": "upgraded", "last_modified": "Thu Nov 24 17:29:41 2022" }]
						}
					}
				}
			]
		}
		""";

	[Fact]
	public async Task StartAsync_SendsPost()
		=> (await RequestProbe.SendAsync((c, ct) => c.ShClusterUpgrades.StartAsync(ct), ResultJson))
			.ShouldBeProbed(HttpMethod.Post, $"{Path}/upgrade");

	[Fact]
	public async Task GetStatusAsync_SendsGet()
		=> (await RequestProbe.SendAsync((c, ct) => c.ShClusterUpgrades.GetStatusAsync(ct), StatusJson))
			.ShouldBeProbed(HttpMethod.Get, $"{Path}/status");

	[Fact]
	public async Task RecoverAsync_SendsPost()
		=> (await RequestProbe.SendAsync((c, ct) => c.ShClusterUpgrades.RecoverAsync(ct), ResultJson))
			.ShouldBeProbed(HttpMethod.Post, $"{Path}/recovery");

	[Fact]
	public async Task StartAsync_MapsTheResponse()
	{
		var response = await RequestProbe.ReadAsync((c, ct) => c.ShClusterUpgrades.StartAsync(ct), ResultJson);

		response.Updated.Should().Be("2022-11-24T17:25:54+0000");
		response.Author.Should().Be("Splunk");
		response.Layout.Should().Be("props");
		var entry = response.Entries.Should().ContainSingle().Subject;
		entry.Title.Should().Be("upgrade");
		entry.Id.Should().Be("/services/upgrade/shc/upgrade");
		entry.Updated.Should().Be("2022-11-24T17:25:54+0000");
		entry.Content!.Message.Should().Be("Upgrade initiated");
		entry.Content.Status.Should().Be("succeeded");
	}

	[Fact]
	public async Task GetStatusAsync_MapsTheProgress()
	{
		var response = await RequestProbe.ReadAsync((c, ct) => c.ShClusterUpgrades.GetStatusAsync(ct), StatusJson);

		var progress = response.Entries.Should().ContainSingle().Subject.Content!.Message!;
		progress.UpgradeStatus.Should().Be("completed");
		progress.Statistics!.PeersToUpgrade.Should().Be(3);
		progress.Statistics.OverallPeersUpgraded.Should().Be(3);
		progress.Statistics.OverallPeersUpgradedPercentage.Should().Be(100);
		var peer = progress.Peers.Should().ContainSingle().Subject;
		peer.Name.Should().Be("sh2");
		peer.Status.Should().Be("upgraded");
		peer.LastModified.Should().Be("Thu Nov 24 17:29:41 2022");
	}

	[Fact]
	public Task GetStatusAsync_NotConfigured_RaisesTheXmlError()
		=> RequestProbe.FailsAsync(
			(c, ct) => c.ShClusterUpgrades.GetStatusAsync(ct),
			HttpStatusCode.BadRequest,
			"""<?xml version="1.0" encoding="UTF-8"?><response>  <messages>    <msg type="ERROR">Configuration error: 'passAuth' does not exist</msg>  </messages></response>""",
			"Configuration error: 'passAuth' does not exist");
}
