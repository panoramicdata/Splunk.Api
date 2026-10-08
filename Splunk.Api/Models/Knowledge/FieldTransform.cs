using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Knowledge;

/// <summary>A field transformation, a <c>transforms.conf</c> stanza used by <c>REPORT-</c> extractions (<c>data/transforms/extractions</c>).</summary>
public sealed class FieldTransform : TransformsStanza
{
	/// <summary>For delimiter-based extractions, the pair and key/value delimiters (<c>DELIMS</c>).</summary>
	[JsonPropertyName("DELIMS")]
	public string? Delimiters { get; init; }

	/// <summary>For delimiter-based extractions, the field names in order (<c>FIELDS</c>).</summary>
	[JsonPropertyName("FIELDS")]
	public string? Fields { get; init; }
}
