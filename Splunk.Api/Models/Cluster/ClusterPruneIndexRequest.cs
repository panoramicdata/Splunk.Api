using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Cluster;

/// <summary>Which index to remove excess bucket copies from (<c>POST cluster/manager/control/control/prune_index</c>).</summary>
public sealed class ClusterPruneIndexRequest : SplunkFormRequest
{
	/// <summary>The index; <see langword="null"/> for every index.</summary>
	[JsonPropertyName("index")]
	public string? Index { get; init; }
}
