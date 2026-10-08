using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Topology;

/// <summary>The identity of a Splunk node (<c>stack-explainer/v1/node-identity</c>).</summary>
public sealed class NodeIdentity
{
	/// <summary>The response header.</summary>
	[JsonPropertyName("header")]
	public TopologyHeader? Header { get; init; }

	/// <summary>Whether the node runs in FIPS mode.</summary>
	[JsonPropertyName("fips_enabled")]
	public bool? FipsEnabled { get; init; }

	/// <summary>Network details.</summary>
	[JsonPropertyName("host_info")]
	public NodeIdentityHostInfo? HostInfo { get; init; }

	/// <summary>The server roles.</summary>
	[JsonPropertyName("roles")]
	public IReadOnlyList<string> Roles { get; init; } = [];

	/// <summary>The Splunk build and version.</summary>
	[JsonPropertyName("version_info")]
	public TopologyVersionInfo? VersionInfo { get; init; }

	/// <summary>The operating system.</summary>
	[JsonPropertyName("os_info")]
	public TopologyOsInfo? OsInfo { get; init; }
}
