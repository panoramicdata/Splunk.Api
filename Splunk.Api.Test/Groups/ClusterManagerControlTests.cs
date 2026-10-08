using Splunk.Api.Models.Cluster;
using Splunk.Api.Test.Support;
using System.Net;

namespace Splunk.Api.Test.Groups;

public class ClusterManagerControlTests
{
	private const string Path = "/services/cluster/manager/control/control";
	private const string BucketId = "_audit~2~1A3889D7-954B-4CE6-B071-01B438DE9865";
	private const string EscapedBucketId = "_audit~2~1A3889D7-954B-4CE6-B071-01B438DE9865";

	[Fact]
	public async Task PruneIndexAsync_SendsPostWithTheIndex()
		=> (await RequestProbe.SendAsync((c, ct) => c.ClusterManagerControl.PruneIndexAsync(new ClusterPruneIndexRequest { Index = "my_index" }, ct)))
			.ShouldBeProbed(HttpMethod.Post, $"{Path}/prune_index", body: "index=my_index");

	[Fact]
	public async Task RebalancePrimariesAsync_SendsPost()
		=> (await RequestProbe.SendAsync((c, ct) => c.ClusterManagerControl.RebalancePrimariesAsync(ct)))
			.ShouldBeProbed(HttpMethod.Post, $"{Path}/rebalance_primaries");

	[Fact]
	public async Task RemovePeersAsync_SendsPostWithThePeers()
		=> (await RequestProbe.SendAsync((c, ct) => c.ClusterManagerControl.RemovePeersAsync(new ClusterRemovePeersRequest { Peers = "GUID1,GUID2" }, ct)))
			.ShouldBeProbed(HttpMethod.Post, $"{Path}/remove_peers", body: "peers=GUID1%2CGUID2");

	[Fact]
	public async Task ResyncBucketFromPeerAsync_SendsPostWithBucketAndPeer()
		=> (await RequestProbe.SendAsync((c, ct) => c.ClusterManagerControl.ResyncBucketFromPeerAsync(new ClusterResyncBucketRequest { BucketId = BucketId, Peer = "GUID1" }, ct)))
			.ShouldBeProbed(HttpMethod.Post, $"{Path}/resync_bucket_from_peer", body: $"bucket_id={EscapedBucketId}&peer=GUID1");

	[Fact]
	public async Task RollHotBucketAsync_SendsPostWithTheBucket()
		=> (await RequestProbe.SendAsync((c, ct) => c.ClusterManagerControl.RollHotBucketAsync(new ClusterRollHotBucketRequest { BucketId = BucketId }, ct)))
			.ShouldBeProbed(HttpMethod.Post, $"{Path}/roll-hot-buckets", body: $"bucket_id={EscapedBucketId}");

	[Fact]
	public async Task FinalizeRollingUpgradeAsync_SendsPost()
		=> (await RequestProbe.SendAsync((c, ct) => c.ClusterManagerControl.FinalizeRollingUpgradeAsync(ct)))
			.ShouldBeProbed(HttpMethod.Post, $"{Path}/rolling_upgrade_finalize");

	[Fact]
	public async Task InitializeRollingUpgradeAsync_SendsPost()
		=> (await RequestProbe.SendAsync((c, ct) => c.ClusterManagerControl.InitializeRollingUpgradeAsync(ct)))
			.ShouldBeProbed(HttpMethod.Post, $"{Path}/rolling_upgrade_init");

	[Fact]
	public async Task RemovePeersAsync_MapsTheEmptyFeedAndItsMessages()
	{
		var feed = await RequestProbe.ReadAsync(
			(c, ct) => c.ClusterManagerControl.RemovePeersAsync(new ClusterRemovePeersRequest { Peers = "GUID1" }, ct),
			"""{"links":{},"origin":"https://splunk.test:8089/services/cluster/manager/control","entry":[],"paging":{"total":0,"perPage":30,"offset":0},"messages":[{"type":"INFO","text":"Removed 1 peer"}]}""");

		feed.Entries.Should().BeEmpty();
		feed.Messages.Should().ContainSingle().Which.Text.Should().Be("Removed 1 peer");
	}

	[Fact]
	public Task RemovePeersAsync_PeerUp_RaisesTheSplunkError()
		=> RequestProbe.FailsAsync(
			(c, ct) => c.ClusterManagerControl.RemovePeersAsync(new ClusterRemovePeersRequest { Peers = "GUID1" }, ct),
			HttpStatusCode.BadRequest,
			"""<response><messages><msg type="ERROR">In handler 'clustermanagercontrol': Remove aborted, Reason: Peer=idx1 with guid=GUID1 cannot be removed. Peer has status=Up.</msg></messages></response>""",
			"In handler 'clustermanagercontrol': Remove aborted, Reason: Peer=idx1 with guid=GUID1 cannot be removed. Peer has status=Up.");
}
