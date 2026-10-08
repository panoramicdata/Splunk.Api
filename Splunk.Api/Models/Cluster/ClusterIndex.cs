using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Cluster;

/// <summary>A clustered index, as the cluster manager tracks it (<c>cluster/manager/indexes</c>).</summary>
public sealed class ClusterIndex : SplunkContent
{
	/// <summary>Whether every bucket of the index has a primary copy.</summary>
	[JsonPropertyName("is_searchable")]
	public bool? IsSearchable { get; init; }

	/// <summary>The index size, in bytes.</summary>
	[JsonPropertyName("index_size")]
	public long? IndexSize { get; init; }

	/// <summary>The number of distinct buckets.</summary>
	[JsonPropertyName("num_buckets")]
	public int? NumberOfBuckets { get; init; }

	/// <summary>The number of buckets with more copies than needed.</summary>
	[JsonPropertyName("buckets_with_excess_copies")]
	public int? BucketsWithExcessCopies { get; init; }

	/// <summary>The number of buckets with more searchable copies than needed.</summary>
	[JsonPropertyName("buckets_with_excess_searchable_copies")]
	public int? BucketsWithExcessSearchableCopies { get; init; }

	/// <summary>The total number of excess copies.</summary>
	[JsonPropertyName("total_excess_bucket_copies")]
	public int? TotalExcessBucketCopies { get; init; }

	/// <summary>The total number of excess searchable copies.</summary>
	[JsonPropertyName("total_excess_searchable_copies")]
	public int? TotalExcessSearchableCopies { get; init; }

	/// <summary>The number of buckets created before the cluster became multisite (multisite only).</summary>
	[JsonPropertyName("non_site_aware_buckets_in_site_aware_cluster")]
	public int? NonSiteAwareBuckets { get; init; }

	/// <summary>The buckets per number of copies, by copy count.</summary>
	[JsonPropertyName("replicated_copies_tracker")]
	public IReadOnlyDictionary<string, ClusterCopiesTracker> ReplicatedCopiesTracker { get; init; } = new Dictionary<string, ClusterCopiesTracker>();

	/// <summary>The buckets per number of searchable copies, by copy count.</summary>
	[JsonPropertyName("searchable_copies_tracker")]
	public IReadOnlyDictionary<string, ClusterCopiesTracker> SearchableCopiesTracker { get; init; } = new Dictionary<string, ClusterCopiesTracker>();

	/// <summary>Used by Splunk Web.</summary>
	[JsonPropertyName("sort_order")]
	public long? SortOrder { get; init; }
}
