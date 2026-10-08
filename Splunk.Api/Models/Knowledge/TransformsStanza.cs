using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Knowledge;

/// <summary>
/// The <c>transforms.conf</c> settings Splunk reports for every transform, whether a field transformation
/// (<c>data/transforms/extractions</c>) or a lookup definition (<c>data/transforms/lookups</c>).
/// </summary>
public abstract class TransformsStanza : SplunkContent
{
	/// <summary>The regular expression applied to <see cref="SourceKey"/> (<c>REGEX</c>).</summary>
	[JsonPropertyName("REGEX")]
	public string? Regex { get; init; }

	/// <summary>The key the regular expression is applied to, by default <c>_raw</c> (<c>SOURCE_KEY</c>).</summary>
	[JsonPropertyName("SOURCE_KEY")]
	public string? SourceKey { get; init; }

	/// <summary>How the extracted values are written, for example <c>$1::$2</c> (<c>FORMAT</c>).</summary>
	[JsonPropertyName("FORMAT")]
	public string? Format { get; init; }

	/// <summary>For index-time transforms, where the result is written (<c>DEST_KEY</c>).</summary>
	[JsonPropertyName("DEST_KEY")]
	public string? DestinationKey { get; init; }

	/// <summary>For index-time transforms, the value written to <see cref="DestinationKey"/> when the expression does not match (<c>DEFAULT_VALUE</c>).</summary>
	[JsonPropertyName("DEFAULT_VALUE")]
	public string? DefaultValue { get; init; }

	/// <summary>Whether Splunk may skip the extraction when no search needs its fields (<c>CAN_OPTIMIZE</c>).</summary>
	[JsonPropertyName("CAN_OPTIMIZE")]
	public bool? CanOptimize { get; init; }

	/// <summary>Whether extracted field names are cleaned to letters, digits and underscores (<c>CLEAN_KEYS</c>).</summary>
	[JsonPropertyName("CLEAN_KEYS")]
	public bool? CleanKeys { get; init; }

	/// <summary>Whether fields with empty values are kept (<c>KEEP_EMPTY_VALS</c>).</summary>
	[JsonPropertyName("KEEP_EMPTY_VALS")]
	public bool? KeepEmptyValues { get; init; }

	/// <summary>Whether a value extracted for a field that already exists is appended, making it multivalued (<c>MV_ADD</c>).</summary>
	[JsonPropertyName("MV_ADD")]
	public bool? MultivalueAdd { get; init; }

	/// <summary>For index-time transforms, whether the result is written to the event's metadata (<c>WRITE_META</c>).</summary>
	[JsonPropertyName("WRITE_META")]
	public bool? WriteMeta { get; init; }

	/// <summary>How many characters into an event the expression looks (<c>LOOKAHEAD</c>, default 4096).</summary>
	[JsonPropertyName("LOOKAHEAD")]
	public int? Lookahead { get; init; }

	/// <summary>The PCRE recursion depth limit (<c>DEPTH_LIMIT</c>).</summary>
	[JsonPropertyName("DEPTH_LIMIT")]
	public int? DepthLimit { get; init; }

	/// <summary>The PCRE match limit (<c>MATCH_LIMIT</c>).</summary>
	[JsonPropertyName("MATCH_LIMIT")]
	public int? MatchLimit { get; init; }
}
