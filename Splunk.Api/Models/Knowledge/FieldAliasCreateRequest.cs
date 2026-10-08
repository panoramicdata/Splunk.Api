using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Knowledge;

/// <summary>Creates a field alias (<c>POST data/props/fieldaliases</c>).</summary>
public sealed class FieldAliasCreateRequest : SplunkFormRequest
{
	private const string AliasPrefix = "alias.";

	/// <summary>The alias group name, without the <c>FIELDALIAS-</c> prefix Splunk adds.</summary>
	[JsonPropertyName("name")]
	public required string Name { get; init; }

	/// <summary>The <c>props.conf</c> stanza: a sourcetype, <c>host::&lt;host&gt;</c> or <c>source::&lt;source&gt;</c>.</summary>
	[JsonPropertyName("stanza")]
	public required string Stanza { get; init; }

	/// <summary>Whether an alias overwrites a field that already has a value (otherwise Splunk writes <c>ASNEW</c>).</summary>
	[JsonPropertyName("overwrite")]
	public bool? Overwrite { get; init; }

	/// <summary>
	/// The aliases, keyed by the original field name, with the alias as the value; each is sent as
	/// <c>alias.{field}={alias}</c>. They are stored in <see cref="SplunkFormRequest.AdditionalParameters"/>, so set
	/// <see cref="SplunkFormRequest.AdditionalParameters"/> (if at all) before this property.
	/// </summary>
	[JsonIgnore]
	public required IReadOnlyDictionary<string, string> Aliases
	{
		get => PrefixedParameters.Get(AdditionalParameters, AliasPrefix);
		init => PrefixedParameters.Set(AdditionalParameters, AliasPrefix, value);
	}
}
