using Splunk.Api.Models.Cluster;
using Splunk.Api.Test.Support;
using System.Net;

namespace Splunk.Api.Test.Groups;

public class ClusterPeerBucketsTests
{
	private const string Path = "/services/cluster/peer/buckets";
	private const string BucketId = "_internal~58~11111111-1111-1111-1111-111111111111";

	// From the reference's example.
	private const string BucketContent = """
		{
			"checksum": "",
			"eai:acl": null,
			"earliest_time": "1346859162",
			"generation_id": "4",
			"generations": { "0": "0xffffffffffffffff" },
			"latest_time": "1346859257",
			"search_state": "Searchable",
			"status": "StreamingSource"
		}
		""";

	[Fact]
	public async Task ListAsync_SendsGetWithTheGeneration()
		=> (await RequestProbe.SendAsync((c, ct) => c.ClusterPeerBuckets.ListAsync(new ClusterPeerBucketListOptions { GenerationId = "4" }, ct)))
			.ShouldBeProbed(HttpMethod.Get, Path, "?generation_id=4&output_mode=json");

	[Fact]
	public async Task GetAsync_SendsGetWithTheGeneration()
		=> (await RequestProbe.SendAsync((c, ct) => c.ClusterPeerBuckets.GetAsync(BucketId, "4", ct)))
			.ShouldBeProbed(HttpMethod.Get, $"{Path}/{BucketId}", "?generation_id=4&output_mode=json");

	[Fact]
	public async Task DeleteAsync_SendsDeleteWithTheBucketIdField()
		=> (await RequestProbe.SendAsync((c, ct) => c.ClusterPeerBuckets.DeleteAsync(BucketId, new ClusterPeerBucketRemoveRequest { BucketId = BucketId }, ct)))
			.ShouldBeProbed(HttpMethod.Delete, $"{Path}/{BucketId}", body: $"bucket_id={BucketId}");

	[Fact]
	public async Task GetAsync_MapsEveryModelledField()
	{
		var bucket = await RequestProbe.ReadContentAsync((c, ct) => c.ClusterPeerBuckets.GetAsync(BucketId, null, ct), BucketId, BucketContent);

		bucket.Checksum.Should().BeEmpty();
		bucket.EarliestTime.Should().Be(DateTimeOffset.FromUnixTimeSeconds(1346859162));
		bucket.LatestTime.Should().Be(DateTimeOffset.FromUnixTimeSeconds(1346859257));
		bucket.GenerationId.Should().Be(4);
		bucket.Generations["0"].Should().Be("0xffffffffffffffff");
		bucket.SearchState.Should().Be("Searchable");
		bucket.Status.Should().Be("StreamingSource");
	}

	[Fact]
	public Task ListAsync_NotAPeer_RaisesServiceUnavailable()
		=> RequestProbe.FailsAsync(
			(c, ct) => c.ClusterPeerBuckets.ListAsync(null, ct),
			HttpStatusCode.ServiceUnavailable,
			ClusterErrors.PeerNotEnabled,
			"Cluster peer is not enabled on this node, check clustering stanza in server.conf");
}
