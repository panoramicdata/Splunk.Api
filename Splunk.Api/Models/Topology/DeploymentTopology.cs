using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Topology;

/// <summary>The deployment topology seen by the license manager (<c>stack-explainer/v1/topology</c>).</summary>
public sealed class DeploymentTopology
{
	/// <summary>The response header.</summary>
	[JsonPropertyName("header")]
	public TopologyHeader? Header { get; init; }

	/// <summary>The license manager.</summary>
	[JsonPropertyName("license_manager")]
	public TopologyNode? LicenseManager { get; init; }

	/// <summary>The cluster manager, if there is one.</summary>
	[JsonPropertyName("cluster_manager")]
	public TopologyNode? ClusterManager { get; init; }

	/// <summary>The indexers.</summary>
	[JsonPropertyName("indexers")]
	public IReadOnlyList<TopologyNode> Indexers { get; init; } = [];

	/// <summary>The search heads.</summary>
	[JsonPropertyName("search_heads")]
	public IReadOnlyList<TopologyNode> SearchHeads { get; init; } = [];

	/// <summary>The search head cluster deployers.</summary>
	[JsonPropertyName("deployers")]
	public IReadOnlyList<TopologyNode> Deployers { get; init; } = [];

	/// <summary>Nodes that could not be assigned a known role.</summary>
	[JsonPropertyName("unrecognized")]
	public IReadOnlyList<TopologyNode> Unrecognized { get; init; } = [];

	/// <summary>Unmanaged nodes; returned only when they are asked for.</summary>
	[JsonPropertyName("unmanaged_actors")]
	public IReadOnlyList<TopologyUnmanagedActor> UnmanagedActors { get; init; } = [];
}
