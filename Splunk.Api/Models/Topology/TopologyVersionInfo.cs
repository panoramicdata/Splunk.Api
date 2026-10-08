using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Topology;

/// <summary>The Splunk build and version of a node.</summary>
public sealed class TopologyVersionInfo
{
	/// <summary>The build identifier.</summary>
	[JsonPropertyName("build")]
	public string? Build { get; init; }

	/// <summary>The Splunk version, for example <c>10.6.0.5</c>.</summary>
	[JsonPropertyName("version")]
	public string? Version { get; init; }
}
