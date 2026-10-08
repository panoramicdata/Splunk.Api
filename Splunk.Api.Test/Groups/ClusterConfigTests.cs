using Splunk.Api.Models;
using Splunk.Api.Models.Cluster;
using Splunk.Api.Test.Support;
using System.Net;

namespace Splunk.Api.Test.Groups;

public class ClusterConfigTests
{
	private const string Path = "/services/cluster/config";

	// Captured from Splunk Enterprise 10.6.0.5 (a standalone, so mode disabled), trimmed; GUID replaced and the
	// manager-only multisite values set to show their shapes.
	private const string ConfigContent = """
		{
			"access_logging_for_heartbeats": true,
			"available_sites": "site1,site2",
			"cluster_label": "idxc1",
			"cm_com_timeout": 18446744073709552000,
			"cxn_timeout": 60,
			"disabled": true,
			"eai:acl": null,
			"forwarderdata_rcv_port": 0,
			"forwarderdata_use_ssl": false,
			"guid": "00000000-0000-0000-0000-000000000001",
			"heartbeat_period": 18446744073709552000,
			"heartbeat_timeout": 60,
			"manager_switchover_mode": "disabled",
			"manager_uri": "?",
			"max_peer_build_load": 5,
			"max_peer_rep_load": 5,
			"mode": "disabled",
			"multisite": "false",
			"notify_scan_period": 10,
			"ping_flag": true,
			"quiet_period": 60,
			"rcv_timeout": 60,
			"register_forwarder_address": "",
			"register_replication_address": "",
			"register_search_address": "",
			"rep_cxn_timeout": 5,
			"rep_max_rcv_timeout": 600,
			"rep_max_send_timeout": 600,
			"rep_rcv_timeout": 10,
			"rep_send_timeout": 5,
			"replication_factor": 3,
			"replication_port": null,
			"replication_use_ssl": false,
			"restart_timeout": 60,
			"search_factor": 2,
			"secret": "",
			"send_timeout": 60,
			"site": "default",
			"site_replication_factor": "origin:2,total:3",
			"site_search_factor": "origin:1,total:2",
			"summary_replication": "false"
		}
		""";

	[Fact]
	public async Task ListAsync_SendsGet()
		=> (await RequestProbe.SendAsync((c, ct) => c.ClusterConfig.ListAsync(new ListOptions { Count = 1 }, ct)))
			.ShouldBeProbed(HttpMethod.Get, Path, "?count=1&output_mode=json");

	[Fact]
	public async Task GetAsync_SendsGet()
		=> (await RequestProbe.SendAsync((c, ct) => c.ClusterConfig.GetAsync(ct)))
			.ShouldBeProbed(HttpMethod.Get, $"{Path}/config");

	[Fact]
	public async Task UpdateAsync_SendsPostWithTheSetFields()
		=> (await RequestProbe.SendAsync((c, ct) => c.ClusterConfig.UpdateAsync(
			new ClusterConfigUpdateRequest
			{
				Mode = ClusterMode.Peer,
				AvailableSites = "site1,site2",
				ClusterLabel = "idxc1",
				ManagerUri = "https://cm:8089",
				Secret = "s3cret",
				Site = "site1",
				Multisite = true,
				ReplicationFactor = 3,
				SearchFactor = 2,
				SiteReplicationFactor = "origin:2,total:3",
				SiteSearchFactor = "origin:1,total:2",
				HeartbeatPeriod = 1,
				HeartbeatTimeout = 60,
				QuietPeriod = 60,
				RestartTimeout = 600,
				MaxPeerBuildLoad = 5,
				MaxPeerReplicationLoad = 5,
				NotifyScanPeriod = 10,
				ReplicationPort = 9887,
				ReplicationUseSsl = false,
				RegisterReplicationAddress = "peer1.example.com",
				RegisterSearchAddress = "peer1-search.example.com",
				SummaryReplication = true,
				UseBatchMaskChanges = true,
				ConnectionTimeout = 61,
				SendTimeout = 62,
				ReceiveTimeout = 63,
				ReplicationConnectionTimeout = 6,
				ReplicationSendTimeout = 7,
				ReplicationReceiveTimeout = 11,
				ReplicationMaxSendTimeout = 601,
				ReplicationMaxReceiveTimeout = 602
			},
			ct)))
			.ShouldBeProbed(HttpMethod.Post, $"{Path}/config", body:
				"mode=peer&available_sites=site1%2Csite2&cluster_label=idxc1&manager_uri=https%3A%2F%2Fcm%3A8089&secret=s3cret&site=site1"
				+ "&multisite=true&replication_factor=3&search_factor=2&site_replication_factor=origin%3A2%2Ctotal%3A3"
				+ "&site_search_factor=origin%3A1%2Ctotal%3A2&heartbeat_period=1&heartbeat_timeout=60&quiet_period=60&restart_timeout=600"
				+ "&max_peer_build_load=5&max_peer_rep_load=5&notify_scan_period=10&replication_port=9887&replication_use_ssl=false"
				+ "&register_replication_address=peer1.example.com&register_search_address=peer1-search.example.com&summary_replication=true&use_batch_mask_changes=true"
				+ "&cxn_timeout=61&send_timeout=62&rcv_timeout=63&rep_cxn_timeout=6&rep_send_timeout=7&rep_rcv_timeout=11"
				+ "&rep_max_send_timeout=601&rep_max_rcv_timeout=602");

