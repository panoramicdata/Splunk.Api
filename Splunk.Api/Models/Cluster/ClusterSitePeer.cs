using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Cluster;

/// <summary>A peer of a cluster site.</summary>
public sealed class ClusterSitePeer
{
	/// <summary>The peer's host and port.</summary>
	[JsonPropertyName("host_port_pair")]
	public string? HostPortPair { get; init; }

	/// <summary>The peer's server name.</summary>
	[JsonPropertyName("server_name")]
	public string? ServerName { get; init; }
}
