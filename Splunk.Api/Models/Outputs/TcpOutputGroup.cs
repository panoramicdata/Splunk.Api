using Splunk.Api.Serialization;
using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Outputs;

/// <summary>A forwarding target group, a <c>[tcpout:{name}]</c> stanza (<c>data/outputs/tcp/group</c>).</summary>
public sealed class TcpOutputGroup : SplunkContent
{
	/// <summary>The receivers in the group, as <c>host:port</c>.</summary>
	[JsonPropertyName("servers")]
	[JsonConverter(typeof(TolerantStringListConverter))]
	public IReadOnlyList<string> Servers { get; init; } = [];

	/// <summary>How data is distributed across the receivers, for example <c>autobalance</c> or <c>clone</c>.</summary>
	[JsonPropertyName("method")]
	public string? Method { get; init; }

	/// <summary>Whether data is compressed.</summary>
	[JsonPropertyName("compressed")]
	public bool? Compressed { get; init; }

	/// <summary>Seconds between heartbeats (<c>heartbeatFrequency</c>).</summary>
	[JsonPropertyName("heartbeatFrequency")]
	public int? HeartbeatFrequency { get; init; }

	/// <summary>The output queue's maximum size (<c>maxQueueSize</c>).</summary>
	[JsonPropertyName("maxQueueSize")]
	public string? MaxQueueSize { get; init; }

	/// <summary>Seconds to wait before dropping events when the queue is full (<c>dropEventsOnQueueFull</c>).</summary>
	[JsonPropertyName("dropEventsOnQueueFull")]
	public int? DropEventsOnQueueFull { get; init; }

	/// <summary>Whether events are sent cooked (<c>sendCookedData</c>).</summary>
	[JsonPropertyName("sendCookedData")]
	public bool? SendCookedData { get; init; }

	/// <summary>Whether the forwarder load-balances automatically (<c>autoLB</c>).</summary>
	[JsonPropertyName("autoLB")]
	public bool? AutoLoadBalance { get; init; }
}
