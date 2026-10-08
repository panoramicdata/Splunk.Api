using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Outputs;

/// <summary>The global forwarding settings, the <c>[tcpout]</c> stanza of <c>outputs.conf</c> (<c>data/outputs/tcp/default</c>).</summary>
public sealed class TcpOutputDefaults : SplunkContent
{
	/// <summary>The target groups all data is forwarded to, comma-separated (<c>defaultGroup</c>); empty when forwarding is off.</summary>
	[JsonPropertyName("defaultGroup")]
	public string? DefaultGroup { get; init; }

	/// <summary>Whether data is also indexed locally (<c>indexAndForward</c>).</summary>
	[JsonPropertyName("indexAndForward")]
	public bool? IndexAndForward { get; init; }

	/// <summary>Whether events are sent cooked (processed by Splunk) rather than raw (<c>sendCookedData</c>).</summary>
	[JsonPropertyName("sendCookedData")]
	public bool? SendCookedData { get; init; }

	/// <summary>Seconds between heartbeats to receivers (<c>heartbeatFrequency</c>).</summary>
	[JsonPropertyName("heartbeatFrequency")]
	public int? HeartbeatFrequency { get; init; }

	/// <summary>The output queue's maximum size, for example <c>auto</c> or <c>7MB</c> (<c>maxQueueSize</c>).</summary>
	[JsonPropertyName("maxQueueSize")]
	public string? MaxQueueSize { get; init; }

	/// <summary>Seconds to wait before dropping events when the queue is full; -1 never drops (<c>dropEventsOnQueueFull</c>).</summary>
	[JsonPropertyName("dropEventsOnQueueFull")]
	public int? DropEventsOnQueueFull { get; init; }

	/// <summary>Whether data is compressed.</summary>
	[JsonPropertyName("compressed")]
	public bool? Compressed { get; init; }

	/// <summary>Whether indexer acknowledgement is used (<c>useACK</c>).</summary>
	[JsonPropertyName("useACK")]
	public bool? UseAck { get; init; }

	/// <summary>Seconds between switching receivers when load balancing (<c>autoLBFrequency</c>).</summary>
	[JsonPropertyName("autoLBFrequency")]
	public int? AutoLoadBalanceFrequency { get; init; }

	/// <summary>Seconds to wait for a connection (<c>connectionTimeout</c>).</summary>
	[JsonPropertyName("connectionTimeout")]
	public int? ConnectionTimeout { get; init; }

	/// <summary>Seconds to wait for a read (<c>readTimeout</c>).</summary>
	[JsonPropertyName("readTimeout")]
	public int? ReadTimeout { get; init; }

	/// <summary>Seconds to wait for a write (<c>writeTimeout</c>).</summary>
	[JsonPropertyName("writeTimeout")]
	public int? WriteTimeout { get; init; }

	/// <summary>Whether filtering of forwarded data by index is off (<c>forwardedindex.filter.disable</c>).</summary>
	[JsonPropertyName("forwardedindex.filter.disable")]
	public bool? ForwardedIndexFilterDisabled { get; init; }
}
