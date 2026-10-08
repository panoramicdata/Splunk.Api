using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Cluster;

/// <summary>The bucket and peer of <c>POST cluster/manager/control/control/resync_bucket_from_peer</c>.</summary>
public sealed class ClusterResyncBucketRequest : SplunkFormRequest
{
	/// <summary>The bucket to reset.</summary>
	[JsonPropertyName("bucket_id")]
	public required string BucketId { get; init; }

	/// <summary>The GUID of the peer whose copy is authoritative.</summary>
	[JsonPropertyName("peer")]
	public required string Peer { get; init; }
}
