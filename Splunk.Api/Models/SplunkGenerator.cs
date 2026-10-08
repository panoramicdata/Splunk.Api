using System.Text.Json.Serialization;

namespace Splunk.Api.Models;

/// <summary>The Splunk build and version that produced a response.</summary>
public sealed class SplunkGenerator
{
	/// <summary>The build identifier.</summary>
	[JsonPropertyName("build")]
	public string? Build { get; init; }

	/// <summary>The Splunk version, for example <c>10.6.0</c>.</summary>
	[JsonPropertyName("version")]
	public string? Version { get; init; }
}
