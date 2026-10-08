using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Knowledge;

/// <summary>A field alias, a <c>FIELDALIAS-</c> attribute in <c>props.conf</c> (<c>data/props/fieldaliases</c>).</summary>
/// <remarks>
/// Splunk returns each alias as <c>alias.{n}.{field}</c> = <c>{alias}</c>; <see cref="Aliases"/> collects them. The raw
/// properties stay in <see cref="SplunkContent.AdditionalProperties"/>.
/// </remarks>
public sealed class FieldAlias : PropsEntry
{
	/// <summary>Whether an alias overwrites a field that already has a value (<c>overwrite</c>, <c>ASNEW</c> when false).</summary>
	[JsonPropertyName("overwrite")]
	public bool? Overwrite { get; init; }

	/// <summary>The aliases as (original field, alias) pairs, in Splunk's order; a field can have more than one alias.</summary>
	[JsonIgnore]
	public IReadOnlyList<KeyValuePair<string, string>> Aliases => IndexedProperties.Collect(AdditionalProperties, "alias.");
}
