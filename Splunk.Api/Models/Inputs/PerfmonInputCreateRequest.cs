using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Inputs;

/// <summary>Creates a Performance Monitor input (<c>POST data/inputs/win-perfmon</c>). Windows only.</summary>
public sealed class PerfmonInputCreateRequest : PerfmonInputUpdateRequest
{
	/// <summary>The collection's name, which also becomes the events' source and sourcetype.</summary>
	[JsonPropertyName("name")]
	public required string Name { get; init; }
}
