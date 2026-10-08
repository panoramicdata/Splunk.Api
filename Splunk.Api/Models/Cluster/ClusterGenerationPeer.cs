using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Cluster;

/// <summary>A peer in a cluster generation.</summary>
public sealed class ClusterGenerationPeer
{
	/// <summary>The peer's replication host and port.</summary>
	[JsonPropertyName("host_port_pair")]
	public string? HostPortPair { get; init; }

	/// <summary>The peer's server name.</summary>
	[JsonPropertyName("peer")]
	public string? Peer { get; init; }
}
