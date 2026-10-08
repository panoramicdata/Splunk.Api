using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Knowledge;

/// <summary>The settings of an automatic lookup, shared by <see cref="AutomaticLookupCreateRequest"/> and <see cref="AutomaticLookupUpdateRequest"/>.</summary>
public abstract class AutomaticLookupSettings : SplunkFormRequest
{
	private const string InputPrefix = "lookup.field.input.";
	private const string OutputPrefix = "lookup.field.output.";

	/// <summary>The lookup definition (<c>transforms.conf</c> stanza) to apply.</summary>
	[JsonPropertyName("transform")]
	public required string Transform { get; init; }

	/// <summary>Whether output fields always overwrite existing values (<c>OUTPUT</c>) or only fill missing ones (<c>OUTPUTNEW</c>).</summary>
	[JsonPropertyName("overwrite")]
	public required bool Overwrite { get; init; }

	/// <summary>
	/// The lookup columns to match against events, keyed by column, with the event field name as the value, or empty
	/// when it has the same name; each is sent as <c>lookup.field.input.{column}</c>. Stored in
	/// <see cref="SplunkFormRequest.AdditionalParameters"/>, so set that (if at all) before this property.
	/// </summary>
	[JsonIgnore]
	public IReadOnlyDictionary<string, string> InputFields
	{
		get => PrefixedParameters.Get(AdditionalParameters, InputPrefix);
		init => PrefixedParameters.Set(AdditionalParameters, InputPrefix, value);
	}

	/// <summary>
	/// The lookup columns to write to events, keyed by column, with the event field name as the value, or empty when it
	/// has the same name; each is sent as <c>lookup.field.output.{column}</c>. Stored in
	/// <see cref="SplunkFormRequest.AdditionalParameters"/>, so set that (if at all) before this property.
	/// </summary>
	[JsonIgnore]
	public IReadOnlyDictionary<string, string> OutputFields
	{
		get => PrefixedParameters.Get(AdditionalParameters, OutputPrefix);
		init => PrefixedParameters.Set(AdditionalParameters, OutputPrefix, value);
	}
}
