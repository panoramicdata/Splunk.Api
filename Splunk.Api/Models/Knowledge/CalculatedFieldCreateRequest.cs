using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Knowledge;

/// <summary>Creates a calculated field (<c>POST data/props/calcfields</c>).</summary>
public sealed class CalculatedFieldCreateRequest : SplunkFormRequest
{
	/// <summary>The field to calculate, without the <c>EVAL-</c> prefix Splunk adds.</summary>
	[JsonPropertyName("name")]
	public required string Name { get; init; }

	/// <summary>The <c>props.conf</c> stanza: a sourcetype, <c>host::&lt;host&gt;</c> or <c>source::&lt;source&gt;</c>.</summary>
	[JsonPropertyName("stanza")]
	public required string Stanza { get; init; }

	/// <summary>The eval expression, for example <c>response_time/1000</c>.</summary>
	[JsonPropertyName("value")]
	public required string Value { get; init; }
}
