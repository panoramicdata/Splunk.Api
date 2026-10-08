using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Knowledge;

/// <summary>Creates a field alias (<c>POST data/props/fieldaliases</c>).</summary>
public sealed class FieldAliasCreateRequest : FieldAliasSettings
{
	/// <summary>The alias group name, without the <c>FIELDALIAS-</c> prefix Splunk adds.</summary>
	[JsonPropertyName("name")]
	public required string Name { get; init; }

	/// <summary>The <c>props.conf</c> stanza: a sourcetype, <c>host::&lt;host&gt;</c> or <c>source::&lt;source&gt;</c>.</summary>
	[JsonPropertyName("stanza")]
	public required string Stanza { get; init; }
}
