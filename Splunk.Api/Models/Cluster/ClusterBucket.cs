using Splunk.Api.Serialization;
using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Cluster;

/// <summary>A clustered bucket, as the cluster manager tracks it (<c>cluster/manager/buckets</c>).</summary>
public sealed class ClusterBucket : SplunkContent
{
	/// <summary>The index the bucket belongs to.</summary>
	[JsonPropertyName("index")]
	public string? Index { get; init; }

	/// <summary>The bucket size, in bytes.</summary>
	[JsonPropertyName("bucket_size")]
	public long? BucketSize { get; init; }

	/// <summary>Whether the bucket is frozen.</summary>
	[JsonPropertyName("frozen")]
	public bool? Frozen { get; init; }

	/// <summary>Whether the bucket was created before its peer joined the cluster.</summary>
	[JsonPropertyName("standalone")]
	public bool? Standalone { get; init; }

	/// <summary>The site the bucket was created on.</summary>
	[JsonPropertyName("origin_site")]
	public string? OriginSite { get; init; }

	/// <summary>Whether the bucket is a pre-multisite bucket replicated only within its origin site.</summary>
	[JsonPropertyName("constrain_to_origin_site")]
	public bool? ConstrainToOriginSite { get; init; }

	/// <summary>The copies of the bucket, by peer GUID.</summary>
	[JsonPropertyName("peers")]
	public IReadOnlyDictionary<string, ClusterBucketCopy> Peers { get; init; } = new Dictionary<string, ClusterBucketCopy>();

	/// <summary>The GUID of the peer holding the primary copy, by site.</summary>
	[JsonPropertyName("primaries_by_site")]
	public IReadOnlyDictionary<string, string> PrimariesBySite { get; init; } = new Dictionary<string, string>();

	/// <summary>The number of copies, by site.</summary>
	[JsonPropertyName("rep_count_by_site")]
	public IReadOnlyDictionary<string, int> ReplicationCountBySite { get; init; } = new Dictionary<string, int>();

	/// <summary>The number of searchable copies, by site.</summary>
	[JsonPropertyName("search_count_by_site")]
	public IReadOnlyDictionary<string, int> SearchCountBySite { get; init; } = new Dictionary<string, int>();

	/// <summary>When deferred servicing of the bucket may start.</summary>
	[JsonPropertyName("service_after_time")]
	[JsonConverter(typeof(EpochSecondsConverter))]
	public DateTimeOffset? ServiceAfterTime { get; init; }
}
