using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Topology;

/// <summary>The header of a Topology REST API response.</summary>
public sealed class TopologyHeader
{
	/// <summary>When the response was generated (UTC).</summary>
	[JsonPropertyName("timestamp")]
	public DateTimeOffset? Timestamp { get; init; }
}
