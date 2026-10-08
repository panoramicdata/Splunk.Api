using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Topology;

/// <summary>Network details of a node, as <c>node-identity</c> reports them.</summary>
public sealed class NodeIdentityHostInfo
{
	/// <summary>The fully qualified domain name.</summary>
	[JsonPropertyName("fqdn")]
	public string? Fqdn { get; init; }

	/// <summary>The management protocol, <c>http</c> or <c>https</c>.</summary>
	[JsonPropertyName("mgmt_scheme")]
	public string? ManagementScheme { get; init; }

	/// <summary>The host name or address of the management interface.</summary>
	[JsonPropertyName("mgmt_hostname")]
	public string? ManagementHostname { get; init; }

	/// <summary>The management port.</summary>
	[JsonPropertyName("mgmt_port")]
	public int? ManagementPort { get; init; }

	/// <summary>The Splunk Web protocol, <c>http</c> or <c>https</c>.</summary>
	[JsonPropertyName("web_scheme")]
	public string? WebScheme { get; init; }

	/// <summary>The Splunk Web port.</summary>
	[JsonPropertyName("web_port")]
	public int? WebPort { get; init; }
}
