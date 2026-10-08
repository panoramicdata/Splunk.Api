using Splunk.Api.Test.Support;
using System.Net;

namespace Splunk.Api.Test.Groups;

public class TopologyTests
{
	private const string Guid1 = "00000000-0000-0000-0000-000000000001";

	// Captured from Splunk Enterprise 10.6.0.5 (standalone, docker), with a cluster manager, search head and unmanaged
	// actor added from the reference's field list; host names, addresses and GUIDs replaced.
	private const string TopologyJson = """
		{
			"header": { "timestamp": "2026-10-08T13:46:44Z" },
			"license_manager": {
				"guid": "00000000-0000-0000-0000-000000000001",
				"managed_by": { "license_manager": "00000000-0000-0000-0000-000000000001" },
				"label": "splunk01",
				"fips_enabled": false,
				"host_info": { "scheme": "https", "ip": "192.0.2.10", "fqdn": "splunk01.example.com", "mgmt_port": 8089, "web-port": 8000 },
				"roles": ["indexer", "license_manager", "kv_store"],
				"version-info": { "build": "86587d4e3b27", "version": "10.6.0.5" },
				"os-info": { "build": "#1 SMP PREEMPT_DYNAMIC", "name": "Linux", "version": "6.18.33.2" }
			},
			"cluster_manager": {
				"guid": "00000000-0000-0000-0000-000000000002",
				"label": "cm01",
				"roles": ["cluster_manager"],
				"status": "Up",
				"managed_by": { "license_manager": "00000000-0000-0000-0000-000000000001" }
			},
			"indexers": [
				{
					"guid": "00000000-0000-0000-0000-000000000003",
					"label": "idx01",
					"roles": ["indexer", "cluster_peer"],
					"status": "Up",
					"last_heartbeat": "2026-10-08T13:46:40Z",
					"managed_by": { "cluster_manager": "00000000-0000-0000-0000-000000000002", "license_manager": "00000000-0000-0000-0000-000000000001" }
				}
			],
			"search_heads": [
				{
					"guid": "00000000-0000-0000-0000-000000000004",
					"label": "sh01",
					"roles": ["search_head", "shc_member"],
					"managed_by": { "search_head_cluster": "00000000-0000-0000-0000-000000000005", "deployer": "00000000-0000-0000-0000-000000000006" }
				}
			],
			"deployers": [{ "guid": "00000000-0000-0000-0000-000000000006", "label": "deployer01", "roles": ["shc_deployer"] }],
			"unrecognized": [{ "guid": "00000000-0000-0000-0000-000000000007", "label": "mystery" }],
			"unmanaged_actors": [
				{
					"guid": "00000000-0000-0000-0000-000000000008",
					"label": "uf01",
					"host_info": { "ip": "192.0.2.20" },
					"indexer_guids": ["00000000-0000-0000-0000-000000000003"],
					"connection_types": ["tcp/cooked", "http"],
					"last_conn_time": "2026-10-08T13:40:00Z"
				}
			]
		}
		""";

	// Captured from Splunk Enterprise 10.6.0.5; host name replaced.
	private const string NodeIdentityJson = """
		{
			"header": { "timestamp": "2026-10-08T13:49:55Z" },
			"fips_enabled": false,
			"host_info": {
				"fqdn": "splunk01.example.com", "mgmt_scheme": "https", "mgmt_hostname": "127.0.0.1",
				"mgmt_port": 8089, "web_scheme": "http", "web_port": 8000
			},
			"roles": ["indexer", "license_master", "license_manager", "kv_store"],
			"version_info": { "build": "86587d4e3b27", "version": "10.6.0.5" },
			"os_info": { "build": "#1 SMP PREEMPT_DYNAMIC", "name": "Linux", "version": "6.18.33.2" }
		}
		""";

