using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Outputs;

/// <summary>Changes a forwarding target group (<c>POST data/outputs/tcp/group/{name}</c>).</summary>
public class TcpOutputGroupUpdateRequest : SplunkFormRequest
{
	/// <summary>The receivers in the group, as comma-separated <c>host:port</c> values.</summary>
	[JsonPropertyName("servers")]
	public required string Servers { get; init; }

	/// <summary>Whether data is compressed; the receiving port must accept compression.</summary>
	[JsonPropertyName("compressed")]
	public bool? Compressed { get; init; }

	/// <summary>Whether the group is disabled.</summary>
	[JsonPropertyName("disabled")]
	public bool? Disabled { get; init; }

	/// <summary>Seconds to wait before dropping events when the queue is full; -1 never drops (<c>dropEventsOnQueueFull</c>).</summary>
	[JsonPropertyName("dropEventsOnQueueFull")]
	public int? DropEventsOnQueueFull { get; init; }

	/// <summary>Seconds between heartbeats (<c>heartbeatFrequency</c>).</summary>
	[JsonPropertyName("heartbeatFrequency")]
	public int? HeartbeatFrequency { get; init; }

	/// <summary>The output queue's maximum size: an integer, optionally with <c>KB</c>, <c>MB</c> or <c>GB</c>, or <c>auto</c>.</summary>
	[JsonPropertyName("maxQueueSize")]
	public string? MaxQueueSize { get; init; }

	/// <summary>
	/// How data is distributed: <c>autobalance</c> or <c>clone</c>. The reference lists <c>tcpout</c> and
	/// <c>syslog</c>, which Splunk 10.6 rejects.
	/// </summary>
	[JsonPropertyName("method")]
	public string? Method { get; init; }

	/// <summary>Whether events are sent cooked; set <see langword="false"/> for a third-party receiver (<c>sendCookedData</c>).</summary>
	[JsonPropertyName("sendCookedData")]
	public bool? SendCookedData { get; init; }

	/// <summary>The token the receivers require, if any.</summary>
	[JsonPropertyName("token")]
	public string? Token { get; init; }
}
