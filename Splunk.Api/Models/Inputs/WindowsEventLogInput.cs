using Splunk.Api.Serialization;
using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Inputs;

/// <summary>
/// A Windows event log collection (<c>data/inputs/win-event-log-collections</c>). Windows only. The collection named
/// <c>localhost</c> reads the local event logs natively; any other uses WMI.
/// </summary>
public sealed class WindowsEventLogInput : InputContent
{
	/// <summary>The hosts monitored, comma-separated.</summary>
	[JsonPropertyName("hosts")]
	public string? Hosts { get; init; }

	/// <summary>The event log channels monitored, for example <c>Application</c>, <c>Security</c>, <c>System</c>.</summary>
	[JsonPropertyName("logs")]
	[JsonConverter(typeof(TolerantStringListConverter))]
	public IReadOnlyList<string> Logs { get; init; } = [];

	/// <summary>The first host monitored (<c>lookup_host</c>).</summary>
	[JsonPropertyName("lookup_host")]
	public string? LookupHost { get; init; }
}
