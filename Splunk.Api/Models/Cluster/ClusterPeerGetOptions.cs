using Refit;

namespace Splunk.Api.Models.Cluster;

/// <summary>The optional parameters of <c>GET cluster/manager/peers/{name}</c>.</summary>
public sealed class ClusterPeerGetOptions
{
	/// <summary>Whether to list the peer's buckets.</summary>
	[AliasAs("list_buckets")]
	public bool? ListBuckets { get; init; }
}
