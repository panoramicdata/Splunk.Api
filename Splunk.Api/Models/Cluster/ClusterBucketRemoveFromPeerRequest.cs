using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Cluster;

/// <summary>The peer to delete a bucket copy from (<c>POST cluster/manager/buckets/{bucket_id}/remove_from_peer</c>).</summary>
public sealed class ClusterBucketRemoveFromPeerRequest : SplunkFormRequest
{
	/// <summary>The GUID of the peer.</summary>
	[JsonPropertyName("peer")]
	public required string Peer { get; init; }
}
