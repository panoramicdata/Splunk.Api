using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Topology;

/// <summary>The GUIDs of the entities that manage a node; each is <see langword="null"/> when that entity does not manage it.</summary>
public sealed class TopologyManagedBy
{
	/// <summary>The GUID of the managing cluster manager.</summary>
	[JsonPropertyName("cluster_manager")]
	public string? ClusterManager { get; init; }

	/// <summary>The GUID of the managing license manager.</summary>
	[JsonPropertyName("license_manager")]
	public string? LicenseManager { get; init; }

	/// <summary>The GUID of the search head cluster (a logical group, not a node: it cannot be passed to <c>node-identity/{guid}</c>).</summary>
	[JsonPropertyName("search_head_cluster")]
	public string? SearchHeadCluster { get; init; }

	/// <summary>The GUID of the managing deployer.</summary>
	[JsonPropertyName("deployer")]
	public string? Deployer { get; init; }
}
