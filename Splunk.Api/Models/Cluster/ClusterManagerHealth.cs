using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Cluster;

/// <summary>The cluster health checks run before a rolling upgrade (<c>cluster/manager/health</c>).</summary>
/// <remarks>The reference names one check <c>no_fixups_in_progress</c>; its example, like Splunk, returns <c>no_fixup_tasks_in_progress</c>.</remarks>
public sealed class ClusterManagerHealth : SplunkContent
{
	/// <summary>Whether every check passed.</summary>
	[JsonPropertyName("pre_flight_check")]
	public bool? PreFlightCheck { get; init; }

	/// <summary>Whether all data is searchable.</summary>
	[JsonPropertyName("all_data_is_searchable")]
	public bool? AllDataIsSearchable { get; init; }

	/// <summary>Whether every peer is <c>Up</c>.</summary>
	[JsonPropertyName("all_peers_are_up")]
	public bool? AllPeersAreUp { get; init; }

	/// <summary>Whether no peer runs a newer Splunk version than the manager.</summary>
	[JsonPropertyName("cm_version_is_compatible")]
	public bool? ManagerVersionIsCompatible { get; init; }

	/// <summary>Whether the cluster is multisite.</summary>
	[JsonPropertyName("multisite")]
	public bool? Multisite { get; init; }

	/// <summary>Whether no bucket fixup is in progress.</summary>
	[JsonPropertyName("no_fixup_tasks_in_progress")]
	public bool? NoFixupTasksInProgress { get; init; }

	/// <summary>Whether the replication factor is met (single-site).</summary>
	[JsonPropertyName("replication_factor_met")]
	public bool? ReplicationFactorMet { get; init; }

	/// <summary>Whether the search factor is met (single-site).</summary>
	[JsonPropertyName("search_factor_met")]
	public bool? SearchFactorMet { get; init; }

	/// <summary>Whether the site replication factor is met (multisite).</summary>
	[JsonPropertyName("site_replication_factor_met")]
	public bool? SiteReplicationFactorMet { get; init; }

	/// <summary>Whether the site search factor is met (multisite).</summary>
	[JsonPropertyName("site_search_factor_met")]
	public bool? SiteSearchFactorMet { get; init; }

	/// <summary>The number of peers per Splunk version, as Splunk formats it, for example <c>{ 10.6.0: 3 }</c>.</summary>
	[JsonPropertyName("splunk_version_peer_count")]
	public string? SplunkVersionPeerCount { get; init; }
}
