using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Cluster;

/// <summary>A peer in the rolling restart status.</summary>
public sealed class ClusterStatusPeer
{
	/// <summary>The peer's name.</summary>
	[JsonPropertyName("label")]
	public string? Label { get; init; }

	/// <summary>The peer's site.</summary>
	[JsonPropertyName("site")]
	public string? Site { get; init; }

	/// <summary>The peer status.</summary>
	[JsonPropertyName("status")]
	public ClusterPeerStatus Status { get; init; }
}
