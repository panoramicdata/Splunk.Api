using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Inputs;

/// <summary>Changes a Windows event log collection (<c>POST data/inputs/win-event-log-collections/{name}</c>). Windows only.</summary>
public class WindowsEventLogInputUpdateRequest : SplunkFormRequest
{
	/// <summary>The first host to monitor (<c>lookup_host</c>); add more with <see cref="Hosts"/>.</summary>
	[JsonPropertyName("lookup_host")]
	public required string LookupHost { get; init; }

	/// <summary>Additional hosts to monitor through WMI, comma-separated.</summary>
	[JsonPropertyName("hosts")]
	public string? Hosts { get; init; }

	/// <summary>The index events are stored in.</summary>
	[JsonPropertyName("index")]
	public string? Index { get; init; }

	/// <summary>The event log channels to monitor, sent as repeated <c>logs</c> fields.</summary>
	[JsonPropertyName("logs")]
	public IReadOnlyList<string>? Logs { get; init; }
}
