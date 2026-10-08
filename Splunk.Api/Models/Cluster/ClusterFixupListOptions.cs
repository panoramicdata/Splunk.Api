using Refit;

namespace Splunk.Api.Models.Cluster;

/// <summary>The optional parameters of <c>GET cluster/manager/fixup</c>.</summary>
public sealed class ClusterFixupListOptions : ListOptions
{
	/// <summary>Only buckets of this index.</summary>
	[AliasAs("index")]
	public string? Index { get; init; }
}
