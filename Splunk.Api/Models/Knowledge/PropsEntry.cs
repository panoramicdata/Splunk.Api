using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Knowledge;

/// <summary>
/// The properties shared by the <c>data/props/*</c> endpoints, each of which exposes one kind of <c>props.conf</c>
/// attribute (<c>EVAL-</c>, <c>EXTRACT-</c>/<c>REPORT-</c>, <c>FIELDALIAS-</c>, <c>LOOKUP-</c> or <c>rename</c>) in a stanza.
/// </summary>
/// <remarks>
/// The entry name is <c>{stanza} : {attribute}</c>, for example <c>access_combined : EVAL-response_time</c>; pass it
/// unchanged to the get, update and delete operations.
/// </remarks>
public abstract class PropsEntry : SplunkContent
{
	/// <summary>The full attribute name in <c>props.conf</c>, for example <c>EVAL-response_time</c>.</summary>
	[JsonPropertyName("attribute")]
	public string? Attribute { get; init; }

	/// <summary>
	/// The <c>props.conf</c> stanza the attribute belongs to: a sourcetype, <c>host::&lt;host&gt;</c> or
	/// <c>source::&lt;source&gt;</c>.
	/// </summary>
	[JsonPropertyName("stanza")]
	public string? Stanza { get; init; }

	/// <summary>
	/// The attribute type as Splunk reports it, for example <c>EVAL</c>, <c>Inline</c>, <c>Uses transform</c>,
	/// <c>FIELDALIAS</c>, <c>LOOKUP</c> or <c>rename</c>.
	/// </summary>
	[JsonPropertyName("type")]
	public string? Type { get; init; }

	/// <summary>The attribute's value as written in <c>props.conf</c>.</summary>
	[JsonPropertyName("value")]
	public string? Value { get; init; }
}
