using Splunk.Api.Models.Cluster;
using Splunk.Api.Test.Support;
using System.Net;

namespace Splunk.Api.Test.Groups;

public class ClusterManagerPeersTests
{
	private const string Path = "/services/cluster/manager/peers";
	private const string PeerGuid = "29F9560E-A44A-425C-8753-1C6158B46C84";

	// From the reference's example.
	private const string PeerContent = """
		{
			"active_bundle_id": "4708B74780A1E5101449548B1E103616",
			"apply_bundle_status": {
				"invalid_bundle": { "bundle_validation_errors": [], "invalid_bundle_id": "" },
				"reload_error": "",
				"restart_required_for_apply_bundle": "0"
			},
			"base_generation_id": "3",
			"bucket_count": "11",
			"bucket_count_by_index": { "_audit": "6", "_internal": "5" },
			"delayed_buckets_to_discard": [],
			"eai:acl": null,
			"fixup_set": ["_audit~3~29F9560E"],
			"heartbeat_started": "1",
			"host_port_pair": "10.0.1.1:8092",
			"is_searchable": "1",
			"label": "s1p3",
			"last_heartbeat": "1397762298",
			"latest_bundle_id": "4708B74780A1E5101449548B1E103616",
			"pending_job_count": "0",
			"primary_count": "6",
			"primary_count_remote": "2",
			"replication_count": "1",
			"replication_port": "9902",
			"replication_use_ssl": "0",
			"search_state_counter": { "PendingSearchable": "0", "Searchable": "8", "Unsearchable": "3" },
			"site": "site1",
			"splunk_version": "10.6.0",
			"status": "ManualDetention-PortsEnabled",
			"status_counter": { "Complete": "6", "StreamingSource": "2" }
		}
		""";

	[Fact]
	public async Task ListAsync_SendsGet()
		=> (await RequestProbe.SendAsync((c, ct) => c.ClusterManagerPeers.ListAsync(null, ct)))
			.ShouldBeProbed(HttpMethod.Get, Path);

	[Fact]
	public async Task GetAsync_SendsGetWithListBuckets()
		=> (await RequestProbe.SendAsync((c, ct) => c.ClusterManagerPeers.GetAsync(PeerGuid, new ClusterPeerGetOptions { ListBuckets = true }, ct)))
			.ShouldBeProbed(HttpMethod.Get, $"{Path}/{PeerGuid}", "?list_buckets=true&output_mode=json");

	[Fact]
	public async Task GetAsync_MapsEveryModelledField()
	{
		var peer = await RequestProbe.ReadContentAsync((c, ct) => c.ClusterManagerPeers.GetAsync(PeerGuid, null, ct), PeerGuid, PeerContent);

		peer.ActiveBundleId.Should().Be("4708B74780A1E5101449548B1E103616");
		peer.ApplyBundleStatus!.ReloadError.Should().BeEmpty();
		peer.ApplyBundleStatus.RestartRequiredForApplyBundle.Should().BeFalse();
		peer.ApplyBundleStatus.AdditionalProperties.Should().ContainKey("invalid_bundle");
		peer.BaseGenerationId.Should().Be(3);
		peer.BucketCount.Should().Be(11);
		peer.BucketCountByIndex["_audit"].Should().Be(6);
		peer.DelayedBucketsToDiscard.Should().BeEmpty();
		peer.FixupSet.Should().Equal("_audit~3~29F9560E");
		peer.HeartbeatStarted.Should().BeTrue();
		peer.HostPortPair.Should().Be("10.0.1.1:8092");
		peer.IsSearchable.Should().BeTrue();
		peer.Label.Should().Be("s1p3");
		peer.LastHeartbeat.Should().Be(DateTimeOffset.FromUnixTimeSeconds(1397762298));
		peer.LatestBundleId.Should().Be("4708B74780A1E5101449548B1E103616");
		peer.PendingJobCount.Should().Be(0);
		peer.PrimaryCount.Should().Be(6);
		peer.PrimaryCountRemote.Should().Be(2);
		peer.ReplicationCount.Should().Be(1);
		peer.ReplicationPort.Should().Be(9902);
		peer.ReplicationUseSsl.Should().BeFalse();
		peer.SearchStateCounter["Searchable"].Should().Be(8);
		peer.Site.Should().Be("site1");
		peer.SplunkVersion.Should().Be("10.6.0");
		peer.Status.Should().Be(ClusterPeerStatus.ManualDetentionPortsEnabled);
		peer.StatusCounter["StreamingSource"].Should().Be(2);
	}

	[Theory]
	[InlineData("Up", ClusterPeerStatus.Up)]
	[InlineData("Pending", ClusterPeerStatus.Pending)]
	[InlineData("AutomaticDetention", ClusterPeerStatus.AutomaticDetention)]
	[InlineData("ManualDetention", ClusterPeerStatus.ManualDetention)]
	[InlineData("ShuttingDown", ClusterPeerStatus.ShuttingDown)]
	[InlineData("ReassigningPrimaries", ClusterPeerStatus.ReassigningPrimaries)]
	[InlineData("Decommissioning", ClusterPeerStatus.Decommissioning)]
	[InlineData("GracefulShutdown", ClusterPeerStatus.GracefulShutdown)]
	[InlineData("Stopped", ClusterPeerStatus.Stopped)]
	[InlineData("Down", ClusterPeerStatus.Down)]
	[InlineData("BatchAdding", ClusterPeerStatus.BatchAdding)]
	[InlineData("Something new", ClusterPeerStatus.Unknown)]
	public async Task GetAsync_MapsEachStatus(string wire, ClusterPeerStatus status)
		=> (await RequestProbe.ReadContentAsync((c, ct) => c.ClusterManagerPeers.GetAsync(PeerGuid, null, ct), PeerGuid, $$"""{"status":"{{wire}}"}"""))
			.Status.Should().Be(status);

	[Fact]
	public Task ListAsync_NotAManager_RaisesServiceUnavailable()
		=> RequestProbe.FailsAsync(
			(c, ct) => c.ClusterManagerPeers.ListAsync(null, ct),
			HttpStatusCode.ServiceUnavailable,
			ClusterErrors.ManagerNotEnabled,
			"Cluster manager is not enabled on this node");
}
