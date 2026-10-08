using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Introspection;

/// <summary>An index volume (<c>data/index-volumes</c>).</summary>
/// <remarks>Splunk refreshes the sizes every 10 minutes, so they are not available straight after startup.</remarks>
public sealed class IndexVolume : SplunkContent
{
	/// <summary>The volume name (<c>name</c>).</summary>
	[JsonPropertyName("name")]
	public string? Name { get; init; }

	/// <summary>The volume path (<c>volume_path</c>).</summary>
	[JsonPropertyName("volume_path")]
	public string? VolumePath { get; init; }

	/// <summary>The maximum size in megabytes, or <c>infinite</c> (<c>max_size</c>).</summary>
	[JsonPropertyName("max_size")]
	public string? MaxSize { get; init; }

	/// <summary>The size in use in megabytes; not reported when <see cref="MaxSize"/> is <c>infinite</c> (<c>total_size</c>).</summary>
	[JsonPropertyName("total_size")]
	public double? TotalSizeMB { get; init; }
}
