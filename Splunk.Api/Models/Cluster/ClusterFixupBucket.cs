using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Cluster;

/// <summary>A bucket on a fixup level (<c>cluster/manager/fixup</c>).</summary>
public sealed class ClusterFixupBucket : SplunkContent
{
	/// <summary>The index of the bucket.</summary>
	[JsonPropertyName("index")]
	public string? Index { get; init; }

	/// <summary>The fixup level.</summary>
	[JsonPropertyName("level")]
	public ClusterFixupLevel Level { get; init; }

	/// <summary>When and why the bucket entered the level.</summary>
	[JsonPropertyName("initial")]
	public ClusterFixupRecord? Initial { get; init; }

	/// <summary>When and why the bucket was last checked.</summary>
	[JsonPropertyName("latest")]
	public ClusterFixupRecord? Latest { get; init; }
}
