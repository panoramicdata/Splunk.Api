using Splunk.Api.Models.Cluster;
using Splunk.Api.Test.Support;
using System.Net;

namespace Splunk.Api.Test.Groups;

public class ConfigurationReplicationTests
{
	private const string Path = "/services/replication/configuration";

	[Fact]
	public async Task GetHealthAsync_SendsGetWithTheChecks()
		=> (await RequestProbe.SendAsync((c, ct) => c.ConfigurationReplication.GetHealthAsync(
			new ConfigurationReplicationHealthOptions { Bookmark = true, CheckShareBaseline = false, Unpublished = true },
			ct)))
			.ShouldBeProbed(HttpMethod.Get, $"{Path}/health", "?bookmark=true&check_share_baseline=false&unpublished=true&output_mode=json");

	[Fact]
	public async Task ListQuarantinedAssetsAsync_SendsGet()
		=> (await RequestProbe.SendAsync((c, ct) => c.ConfigurationReplication.ListQuarantinedAssetsAsync(ct)))
			.ShouldBeProbed(HttpMethod.Get, $"{Path}/quarantined-assets");

	[Fact]
	public async Task GetHealthAsync_MapsABaselineCheck()
	{
		// From the reference's examples.
		var health = await RequestProbe.ReadContentAsync(
			(c, ct) => c.ConfigurationReplication.GetHealthAsync(null, ct),
			"https://sh2:8089",
			"""{"check_share_baseline":"Yes","server_name":"sh2","Number of unpublished changes":"0"}""");

		health.CheckShareBaseline.Should().Be("Yes");
		health.ServerName.Should().Be("sh2");
		health.UnpublishedChanges.Should().Be("0");
	}

	[Fact]
	public async Task ListQuarantinedAssetsAsync_MapsEveryModelledField()
	{
		var asset = await RequestProbe.ReadContentAsync(
			(c, ct) => c.ConfigurationReplication.ListQuarantinedAssetsAsync(ct),
			"quarantined-assets",
			"""
			{
				"assetId": "b4c9340713a5dd8c61105b05acea79fbbd3fc98d",
				"assetURI": "/nobody/search/lookups/test.csv",
				"user": "nobody",
				"app": "search",
				"assetType": "lookups",
				"assetName": "test.csv",
				"quarantineInfo": "[ {quarantined_at_host=https://sh1:8089, quarantined_at=1724885036, lookup_size=30246329, quarantine_reason=large_lookup} ]"
			}
			""");

		asset.AssetId.Should().Be("b4c9340713a5dd8c61105b05acea79fbbd3fc98d");
		asset.AssetUri.Should().Be("/nobody/search/lookups/test.csv");
		asset.User.Should().Be("nobody");
		asset.App.Should().Be("search");
		asset.AssetType.Should().Be("lookups");
		asset.AssetName.Should().Be("test.csv");
		asset.QuarantineInfo.Should().Contain("quarantine_reason=large_lookup");
	}

	[Fact]
	public Task GetHealthAsync_NotAShcMember_RaisesBadRequest()
		=> RequestProbe.FailsAsync(
			(c, ct) => c.ConfigurationReplication.GetHealthAsync(null, ct),
			HttpStatusCode.BadRequest,
			"""{"messages":[{"type":"ERROR","text":"No local ConfRepo registered"}]}""",
			"No local ConfRepo registered");
}
