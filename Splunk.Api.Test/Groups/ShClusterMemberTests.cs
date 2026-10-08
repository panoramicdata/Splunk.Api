using Splunk.Api.Models.Cluster;
using Splunk.Api.Test.Support;
using System.Net;

namespace Splunk.Api.Test.Groups;

public class ShClusterMemberTests
{
	private const string Path = "/services/shcluster/member";
	private const string Sid = "scheduler__admin_U0Etbml4__RMD592d31e53ed62579e_at_1413518400_762_88888888-8888-8888-8888-888888888888";

	// From the reference's example.
	private const string InfoContent = """
		{
			"active_historical_search_count": "2",
			"active_realtime_search_count": "1",
			"adhoc_searchhead": "0",
			"eai:acl": null,
			"is_registered": "1",
			"last_heartbeat_attempt": "1522350335",
			"maintenance_mode": "0",
			"no_artifact_replications": "0",
			"peer_load_stats_gla_15m": "15",
			"peer_load_stats_gla_1m": "1",
			"peer_load_stats_gla_5m": "5",
			"peer_load_stats_max_runtime": "120",
			"peer_load_stats_num_autosummary": "3",
			"peer_load_stats_num_historical": "4",
			"peer_load_stats_num_realtime": "6",
			"peer_load_stats_num_running": "7",
			"peer_load_stats_total_runtime": "900",
			"restart_state": "NoRestart",
			"status": "ManualDetention"
		}
		""";

	[Fact]
	public async Task GetInfoAsync_SendsGet()
		=> (await RequestProbe.SendAsync((c, ct) => c.ShClusterMember.GetInfoAsync(ct)))
			.ShouldBeProbed(HttpMethod.Get, $"{Path}/info");

	[Fact]
	public async Task ListArtifactsAsync_SendsGet()
		=> (await RequestProbe.SendAsync((c, ct) => c.ShClusterMember.ListArtifactsAsync(null, ct)))
			.ShouldBeProbed(HttpMethod.Get, $"{Path}/artifacts");

	[Fact]
	public async Task GetArtifactAsync_SendsGetWithTheSid()
		=> (await RequestProbe.SendAsync((c, ct) => c.ShClusterMember.GetArtifactAsync(Sid, ct)))
			.ShouldBeProbed(HttpMethod.Get, $"{Path}/artifacts/{Sid}");

	[Fact]
	public async Task GetConsensusAsync_SendsGet()
		=> (await RequestProbe.SendAsync((c, ct) => c.ShClusterMember.GetConsensusAsync(ct)))
			.ShouldBeProbed(HttpMethod.Get, $"{Path}/consensus");

	[Fact]
	public async Task SetManualDetentionAsync_SendsPostWithTheState()
		=> (await RequestProbe.SendAsync((c, ct) => c.ShClusterMember.SetManualDetentionAsync(new ManualDetentionRequest { ManualDetention = ManualDetentionMode.Off }, ct)))
			.ShouldBeProbed(HttpMethod.Post, $"{Path}/control/control/set_manual_detention", body: "manual_detention=off");

	[Fact]
	public async Task GetInfoAsync_MapsEveryModelledField()
	{
		var info = await RequestProbe.ReadContentAsync((c, ct) => c.ShClusterMember.GetInfoAsync(ct), "member", InfoContent);

		info.ActiveHistoricalSearchCount.Should().Be(2);
		info.ActiveRealtimeSearchCount.Should().Be(1);
		info.AdhocSearchHead.Should().BeFalse();
		info.IsRegistered.Should().BeTrue();
		info.LastHeartbeatAttempt.Should().Be(DateTimeOffset.FromUnixTimeSeconds(1522350335));
		info.MaintenanceMode.Should().BeFalse();
		info.ScheduledSearchesLast15Minutes.Should().Be(15);
		info.ScheduledSearchesLast1Minute.Should().Be(1);
		info.ScheduledSearchesLast5Minutes.Should().Be(5);
		info.MaxRuntime.Should().Be(120);
		info.AutosummarySearchCount.Should().Be(3);
		info.HistoricalSearchCount.Should().Be(4);
		info.RealtimeSearchCount.Should().Be(6);
		info.RunningSearchCount.Should().Be(7);
		info.TotalRuntime.Should().Be(900);
		info.RestartState.Should().Be("NoRestart");
		info.Status.Should().Be(ClusterPeerStatus.ManualDetention);
	}

	[Fact]
	public async Task GetArtifactAsync_MapsTheStatus()
		=> (await RequestProbe.ReadContentAsync((c, ct) => c.ShClusterMember.GetArtifactAsync(Sid, ct), Sid, """{"status":"Complete"}"""))
			.Status.Should().Be("Complete");

	[Fact]
	public async Task GetConsensusAsync_MapsEveryModelledField()
	{
		var consensus = await RequestProbe.ReadContentAsync(
			(c, ct) => c.ShClusterMember.GetConsensusAsync(ct),
			"shc_cluster_configuration",
			"""{"configuration_id":"4","servers_list":"https://sh1:8089,https://sh2:8089"}""");

		consensus.ConfigurationId.Should().Be(4);
		consensus.ServersList.Should().Be("https://sh1:8089,https://sh2:8089");
	}

	[Fact]
	public Task GetConsensusAsync_NoShc_RaisesBadRequest()
		=> RequestProbe.FailsAsync(
			(c, ct) => c.ShClusterMember.GetConsensusAsync(ct),
			HttpStatusCode.BadRequest,
			"""{"messages":[{"type":"ERROR","text":"Search Head Clustering is not enabled on this node. Raft REST endpoints are not available!"}]}""",
			"Search Head Clustering is not enabled on this node. Raft REST endpoints are not available!");
}
