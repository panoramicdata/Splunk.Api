using Splunk.Api.Models.Cluster;
using Splunk.Api.Test.Support;
using System.Net;

namespace Splunk.Api.Test.Groups;

public class ShClusterStatusTests
{
	private const string Path = "/services/shcluster/status";

	// From the reference's example, with the advanced fields from its field list.
	private const string StatusContent = """
		{
			"captain": {
				"decommission_search_jobs_wait_secs": "180",
				"dynamic_captain": "1",
				"elected_captain": "Thu Mar 29 11:58:04 2018",
				"id": "93E0DBE8-A435-462F-BF7D-6297C9D9F939",
				"initialized_flag": "1",
				"label": "sh1",
				"max_failures_to_keep_majority": "1",
				"mgmt_uri": "https://10.0.0.58:8089",
				"min_peers_joined_flag": "1",
				"rolling_restart": "restart",
				"rolling_restart_flag": "0",
				"rolling_upgrade_flag": "0",
				"service_ready_flag": "1",
				"stable_captain": "1",
				"kvstore_status": "ready"
			},
			"eai:acl": null,
			"peers": {
				"2EF65F8B-2689-4A77-B056-E824B2FEB0CA": {
					"label": "sh2",
					"last_conf_replication": "Thu Mar 29 12:00:49 2018",
					"manual_detention": "on",
					"mgmt_uri": "https://10.0.0.57:8089",
					"mgmt_uri_alias": "https://10.0.0.57:8089",
					"out_of_sync_node": "0",
					"preferred_captain": "1",
					"restart_required": "0",
					"splunk_version": "10.6.0",
					"status": "Up",
					"site": "default"
				}
			}
		}
		""";

	[Fact]
	public async Task GetAsync_SendsGetWithAdvanced()
		=> (await RequestProbe.SendAsync((c, ct) => c.ShClusterStatus.GetAsync(true, ct)))
			.ShouldBeProbed(HttpMethod.Get, Path, "?advanced=true&output_mode=json");

	[Fact]
	public async Task GetAsync_MapsEveryModelledField()
	{
		var status = await RequestProbe.ReadContentAsync((c, ct) => c.ShClusterStatus.GetAsync(null, ct), "status", StatusContent);

		var captain = status.Captain!;
		captain.DecommissionSearchJobsWaitSeconds.Should().Be(180);
		captain.DynamicCaptain.Should().BeTrue();
		captain.ElectedCaptain.Should().Be("Thu Mar 29 11:58:04 2018");
		captain.ClusterId.Should().Be("93E0DBE8-A435-462F-BF7D-6297C9D9F939");
		captain.InitializedFlag.Should().BeTrue();
		captain.Label.Should().Be("sh1");
		captain.MaxFailuresToKeepMajority.Should().Be(1);
		captain.ManagementUri.Should().Be("https://10.0.0.58:8089");
		captain.MinPeersJoinedFlag.Should().BeTrue();
		captain.RollingRestart.Should().Be("restart");
		captain.RollingRestartFlag.Should().BeFalse();
		captain.RollingUpgradeFlag.Should().BeFalse();
		captain.ServiceReadyFlag.Should().BeTrue();
		captain.StableCaptain.Should().BeTrue();
		captain.AdditionalProperties["kvstore_status"].GetString().Should().Be("ready");
		var member = status.Peers["2EF65F8B-2689-4A77-B056-E824B2FEB0CA"];
		member.Label.Should().Be("sh2");
		member.LastConfReplication.Should().Be("Thu Mar 29 12:00:49 2018");
		member.ManualDetention.Should().Be(ManualDetentionMode.On);
		member.ManagementUri.Should().Be("https://10.0.0.57:8089");
		member.ManagementUriAlias.Should().Be("https://10.0.0.57:8089");
		member.OutOfSyncNode.Should().BeFalse();
		member.PreferredCaptain.Should().BeTrue();
		member.RestartRequired.Should().BeFalse();
		member.SplunkVersion.Should().Be("10.6.0");
		member.Status.Should().Be(ClusterPeerStatus.Up);
		member.AdditionalProperties["site"].GetString().Should().Be("default");
	}

	[Fact]
	public Task GetAsync_NoShc_RaisesServiceUnavailable()
		=> RequestProbe.FailsAsync(
			(c, ct) => c.ShClusterStatus.GetAsync(null, ct),
			HttpStatusCode.ServiceUnavailable,
			ClusterErrors.ShcNotEnabled,
			"Search Head Clustering is not enabled on this node. REST endpoint is not available");
}
