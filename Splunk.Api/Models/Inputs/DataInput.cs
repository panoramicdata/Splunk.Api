using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Inputs;

/// <summary>An input of any kind, including modular inputs (<c>data/inputs/all</c>). Kind-specific properties are in <see cref="SplunkContent.AdditionalProperties"/>.</summary>
public sealed class DataInput : InputContent
{
	/// <summary>The input's kind, for example <c>monitor</c>, <c>script</c>, <c>cooked</c> or a modular input name (<c>eai:type</c>).</summary>
	[JsonPropertyName("eai:type")]
	public string? Kind { get; init; }

	/// <summary>The endpoint that manages inputs of this kind, for example <c>/data/inputs/monitor</c> (<c>eai:location</c>).</summary>
	[JsonPropertyName("eai:location")]
	public string? Location { get; init; }
}
