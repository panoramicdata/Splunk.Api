using Refit;

namespace Splunk.Api.Models.Cluster;

/// <summary>The parameters of <c>GET cluster/peer/buckets</c>.</summary>
public sealed class ClusterPeerBucketListOptions : ListOptions
{
	/// <summary>The generation to describe the buckets in.</summary>
	[AliasAs("generation_id")]
	public string? GenerationId { get; init; }
}
