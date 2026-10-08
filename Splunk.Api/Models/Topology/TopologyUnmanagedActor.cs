using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Topology;

/// <summary>A node that talks to the deployment (typically by sending data) but whose details cannot be verified.</summary>
public sealed class TopologyUnmanagedActor
{
	/// <summary>The node's unique identifier, if known.</summary>
	[JsonPropertyName("guid")]
	public string? NodeGuid { get; init; }

	/// <summary>The host name or display label.</summary>
	[JsonPropertyName("label")]
	public string? Label { get; init; }

	/// <summary>Network details.</summary>
	[JsonPropertyName("host_info")]
	public TopologyHostInfo? HostInfo { get; init; }

	/// <summary>The GUIDs of the indexers the node is connected to.</summary>
	[JsonPropertyName("indexer_guids")]
	public IReadOnlyList<string> IndexerGuids { get; init; } = [];

	/// <summary>The connection types in use, for example <c>tcp/cooked</c>, <c>tcp/raw</c>, <c>udp</c> or <c>http</c>.</summary>
	[JsonPropertyName("connection_types")]
	public IReadOnlyList<string> ConnectionTypes { get; init; } = [];

	/// <summary>When the node last connected (Splunk Enterprise only).</summary>
	[JsonPropertyName("last_conn_time")]
	public string? LastConnectionTime { get; init; }
}