	// Shape captured from Splunk Enterprise 10.6.0.5, populated with the reference's example values.
	private const string TrustedConnectionsJson = """
		{
			"header": { "timestamp": "2026-10-08T13:49:56Z" },
			"guid": "00000000-0000-0000-0000-000000000001",
			"hec": { "enabled": true, "hashed_tokens": ["e3b0c442"], "port": 8088, "senders": ["192.0.2.5", "192.0.2.6"] },
			"s2s": { "receiving_ports": { "9997": ["192.0.2.10", "192.0.2.11"] }, "forwarding_hosts": ["idx.example.com:9997"] },
			"tcp_inputs": { "5114": ["192.0.2.20"] },
			"udp_inputs": { "5115": ["192.0.2.21"] },
			"searchPeers": [{ "name": "idx01.example.com:8089", "guid": "00000000-0000-0000-0000-000000000003" }]
		}
		""";

	[Fact]
	public async Task GetAsync_SendsGetWithoutOutputMode()
		=> (await RequestProbe.SendAsync((c, ct) => c.Topology.GetAsync(ct), TopologyJson))
			.ShouldBeProbed(HttpMethod.Get, "/services/stack-explainer/v1/topology", string.Empty);

	[Fact]
	public async Task GetWithUnmanagedActorsAsync_SendsTheFlagWithoutAValue()
		=> (await RequestProbe.SendAsync((c, ct) => c.Topology.GetWithUnmanagedActorsAsync(ct), TopologyJson))
			.ShouldBeProbed(HttpMethod.Get, "/services/stack-explainer/v1/topology", "?include_unmanaged_actors");

	[Fact]
	public async Task GetNodeIdentityAsync_SendsGet()
		=> (await RequestProbe.SendAsync((c, ct) => c.Topology.GetNodeIdentityAsync(ct), NodeIdentityJson))
			.ShouldBeProbed(HttpMethod.Get, "/services/stack-explainer/v1/node-identity", string.Empty);

	[Fact]
	public async Task GetRemoteNodeIdentityAsync_SendsGetWithTheGuid()
		=> (await RequestProbe.SendAsync((c, ct) => c.Topology.GetRemoteNodeIdentityAsync(Guid1, ct), NodeIdentityJson))
			.ShouldBeProbed(HttpMethod.Get, $"/services/stack-explainer/v1/node-identity/{Guid1}", string.Empty);

	[Fact]
	public async Task GetTrustedConnectionsAsync_SendsGet()
		=> (await RequestProbe.SendAsync((c, ct) => c.Topology.GetTrustedConnectionsAsync(ct), TrustedConnectionsJson))
			.ShouldBeProbed(HttpMethod.Get, "/services/stack-explainer/v1/trusted-connections", string.Empty);

	[Fact]
	public async Task GetRemoteTrustedConnectionsAsync_SendsGetWithTheGuid()
		=> (await RequestProbe.SendAsync((c, ct) => c.Topology.GetRemoteTrustedConnectionsAsync(Guid1, ct), TrustedConnectionsJson))
			.ShouldBeProbed(HttpMethod.Get, $"/services/stack-explainer/v1/trusted-connections/{Guid1}", string.Empty);

	[Fact]
	public async Task GetAsync_MapsEveryModelledField()
	{
		var topology = await RequestProbe.ReadAsync((c, ct) => c.Topology.GetWithUnmanagedActorsAsync(ct), TopologyJson);

		topology.Header!.Timestamp.Should().Be(new DateTimeOffset(2026, 10, 8, 13, 46, 44, TimeSpan.Zero));
		var manager = topology.LicenseManager!;
		manager.NodeGuid.Should().Be(Guid1);
		manager.Label.Should().Be("splunk01");
		manager.FipsEnabled.Should().BeFalse();
		manager.Roles.Should().Equal("indexer", "license_manager", "kv_store");
		manager.HostInfo!.Scheme.Should().Be("https");
		manager.HostInfo.Ip.Should().Be("192.0.2.10");
		manager.HostInfo.Fqdn.Should().Be("splunk01.example.com");
		manager.HostInfo.ManagementPort.Should().Be(8089);
		manager.HostInfo.WebPort.Should().Be(8000);
		manager.VersionInfo!.Build.Should().Be("86587d4e3b27");
		manager.VersionInfo.Version.Should().Be("10.6.0.5");
		manager.OsInfo!.Build.Should().Be("#1 SMP PREEMPT_DYNAMIC");
		manager.OsInfo.Name.Should().Be("Linux");
		manager.OsInfo.Version.Should().Be("6.18.33.2");
		manager.ManagedBy!.LicenseManager.Should().Be(Guid1);
		topology.ClusterManager!.Status.Should().Be("Up");
		var indexer = topology.Indexers.Should().ContainSingle().Subject;
		indexer.LastHeartbeat.Should().Be("2026-10-08T13:46:40Z");
		indexer.ManagedBy!.ClusterManager.Should().Be("00000000-0000-0000-0000-000000000002");
		var searchHead = topology.SearchHeads.Should().ContainSingle().Subject;
		searchHead.ManagedBy!.SearchHeadCluster.Should().Be("00000000-0000-0000-0000-000000000005");
		searchHead.ManagedBy.Deployer.Should().Be("00000000-0000-0000-0000-000000000006");
		topology.Deployers.Should().ContainSingle().Which.Label.Should().Be("deployer01");
		topology.Unrecognized.Should().ContainSingle().Which.Label.Should().Be("mystery");
		var actor = topology.UnmanagedActors.Should().ContainSingle().Subject;
		actor.NodeGuid.Should().Be("00000000-0000-0000-0000-000000000008");
		actor.Label.Should().Be("uf01");
		actor.HostInfo!.Ip.Should().Be("192.0.2.20");
		actor.IndexerGuids.Should().Equal("00000000-0000-0000-0000-000000000003");
		actor.ConnectionTypes.Should().Equal("tcp/cooked", "http");
		actor.LastConnectionTime.Should().Be("2026-10-08T13:40:00Z");
	}

