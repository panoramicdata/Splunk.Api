using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Knowledge;

/// <summary>The settings of a field transformation, shared by <see cref="FieldTransformCreateRequest"/> and <see cref="FieldTransformUpdateRequest"/>.</summary>
public abstract class FieldTransformSettings : SplunkFormRequest
{
	/// <summary>
	/// The regular expression (<c>REGEX</c>). Named groups become fields; <c>_KEY_&lt;n&gt;</c>/<c>_VAL_&lt;n&gt;</c> group
	/// pairs extract names and values. Required unless <see cref="Delimiters"/> is set.
	/// </summary>
	[JsonPropertyName("REGEX")]
	public string? Regex { get; init; }

	/// <summary>The key the expression is applied to (<c>SOURCE_KEY</c>); Splunk uses <c>_raw</c> when it is left out.</summary>
	[JsonPropertyName("SOURCE_KEY")]
	public string? SourceKey { get; init; }

	/// <summary>How extracted values are written, for example <c>$1::$2</c> or <c>first::$1 second::$2</c> (<c>FORMAT</c>).</summary>
	[JsonPropertyName("FORMAT")]
	public string? Format { get; init; }

	/// <summary>For delimiter-based extractions, the pair and key/value delimiters, for example <c>"|", "="</c> (<c>DELIMS</c>).</summary>
	[JsonPropertyName("DELIMS")]
	public string? Delimiters { get; init; }

	/// <summary>For delimiter-based extractions, the field names in order (<c>FIELDS</c>).</summary>
	[JsonPropertyName("FIELDS")]
	public string? Fields { get; init; }

	/// <summary>Whether Splunk may skip the extraction when no search needs its fields (<c>CAN_OPTIMIZE</c>, default true).</summary>
	[JsonPropertyName("CAN_OPTIMIZE")]
	public bool? CanOptimize { get; init; }

	/// <summary>Whether extracted field names are cleaned to letters, digits and underscores (<c>CLEAN_KEYS</c>, default true).</summary>
	[JsonPropertyName("CLEAN_KEYS")]
	public bool? CleanKeys { get; init; }

	/// <summary>Whether fields with empty values are kept (<c>KEEP_EMPTY_VALS</c>, default false).</summary>
	[JsonPropertyName("KEEP_EMPTY_VALS")]
	public bool? KeepEmptyValues { get; init; }

	/// <summary>Whether a value extracted for a field that already exists is appended, making it multivalued (<c>MV_ADD</c>, default false).</summary>
	[JsonPropertyName("MV_ADD")]
	public bool? MultivalueAdd { get; init; }

	/// <summary>Whether the transformation is disabled.</summary>
	[JsonPropertyName("disabled")]
	public bool? Disabled { get; init; }
}
