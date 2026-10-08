using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Cluster;

/// <summary>The bucket to remove from a peer (<c>DELETE cluster/peer/buckets/{name}</c>).</summary>
public sealed class ClusterPeerBucketRemoveRequest : SplunkFormRequest
{
	/// <summary>The bucket ID; the same as the name in the path.</summary>
	[JsonPropertyName("bucket_id")]
	public required string BucketId { get; init; }
}
