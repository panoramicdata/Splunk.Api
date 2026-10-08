using Splunk.Api.Models.Cluster;
using Splunk.Api.Test.Support;
using System.Net;

namespace Splunk.Api.Test.Groups;

public class ClusterManagerBucketsTests
{
	private const string Path = "/services/cluster/manager/buckets";
	private const string BucketId = "_internal~1~238C3311-F0A4-4A9B-97F0-53667CFFEEAB";

	// From the reference's example (a standalone has no cluster manager).
	private const string BucketContent = """
		{
			"bucket_size": "47187",
			"constrain_to_origin_site": "1",
			"eai:acl": null,
			"frozen": "0",
			"index": "_internal",
			"origin_site": "site2",
			"peers": {
				"238C3311-F0A4-4A9B-97F0-53667CFFEEAB": {
					"bucket_flags": "0x4", "checksum": "", "checksum_state": "StableCksum",
					"search_state": "Searchable", "status": "StreamingSource", "replication_count": "1"
				}
			},
			"primaries_by_site": { "site1": "29F9560E-A44A-425C-8753-1C6158B46C84", "site2": "238C3311-F0A4-4A9B-97F0-53667CFFEEAB" },
			"rep_count_by_site": { "site1": "1", "site2": "2" },
			"search_count_by_site": { "site1": "1", "site2": "1" },
			"service_after_time": "1397762228",
			"standalone": "0"
		}
		""";

	[Fact]
	public async Task ListAsync_SendsGetWithRepeatedFilters()
		=> (await RequestProbe.SendAsync((c, ct) => c.ClusterManagerBuckets.ListAsync(
			new ClusterBucketListOptions { Filters = ["index=main", "status=StreamingSource"], Summaries = true },
			ct)))
			.ShouldBeProbed(HttpMethod.Get, Path, "?filter=index%3Dmain&filter=status%3DStreamingSource&summaries=true&output_mode=json");

	[Fact]
	public async Task GetAsync_SendsGetWithTheBucketId()
		=> (await RequestProbe.SendAsync((c, ct) => c.ClusterManagerBuckets.GetAsync(BucketId, ct)))
			.ShouldBeProbed(HttpMethod.Get, $"{Path}/{BucketId}");

	[Fact]
	public async Task FixAsync_SendsPost()
		=> (await RequestProbe.SendAsync((c, ct) => c.ClusterManagerBuckets.FixAsync(BucketId, ct)))
			.ShouldBeProbed(HttpMethod.Post, $"{Path}/{BucketId}/fix");

	[Fact]
	public async Task FixCorruptAsync_SendsPost()
		=> (await RequestProbe.SendAsync((c, ct) => c.ClusterManagerBuckets.FixCorruptAsync(BucketId, ct)))
			.ShouldBeProbed(HttpMethod.Post, $"{Path}/{BucketId}/fix_corrupt_bucket");

	[Fact]
	public async Task FreezeAsync_SendsPost()
		=> (await RequestProbe.SendAsync((c, ct) => c.ClusterManagerBuckets.FreezeAsync(BucketId, ct)))
			.ShouldBeProbed(HttpMethod.Post, $"{Path}/{BucketId}/freeze");

	[Fact]
	public async Task RemoveAllAsync_SendsPost()
		=> (await RequestProbe.SendAsync((c, ct) => c.ClusterManagerBuckets.RemoveAllAsync(BucketId, ct)))
			.ShouldBeProbed(HttpMethod.Post, $"{Path}/{BucketId}/remove_all");

	[Fact]
	public async Task RemoveFromPeerAsync_SendsPostWithThePeer()
		=> (await RequestProbe.SendAsync((c, ct) => c.ClusterManagerBuckets.RemoveFromPeerAsync(BucketId, new ClusterBucketRemoveFromPeerRequest { Peer = "PEER-GUID" }, ct)))
			.ShouldBeProbed(HttpMethod.Post, $"{Path}/{BucketId}/remove_from_peer", body: "peer=PEER-GUID");

	[Fact]
	public async Task GetAsync_MapsEveryModelledField()
	{
		var bucket = await RequestProbe.ReadContentAsync((c, ct) => c.ClusterManagerBuckets.GetAsync(BucketId, ct), BucketId, BucketContent);

		bucket.Index.Should().Be("_internal");
		bucket.BucketSize.Should().Be(47187);
		bucket.Frozen.Should().BeFalse();
		bucket.Standalone.Should().BeFalse();
		bucket.OriginSite.Should().Be("site2");
		bucket.ConstrainToOriginSite.Should().BeTrue();
		var copy = bucket.Peers["238C3311-F0A4-4A9B-97F0-53667CFFEEAB"];
		copy.BucketFlags.Should().Be("0x4");
		copy.Checksum.Should().BeEmpty();
		copy.ChecksumState.Should().Be("StableCksum");
		copy.SearchState.Should().Be("Searchable");
		copy.Status.Should().Be("StreamingSource");
		copy.AdditionalProperties["replication_count"].GetString().Should().Be("1");
		bucket.PrimariesBySite["site2"].Should().Be("238C3311-F0A4-4A9B-97F0-53667CFFEEAB");
		bucket.ReplicationCountBySite["site2"].Should().Be(2);
		bucket.SearchCountBySite["site1"].Should().Be(1);
		bucket.ServiceAfterTime.Should().Be(DateTimeOffset.FromUnixTimeSeconds(1397762228));
	}

	[Fact]
	public Task FixAsync_NotAManager_RaisesServiceUnavailable()
		=> RequestProbe.FailsAsync(
			(c, ct) => c.ClusterManagerBuckets.FixAsync(BucketId, ct),
			HttpStatusCode.ServiceUnavailable,
			ClusterErrors.ManagerNotEnabled,
			"Cluster manager is not enabled on this node");
}
