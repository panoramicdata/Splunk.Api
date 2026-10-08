using Splunk.Api.Test.Support;
using System.Net;

namespace Splunk.Api.Test.Groups;

public class ClusterManagerIndexesTests
{
	private const string Path = "/services/cluster/manager/indexes";

	// From the reference's example.
	private const string IndexContent = """
		{
			"buckets_with_excess_copies": "1",
			"buckets_with_excess_searchable_copies": "2",
			"eai:acl": null,
			"index_size": "284975",
			"is_searchable": "1",
			"non_site_aware_buckets_in_site_aware_cluster": "6",
			"num_buckets": "12",
			"replicated_copies_tracker": {
				"0": { "actual_copies_per_slot": "12", "expected_total_per_slot": "12" },
				"1": { "actual_copies_per_slot": "11", "expected_total_per_slot": "12" }
			},
			"searchable_copies_tracker": { "0": { "actual_copies_per_slot": "12", "expected_total_per_slot": "12" } },
			"sort_order": "4294967295",
			"total_excess_bucket_copies": "3",
			"total_excess_searchable_copies": "4"
		}
		""";

	[Fact]
	public async Task ListAsync_SendsGet()
		=> (await RequestProbe.SendAsync((c, ct) => c.ClusterManagerIndexes.ListAsync(null, ct)))
			.ShouldBeProbed(HttpMethod.Get, Path);

	[Fact]
	public async Task GetAsync_SendsGetWithTheName()
		=> (await RequestProbe.SendAsync((c, ct) => c.ClusterManagerIndexes.GetAsync("_audit", ct)))
			.ShouldBeProbed(HttpMethod.Get, $"{Path}/_audit");

	[Fact]
	public async Task GetAsync_MapsEveryModelledField()
	{
		var index = await RequestProbe.ReadContentAsync((c, ct) => c.ClusterManagerIndexes.GetAsync("_audit", ct), "_audit", IndexContent);

		index.BucketsWithExcessCopies.Should().Be(1);
		index.BucketsWithExcessSearchableCopies.Should().Be(2);
		index.IndexSize.Should().Be(284975);
		index.IsSearchable.Should().BeTrue();
		index.NonSiteAwareBuckets.Should().Be(6);
		index.NumberOfBuckets.Should().Be(12);
		index.ReplicatedCopiesTracker["1"].ActualCopiesPerSlot.Should().Be(11);
		index.ReplicatedCopiesTracker["1"].ExpectedTotalPerSlot.Should().Be(12);
		index.SearchableCopiesTracker.Should().ContainKey("0");
		index.SortOrder.Should().Be(4294967295);
		index.TotalExcessBucketCopies.Should().Be(3);
		index.TotalExcessSearchableCopies.Should().Be(4);
	}

	[Fact]
	public Task GetAsync_NotAManager_RaisesServiceUnavailable()
		=> RequestProbe.FailsAsync(
			(c, ct) => c.ClusterManagerIndexes.GetAsync("main", ct),
			HttpStatusCode.ServiceUnavailable,
			ClusterErrors.ManagerNotEnabled,
			"Cluster manager is not enabled on this node");
}
