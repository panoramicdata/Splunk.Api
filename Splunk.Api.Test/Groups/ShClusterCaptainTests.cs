using Splunk.Api.Models.Cluster;
using Splunk.Api.Test.Support;
using System.Net;

namespace Splunk.Api.Test.Groups;

public class ShClusterCaptainTests
{
	private const string Path = "/services/shcluster/captain";

	// From the reference's example.
	private const string InfoContent = """
		{
			"eai:acl": null,
			"elected_captain": "1413307273",
			"id": "BB3116C0-73B9-459A-B473-254A18A69776",
			"initialized_flag": "1",
			"label": "searchhead",
			"maintenance_mode": "0",
			"min_peers_joined_flag": "1",
			"peer_scheme_host_port": "https://sh1.example.com:8089",
			"rolling_restart_flag": "0",
			"service_ready_flag": "1",
			"start_time": "1413307203"
		}
		""";

	[Fact]
	public async Task GetInfoAsync_SendsGet()
		=> (await RequestProbe.SendAsync((c, ct) => c.ShClusterCaptain.GetInfoAsync(ct)))
			.ShouldBeProbed(HttpMethod.Get, $"{Path}/info");

	[Fact]
	public async Task RestartAsync_SendsPostWithTheOptions()
		=> (await RequestProbe.SendAsync((c, ct) => c.ShClusterCaptain.RestartAsync(
			new ShClusterRestartRequest { Searchable = true, Force = true, DecommissionSearchJobsWaitSeconds = 30 },
			ct)))
			.ShouldBeProbed(HttpMethod.Post, $"{Path}/control/default/restart", body: "searchable=true&force=true&decommission_search_jobs_wait_secs=30");

	[Fact]
	public async Task RotateSplunkSecretAsync_SendsPost()
		=> (await RequestProbe.SendAsync((c, ct) => c.ShClusterCaptain.RotateSplunkSecretAsync(ct)))
			.ShouldBeProbed(HttpMethod.Post, $"{Path}/control/control/rotate-splunk-secret");

	[Fact]
	public async Task InitializeUpgradeAsync_SendsPost()
		=> (await RequestProbe.SendAsync((c, ct) => c.ShClusterCaptain.InitializeUpgradeAsync(ct)))
			.ShouldBeProbed(HttpMethod.Post, $"{Path}/control/control/upgrade-init");

	[Fact]
	public async Task FinalizeUpgradeAsync_SendsPost()
		=> (await RequestProbe.SendAsync((c, ct) => c.ShClusterCaptain.FinalizeUpgradeAsync(ct)))
			.ShouldBeProbed(HttpMethod.Post, $"{Path}/control/control/upgrade-finalize");

	[Fact]
	public async Task GetInfoAsync_MapsEveryModelledField()
	{
		var info = await RequestProbe.ReadContentAsync((c, ct) => c.ShClusterCaptain.GetInfoAsync(ct), "captain", InfoContent);

		info.ElectedCaptain.Should().Be(DateTimeOffset.FromUnixTimeSeconds(1413307273));
		info.ClusterId.Should().Be("BB3116C0-73B9-459A-B473-254A18A69776");
		info.InitializedFlag.Should().BeTrue();
		info.Label.Should().Be("searchhead");
		info.MaintenanceMode.Should().BeFalse();
		info.MinPeersJoinedFlag.Should().BeTrue();
		info.PeerSchemeHostPort.Should().Be("https://sh1.example.com:8089");
		info.RollingRestartFlag.Should().BeFalse();
		info.ServiceReadyFlag.Should().BeTrue();
		info.StartTime.Should().Be(DateTimeOffset.FromUnixTimeSeconds(1413307203));
	}

	[Fact]
	public async Task RestartAsync_MapsAFailedResult()
	{
		var result = await RequestProbe.ReadContentAsync(
			(c, ct) => c.ShClusterCaptain.RestartAsync(new ShClusterRestartRequest(), ct),
			"restart",
			"""{"msg":"Searchable rolling restarted cannot be started without captain status = Up","success":"0"}""");

		result.Success.Should().BeFalse();
		result.Message.Should().StartWith("Searchable rolling restarted cannot be started");
	}

	[Fact]
	public async Task InitializeUpgradeAsync_MapsTheUpgradeFlag()
	{
		var result = await RequestProbe.ReadContentAsync((c, ct) => c.ShClusterCaptain.InitializeUpgradeAsync(ct), "upgrade-init", """{"success":"1","upgrade":"yes"}""");

		result.Success.Should().BeTrue();
		result.Upgrade.Should().Be("yes");
	}

	[Fact]
	public Task GetInfoAsync_NoShc_RaisesServiceUnavailable()
		=> RequestProbe.FailsAsync(
			(c, ct) => c.ShClusterCaptain.GetInfoAsync(ct),
			HttpStatusCode.ServiceUnavailable,
			ClusterErrors.ShcNotEnabled,
			"Search Head Clustering is not enabled on this node. REST endpoint is not available");
}
