using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Inputs;

/// <summary>Creates a WMI collection (<c>POST data/inputs/win-wmi-collections</c>). Windows only.</summary>
public sealed class WmiInputCreateRequest : WmiInputUpdateRequest
{
	/// <summary>The collection's name, which also becomes the events' source and sourcetype.</summary>
	[JsonPropertyName("name")]
	public required string Name { get; init; }
}
