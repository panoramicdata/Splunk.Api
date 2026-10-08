using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Topology;

/// <summary>Network details of a node in the deployment topology.</summary>
public sealed class TopologyHostInfo
{
	/// <summary>The management protocol, <c>http</c> or <c>https</c>.</summary>
	[JsonPropertyName("scheme")]
	public string? Scheme { get; init; }

	/// <summary>The IP address.</summary>
	[JsonPropertyName("ip")]
	public string? Ip { get; init; }

	/// <summary>The fully qualified domain name.</summary>
	[JsonPropertyName("fqdn")]
	public string? Fqdn { get; init; }

	/// <summary>The management port, for example 8089.</summary>
	[JsonPropertyName("mgmt_port")]
	public int? ManagementPort { get; init; }

	/// <summary>The Splunk Web port, for example 8000.</summary>
	[JsonPropertyName("web-port")]
	public int? WebPort { get; init; }
}
