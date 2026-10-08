using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Inputs;

/// <summary>Creates a file or directory monitor input (<c>POST data/inputs/monitor</c>).</summary>
public sealed class MonitorInputCreateRequest : MonitorInputUpdateRequest
{
	/// <summary>The file or directory path to monitor on the Splunk server.</summary>
	[JsonPropertyName("name")]
	public required string Name { get; init; }
}