	[Fact]
	public async Task GetNodeIdentityAsync_MapsEveryModelledField()
	{
		var identity = await RequestProbe.ReadAsync((c, ct) => c.Topology.GetNodeIdentityAsync(ct), NodeIdentityJson);

		identity.Header!.Timestamp.Should().Be(new DateTimeOffset(2026, 10, 8, 13, 49, 55, TimeSpan.Zero));
		identity.FipsEnabled.Should().BeFalse();
		identity.HostInfo!.Fqdn.Should().Be("splunk01.example.com");
		identity.HostInfo.ManagementScheme.Should().Be("https");
		identity.HostInfo.ManagementHostname.Should().Be("127.0.0.1");
		identity.HostInfo.ManagementPort.Should().Be(8089);
		identity.HostInfo.WebScheme.Should().Be("http");
		identity.HostInfo.WebPort.Should().Be(8000);
		identity.Roles.Should().Equal("indexer", "license_master", "license_manager", "kv_store");
		identity.VersionInfo!.Version.Should().Be("10.6.0.5");
		identity.OsInfo!.Name.Should().Be("Linux");
	}

	[Fact]
	public async Task GetTrustedConnectionsAsync_MapsEveryModelledField()
	{
		var connections = await RequestProbe.ReadAsync((c, ct) => c.Topology.GetTrustedConnectionsAsync(ct), TrustedConnectionsJson);

		connections.Header!.Timestamp.Should().NotBeNull();
		connections.NodeGuid.Should().Be(Guid1);
		connections.Hec!.Enabled.Should().BeTrue();
		connections.Hec.Port.Should().Be(8088);
		connections.Hec.HashedTokens.Should().Equal("e3b0c442");
		connections.Hec.Senders.Should().Equal("192.0.2.5", "192.0.2.6");
		connections.S2s!.ReceivingPorts["9997"].Should().Equal("192.0.2.10", "192.0.2.11");
		connections.S2s.ForwardingHosts.Should().Equal("idx.example.com:9997");
		connections.TcpInputs["5114"].Should().Equal("192.0.2.20");
		connections.UdpInputs["5115"].Should().Equal("192.0.2.21");
		var peer = connections.SearchPeers.Should().ContainSingle().Subject;
		peer.Name.Should().Be("idx01.example.com:8089");
		peer.PeerGuid.Should().Be("00000000-0000-0000-0000-000000000003");
	}

	[Fact]
	public Task GetRemoteNodeIdentityAsync_UnknownGuid_RaisesTheSidecarError()
		=> RequestProbe.FailsAsync(
			(c, ct) => c.Topology.GetRemoteNodeIdentityAsync("x", ct),
			HttpStatusCode.NotFound,
			"""{"error":"failed to get node identity: node with guid: x not found"}""",
			"failed to get node identity: node with guid: x not found");
}
