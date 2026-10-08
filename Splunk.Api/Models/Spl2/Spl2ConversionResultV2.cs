using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Spl2;

/// <summary>The result of <c>POST orchestrator/v2/spl2/convert</c>.</summary>
public sealed class Spl2ConversionResultV2
{
	/// <summary>The SPL2 search; absent when the conversion failed.</summary>
	[JsonPropertyName("spl2")]
	public string? Spl2 { get; init; }

	/// <summary>A warning or error from the conversion, or <see langword="null"/> when there was none.</summary>
	[JsonPropertyName("message")]
	public Spl2ConversionMessage? Message { get; init; }
}
