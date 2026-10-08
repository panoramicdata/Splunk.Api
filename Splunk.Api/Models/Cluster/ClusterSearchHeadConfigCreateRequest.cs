using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Cluster;

/// <summary>Makes this server a search head of a cluster (<c>POST cluster/searchhead/searchheadconfig</c>).</summary>
public sealed class ClusterSearchHeadConfigCreateRequest : SplunkFormRequest
{
	/// <summary>The cluster manager's URI.</summary>
	[JsonPropertyName("name")]
	public required string Name { get; init; }

	/// <summary>The shared cluster secret (<c>pass4SymmKey</c>).</summary>
	[JsonPropertyName("secret")]
	public required string Secret { get; init; }
}
