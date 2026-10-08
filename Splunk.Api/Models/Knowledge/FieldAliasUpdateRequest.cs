using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Knowledge;

/// <summary>Replaces the aliases of a field alias group (<c>POST data/props/fieldaliases/{name}</c>).</summary>
/// <remarks>Splunk replaces the group's aliases with those sent: aliases left out are removed.</remarks>
public sealed class FieldAliasUpdateRequest : SplunkFormRequest
{
	private const string AliasPrefix = "alias.";

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
