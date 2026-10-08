using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Topology;

/// <summary>A managed node in the deployment topology.</summary>
/// <remarks>Splunk 10.6 names the version and operating system objects <c>version-info</c> and <c>os-info</c> here, although the reference documents <c>version_info</c> and <c>os_info</c> (the spelling <c>node-identity</c> uses).</remarks>
public sealed class TopologyNode
{
	/// <summary>The node's unique identifier.</summary>
	[JsonPropertyName("guid")]
	public string? NodeGuid { get; init; }

	/// <summary>The server name.</summary>
	[JsonPropertyName("label")]
	public string? Label { get; init; }

	/// <summary>Whether the node runs in FIPS mode.</summary>
	[JsonPropertyName("fips_enabled")]
	public bool? FipsEnabled { get; init; }

	/// <summary>The server roles, for example <c>indexer</c> or <c>license_manager</c>.</summary>
	[JsonPropertyName("roles")]
	public IReadOnlyList<string> Roles { get; init; } = [];

	/// <summary>Network details.</summary>
	[JsonPropertyName("host_info")]
	public TopologyHostInfo? HostInfo { get; init; }

	/// <summary>The Splunk build and version.</summary>
	[JsonPropertyName("version-info")]
	public TopologyVersionInfo? VersionInfo { get; init; }

	/// <summary>The operating system.</summary>
	[JsonPropertyName("os-info")]
	public TopologyOsInfo? OsInfo { get; init; }

	/// <summary>The node status, where known.</summary>
	[JsonPropertyName("status")]
	public string? Status { get; init; }

	/// <summary>When the node last sent a heartbeat; present only for indexers and search heads the cluster manager knows.</summary>
	[JsonPropertyName("last_heartbeat")]
	public string? LastHeartbeat { get; init; }

	/// <summary>The entities that manage the node.</summary>
	[JsonPropertyName("managed_by")]
	public TopologyManagedBy? ManagedBy { get; init; }
}
