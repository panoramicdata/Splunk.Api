using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Knowledge;

/// <summary>An automatic lookup, a <c>LOOKUP-</c> attribute in <c>props.conf</c> (<c>data/props/lookups</c>).</summary>
/// <remarks>
/// Splunk returns the matched columns as <c>lookup.field.input.{column}</c> and the output columns as
/// <c>lookup.field.output.{n}.{column}</c>, each with the event field name as its value (empty when it is the same as
/// the column); <see cref="InputFields"/> and <see cref="OutputFields"/> collect them.
/// </remarks>
public sealed class AutomaticLookup : PropsEntry
{
	/// <summary>Whether output fields always overwrite existing values (<c>OUTPUT</c>) or only fill missing ones (<c>OUTPUTNEW</c>).</summary>
	[JsonPropertyName("overwrite")]
	public bool? Overwrite { get; init; }

	/// <summary>The lookup definition (<c>transforms.conf</c> stanza) the lookup applies.</summary>
	[JsonPropertyName("transform")]
	public string? Transform { get; init; }

	/// <summary>The lookup columns matched against events, as (column, event field name or empty) pairs.</summary>
	[JsonIgnore]
	public IReadOnlyList<KeyValuePair<string, string>> InputFields => IndexedProperties.Collect(AdditionalProperties, "lookup.field.input.");

	/// <summary>The lookup columns written to events, as (column, event field name or empty) pairs.</summary>
	[JsonIgnore]
	public IReadOnlyList<KeyValuePair<string, string>> OutputFields => IndexedProperties.Collect(AdditionalProperties, "lookup.field.output.");
}
