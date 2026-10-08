using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Knowledge;

/// <summary>Changes a field extraction's regular expression or transforms (<c>POST data/props/extractions/{name}</c>).</summary>
public sealed class FieldExtractionUpdateRequest : SplunkFormRequest
{
	/// <summary>The new regular expression (<c>EXTRACT-</c>) or list of transforms (<c>REPORT-</c>).</summary>
	[JsonPropertyName("value")]
	public required string Value { get; init; }
}
