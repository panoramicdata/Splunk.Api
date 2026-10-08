using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Inputs;

/// <summary>Creates a registry monitoring input (<c>POST data/inputs/registry</c>). Windows only.</summary>
public sealed class RegistryInputCreateRequest : RegistryInputUpdateRequest
{
	/// <summary>The configuration stanza's name.</summary>
	[JsonPropertyName("name")]
	public required string Name { get; init; }
}