	[Fact]
	public async Task GetAsync_MapsEveryModelledField()
	{
		var config = await RequestProbe.ReadContentAsync((c, ct) => c.ClusterConfig.GetAsync(ct), "config", ConfigContent);

		config.Mode.Should().Be(ClusterMode.Disabled);
		config.Disabled.Should().BeTrue();
		config.ClusterLabel.Should().Be("idxc1");
		config.ManagerUri.Should().Be("?");
		config.Secret.Should().BeEmpty();
		config.Site.Should().Be("default");
		config.Multisite.Should().BeFalse();
		config.ReplicationFactor.Should().Be(3);
		config.SearchFactor.Should().Be(2);
		config.HeartbeatTimeout.Should().Be(60);
		config.QuietPeriod.Should().Be(60);
		config.RestartTimeout.Should().Be(60);
		config.MaxPeerBuildLoad.Should().Be(5);
		config.MaxPeerReplicationLoad.Should().Be(5);
		config.ReplicationPort.Should().BeNull();
		config.ReplicationUseSsl.Should().BeFalse();
		config.RegisterReplicationAddress.Should().BeEmpty();
		config.RegisterSearchAddress.Should().BeEmpty();
		config.SummaryReplication.Should().BeFalse();
		config.NotifyScanPeriod.Should().Be(10);
		config.ConnectionTimeout.Should().Be(60);
		config.SendTimeout.Should().Be(60);
		config.ReceiveTimeout.Should().Be(60);
		config.ReplicationConnectionTimeout.Should().Be(5);
		config.ReplicationSendTimeout.Should().Be(5);
		config.ReplicationReceiveTimeout.Should().Be(10);
		config.ReplicationMaxSendTimeout.Should().Be(600);
		config.ReplicationMaxReceiveTimeout.Should().Be(600);
		config.ClusterGuid.Should().Be("00000000-0000-0000-0000-000000000001");
		config.ForwarderDataReceivePort.Should().Be(0);
		config.ForwarderDataUseSsl.Should().BeFalse();
		config.RegisterForwarderAddress.Should().BeEmpty();
		config.PingFlag.Should().BeTrue();
		config.ManagerSwitchoverMode.Should().Be("disabled");
		config.SiteReplicationFactor.Should().Be("origin:2,total:3");
		config.SiteSearchFactor.Should().Be("origin:1,total:2");
		config.AvailableSites.Should().Be("site1,site2");
		config.AdditionalProperties["heartbeat_period"].GetDouble().Should().Be(18446744073709551615d);
	}

	[Theory]
	[InlineData("manager", ClusterMode.Manager)]
	[InlineData("peer", ClusterMode.Peer)]
	[InlineData("searchhead", ClusterMode.SearchHead)]
	[InlineData("master", ClusterMode.Unknown)]
	public async Task GetAsync_MapsEachMode(string wire, ClusterMode mode)
		=> (await RequestProbe.ReadContentAsync((c, ct) => c.ClusterConfig.GetAsync(ct), "config", $$"""{"mode":"{{wire}}"}"""))
			.Mode.Should().Be(mode);

	[Fact]
	public Task UpdateAsync_Error_RaisesSplunkApiException()
		=> RequestProbe.FailsAsync(
			(c, ct) => c.ClusterConfig.UpdateAsync(new ClusterConfigUpdateRequest { Mode = ClusterMode.Manager }, ct),
			HttpStatusCode.BadRequest,
			"""{"messages":[{"type":"ERROR","text":"In handler 'clusterconfig': replication_factor must be greater than 0"}]}""",
			"In handler 'clusterconfig': replication_factor must be greater than 0");
}
