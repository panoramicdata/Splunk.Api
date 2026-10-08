using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Topology;

/// <summary>The operating system of a node.</summary>
public sealed class TopologyOsInfo
{
	/// <summary>The operating system build or kernel identifier.</summary>
	[JsonPropertyName("build")]
	public string? Build { get; init; }

	/// <summary>The operating system name, for example <c>Linux</c>.</summary>
	[JsonPropertyName("name")]
	public string? Name { get; init; }

	/// <summary>The operating system version.</summary>
	[JsonPropertyName("version")]
	public string? Version { get; init; }
}
