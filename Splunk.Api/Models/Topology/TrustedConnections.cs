using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Topology;

/// <summary>The trusted connections of a node: HEC, S2S, TCP and UDP inputs and search peers (<c>stack-explainer/v1/trusted-connections</c>).</summary>
public sealed class TrustedConnections
{
	/// <summary>The response header.</summary>
	[JsonPropertyName("header")]
	public TopologyHeader? Header { get; init; }

	/// <summary>The GUID of the node described.</summary>
	[JsonPropertyName("guid")]
	public string? NodeGuid { get; init; }

	/// <summary>HTTP Event Collector details.</summary>
	[JsonPropertyName("hec")]
	public TrustedHecConnections? Hec { get; init; }

	/// <summary>Splunk-to-Splunk details.</summary>
	[JsonPropertyName("s2s")]
	public TrustedS2sConnections? S2s { get; init; }

	/// <summary>The TCP input ports, each with the IP addresses that sent to it.</summary>
	[JsonPropertyName("tcp_inputs")]
	public IReadOnlyDictionary<string, IReadOnlyList<string>> TcpInputs { get; init; } = new Dictionary<string, IReadOnlyList<string>>();

	/// <summary>The UDP input ports, each with the IP addresses that sent to it.</summary>
	[JsonPropertyName("udp_inputs")]
	public IReadOnlyDictionary<string, IReadOnlyList<string>> UdpInputs { get; init; } = new Dictionary<string, IReadOnlyList<string>>();

	/// <summary>The search peers; present only when the node is a search head.</summary>
	[JsonPropertyName("searchPeers")]
	public IReadOnlyList<TrustedSearchPeer> SearchPeers { get; init; } = [];
}
