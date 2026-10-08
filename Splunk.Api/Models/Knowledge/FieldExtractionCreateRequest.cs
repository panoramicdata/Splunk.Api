using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Knowledge;

/// <summary>Creates a search-time field extraction (<c>POST data/props/extractions</c>).</summary>
public sealed class FieldExtractionCreateRequest : SplunkFormRequest
{
	/// <summary>The extraction name, without the <c>EXTRACT-</c> or <c>REPORT-</c> prefix Splunk adds.</summary>
	[JsonPropertyName("name")]
	public required string Name { get; init; }

	/// <summary>The <c>props.conf</c> stanza: a sourcetype, <c>host::&lt;host&gt;</c> or <c>source::&lt;source&gt;</c>.</summary>
	[JsonPropertyName("stanza")]
	public required string Stanza { get; init; }

	/// <summary>Whether <see cref="Value"/> is an inline regular expression or a list of transforms.</summary>
	[JsonPropertyName("type")]
	public required FieldExtractionType Type { get; init; }

	/// <summary>
	/// For <see cref="FieldExtractionType.Extract"/>, a regular expression with named capture groups; for
	/// <see cref="FieldExtractionType.Report"/>, a comma- or space-separated list of <c>transforms.conf</c> stanza names.
	/// </summary>
	[JsonPropertyName("value")]
	public required string Value { get; init; }
}
