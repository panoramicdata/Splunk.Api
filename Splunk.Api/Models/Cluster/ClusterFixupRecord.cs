using Splunk.Api.Serialization;
using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Cluster;

/// <summary>Why and when a bucket entered, or was last checked on, a fixup level.</summary>
public sealed class ClusterFixupRecord
{
	/// <summary>The reason.</summary>
	[JsonPropertyName("reason")]
	public string? Reason { get; init; }

	/// <summary>When.</summary>
	[JsonPropertyName("timestamp")]
	[JsonConverter(typeof(EpochSecondsConverter))]
	public DateTimeOffset? Timestamp { get; init; }
}
