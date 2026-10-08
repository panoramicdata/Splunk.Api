using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Topology;

/// <summary>A search peer of a search head.</summary>
public sealed class TrustedSearchPeer
{
	/// <summary>The peer's host name or label.</summary>
	[JsonPropertyName("name")]
	public string? Name { get; init; }

	/// <summary>The peer's GUID.</summary>
	[JsonPropertyName("guid")]
	public string? PeerGuid { get; init; }
}
