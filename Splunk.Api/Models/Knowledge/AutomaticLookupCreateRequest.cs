using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Knowledge;

/// <summary>Creates an automatic lookup (<c>POST data/props/lookups</c>).</summary>
public sealed class AutomaticLookupCreateRequest : AutomaticLookupSettings
{
	/// <summary>The automatic lookup name, without the <c>LOOKUP-</c> prefix Splunk adds.</summary>
	[JsonPropertyName("name")]
	public required string Name { get; init; }

	/// <summary>The <c>props.conf</c> stanza: a sourcetype, <c>host::&lt;host&gt;</c> or <c>source::&lt;source&gt;</c>.</summary>
	[JsonPropertyName("stanza")]
	public required string Stanza { get; init; }
}
