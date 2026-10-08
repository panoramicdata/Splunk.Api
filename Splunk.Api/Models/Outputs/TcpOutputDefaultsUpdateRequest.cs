using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Outputs;

/// <summary>Changes the global forwarding settings (<c>POST data/outputs/tcp/default/tcpout</c>). Unset properties are left unchanged.</summary>
public class TcpOutputDefaultsUpdateRequest : SplunkFormRequest
{
	/// <summary>The target groups to forward all data to, comma-separated (<c>defaultGroup</c>).</summary>
	[JsonPropertyName("defaultGroup")]
	public string? DefaultGroup { get; init; }

	/// <summary>Whether the default forwarding settings are disabled.</summary>
	[JsonPropertyName("disabled")]
	public bool? Disabled { get; init; }

	/// <summary>
	/// Seconds to wait before dropping events when the queue is full; -1 never drops (<c>dropEventsOnQueueFull</c>). Do
	/// not set a positive value when monitoring files.
	/// </summary>
	[JsonPropertyName("dropEventsOnQueueFull")]
	public int? DropEventsOnQueueFull { get; init; }

	/// <summary>Seconds between heartbeats to receivers (<c>heartbeatFrequency</c>).</summary>
	[JsonPropertyName("heartbeatFrequency")]
	public int? HeartbeatFrequency { get; init; }

	/// <summary>Whether data is also indexed locally, on a heavy forwarder (<c>indexAndForward</c>).</summary>
	[JsonPropertyName("indexAndForward")]
	public bool? IndexAndForward { get; init; }

	/// <summary>The output queue's maximum size: an integer, optionally with <c>KB</c>, <c>MB</c> or <c>GB</c>, or <c>auto</c>.</summary>
	[JsonPropertyName("maxQueueSize")]
	public string? MaxQueueSize { get; init; }

	/// <summary>Whether events are sent cooked; set <see langword="false"/> for a third-party receiver (<c>sendCookedData</c>).</summary>
	[JsonPropertyName("sendCookedData")]
	public bool? SendCookedData { get; init; }
}
