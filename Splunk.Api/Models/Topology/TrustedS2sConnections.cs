using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Topology;

/// <summary>Splunk-to-Splunk (S2S) details of a node's trusted connections.</summary>
public sealed class TrustedS2sConnections
{
	/// <summary>The cooked TCP receiving ports, each with the IP addresses that sent to it.</summary>
	[JsonPropertyName("receiving_ports")]
	public IReadOnlyDictionary<string, IReadOnlyList<string>> ReceivingPorts { get; init; } = new Dictionary<string, IReadOnlyList<string>>();

	/// <summary>The hosts this node forwards to, as <c>host:port</c>.</summary>
	[JsonPropertyName("forwarding_hosts")]
	public IReadOnlyList<string> ForwardingHosts { get; init; } = [];
}
