using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Inputs;

/// <summary>A modular input kind defined by an app (<c>data/modular-inputs</c>), with the parameters its inputs take.</summary>
public sealed class ModularInput : SplunkContent
{
	/// <summary>The label shown on the Data inputs page.</summary>
	[JsonPropertyName("title")]
	public string? Title { get; init; }

	/// <summary>A description of the input kind.</summary>
	[JsonPropertyName("description")]
	public string? Description { get; init; }

	/// <summary>How the script streams events: <c>xml</c>, <c>simple</c> or <c>json</c> (<c>streaming_mode</c>).</summary>
	[JsonPropertyName("streaming_mode")]
	public string? StreamingMode { get; init; }

	/// <summary>Whether one script instance serves every input of this kind (<c>use_single_instance</c>).</summary>
	[JsonPropertyName("use_single_instance")]
	public bool? UseSingleInstance { get; init; }

	/// <summary>The parameters an input of this kind takes.</summary>
	[JsonPropertyName("endpoint")]
	public ModularInputEndpoint? Endpoint { get; init; }
}
