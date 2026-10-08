using Splunk.Api.Models.Cluster;
using Splunk.Api.Test.Support;
using System.Net;

namespace Splunk.Api.Test.Groups;

public class ShClusterConfigTests
{
	private const string Path = "/services/shcluster/config";

	// Captured from Splunk Enterprise 10.6.0.5 (a standalone, so mode disabled), with the ID and label values set.
	private const string ConfigContent = """
		{
			"adhoc_searchhead": false,
			"async_replicate_on_proxy": false,
			"conf_deploy_fetch_url": "https://deployer:8089",
			"cxn_timeout": 60,
			"decommission_search_jobs_wait_secs": 180,
			"disabled": true,
			"dynamic_captain": false,
			"eai:acl": null,
			"heartbeat_period": 18446744073709552000,
			"heartbeat_timeout": 60,
			"id": "BB3116C0-73B9-459A-B473-254A18A69776",
			"manual_detention": "off",
			"max_peer_rep_load": 5,
			"mode": "disabled",
			"percent_peers_to_restart": 10,
			"ping_flag": true,
			"preferred_captain": true,
			"quiet_period": 60,
			"rcv_timeout": 60,
			"register_replication_address": "",
			"rep_cxn_timeout": 5,
			"rep_max_rcv_timeout": 600,
			"rep_max_send_timeout": 600,
			"rep_rcv_timeout": 10,
			"rep_send_timeout": 5,
			"replication_factor": 3,
			"replication_port": null,
			"replication_use_ssl": false,
			"restart_timeout": 60,
			"rolling_restart": "restart",
			"secret": "",
			"send_timeout": 60,
			"shcluster_label": "shc1"
		}
		""";

	[Fact]
	public async Task ListAsync_SendsGet()
		=> (await RequestProbe.SendAsync((c, ct) => c.ShClusterConfig.ListAsync(null, ct)))
			.ShouldBeProbed(HttpMethod.Get, Path);

	[Fact]
	public async Task UpdateAsync_SendsPostWithTheSettings()
		=> (await RequestProbe.SendAsync((c, ct) => c.ShClusterConfig.UpdateAsync(
			new ShClusterConfigUpdateRequest
			{
				RollingRestart = ShClusterRollingRestartMode.Searchable,
				DecommissionSearchJobsWaitSeconds = 120,
				ManualDetention = ManualDetentionMode.On,
				TargetUri = "https://sh2:8089"
			},
			ct)))
			.ShouldBeProbed(HttpMethod.Post, $"{Path}/config", body:
				"rolling_restart=searchable&decommission_search_jobs_wait_secs=120&manual_detention=on&target_uri=https%3A%2F%2Fsh2%3A8089");

	[Fact]
	public async Task ListAsync_MapsEveryModelledField()
	{
		var config = await RequestProbe.ReadContentAsync((c, ct) => c.ShClusterConfig.ListAsync(null, ct), "config", ConfigContent);

		config.Mode.Should().Be("disabled");
		config.Disabled.Should().BeTrue();
		config.ClusterId.Should().Be("BB3116C0-73B9-459A-B473-254A18A69776");
		config.ShClusterLabel.Should().Be("shc1");
		config.ConfDeployFetchUrl.Should().Be("https://deployer:8089");
		config.DynamicCaptain.Should().BeFalse();
		config.PreferredCaptain.Should().BeTrue();
		config.AdhocSearchHead.Should().BeFalse();
		config.ManualDetention.Should().Be(ManualDetentionMode.Off);
		config.RollingRestart.Should().Be("restart");
		config.DecommissionSearchJobsWaitSeconds.Should().Be(180);
		config.PercentPeersToRestart.Should().Be(10);
		config.ReplicationFactor.Should().Be(3);
		config.ReplicationPort.Should().BeNull();
		config.ReplicationUseSsl.Should().BeFalse();
		config.RegisterReplicationAddress.Should().BeEmpty();
		config.MaxPeerReplicationLoad.Should().Be(5);
		config.HeartbeatTimeout.Should().Be(60);
		config.QuietPeriod.Should().Be(60);
		config.RestartTimeout.Should().Be(60);
		config.PingFlag.Should().BeTrue();
		config.Secret.Should().BeEmpty();
		config.ConnectionTimeout.Should().Be(60);
		config.ReplicationConnectionTimeout.Should().Be(5);
		config.SendTimeout.Should().Be(60);
		config.ReplicationSendTimeout.Should().Be(5);
		config.ReplicationMaxSendTimeout.Should().Be(600);
		config.ReceiveTimeout.Should().Be(60);
		config.ReplicationReceiveTimeout.Should().Be(10);
		config.ReplicationMaxReceiveTimeout.Should().Be(600);
	}

	[Fact]
	public Task UpdateAsync_NoShc_RaisesServiceUnavailable()
		=> RequestProbe.FailsAsync(
			(c, ct) => c.ShClusterConfig.UpdateAsync(new ShClusterConfigUpdateRequest { RollingRestart = ShClusterRollingRestartMode.Restart }, ct),
			HttpStatusCode.ServiceUnavailable,
			ClusterErrors.ShcNotEnabled,
			"Search Head Clustering is not enabled on this node. REST endpoint is not available");
}
