using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Spl2;

/// <summary>The result of <c>POST orchestrator/v1/spl2/convert</c>.</summary>
public sealed class Spl2ConversionResult
{
	/// <summary>The SPL2 search, or <see langword="null"/> when the search could not be converted.</summary>
	[JsonPropertyName("spl2")]
	public string? Spl2 { get; init; }

	/// <summary>Notes on the conversion, as one string.</summary>
	[JsonPropertyName("messages")]
	public string? Messages { get; init; }
}
