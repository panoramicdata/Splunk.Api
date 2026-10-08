using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Cluster;

/// <summary>The configuration bundle a bundle action applied, validated or rolled back to.</summary>
public sealed class ClusterBundleResult : SplunkContent
{
	/// <summary>The bundle checksum.</summary>
	[JsonPropertyName("checksum")]
	public string? Checksum { get; init; }
}
