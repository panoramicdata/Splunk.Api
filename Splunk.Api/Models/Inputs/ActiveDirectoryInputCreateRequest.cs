using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Inputs;

/// <summary>Creates an Active Directory monitoring input (<c>POST data/inputs/ad</c>). Windows only.</summary>
public sealed class ActiveDirectoryInputCreateRequest : ActiveDirectoryInputUpdateRequest
{
	/// <summary>A unique name for the configuration.</summary>
	[JsonPropertyName("name")]
	public required string Name { get; init; }
}
