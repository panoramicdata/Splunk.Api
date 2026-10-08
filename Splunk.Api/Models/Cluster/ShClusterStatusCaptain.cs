using System.Text.Json;
using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Cluster;

/// <summary>The captain in the search head cluster status.</summary>
public sealed class ShClusterStatusCaptain
{
	/// <summary>The search head cluster label.</summary>
	[JsonPropertyName("label")]
	public string? Label { get; init; }

	/// <summary>The search head cluster GUID.</summary>
	[JsonPropertyName("id")]
	public string? ClusterId { get; init; }

	/// <summary>The captain's management URI.</summary>
	[JsonPropertyName("mgmt_uri")]
	public string? ManagementUri { get; init; }

	/// <summary>When the captain was elected, as Splunk formats it.</summary>
	[JsonPropertyName("elected_captain")]
	public string? ElectedCaptain { get; init; }

	/// <summary>Whether the captain is elected rather than static.</summary>
	[JsonPropertyName("dynamic_captain")]
	public bool? DynamicCaptain { get; init; }

	/// <summary>Whether the captain is stable, judged by heartbeats.</summary>
	[JsonPropertyName("stable_captain")]
	public bool? StableCaptain { get; init; }

	/// <summary>Whether the captain is initialized.</summary>
	[JsonPropertyName("initialized_flag")]
	public bool? InitializedFlag { get; init; }

	/// <summary>Whether at least replication factor members have joined.</summary>
	[JsonPropertyName("min_peers_joined_flag")]
	public bool? MinPeersJoinedFlag { get; init; }

	/// <summary>Whether the cluster is ready.</summary>
	[JsonPropertyName("service_ready_flag")]
	public bool? ServiceReadyFlag { get; init; }

	/// <summary>The rolling restart mode, <c>restart</c> or <c>searchable</c>.</summary>
	[JsonPropertyName("rolling_restart")]
	public string? RollingRestart { get; init; }

	/// <summary>Whether a rolling restart is in progress.</summary>
	[JsonPropertyName("rolling_restart_flag")]
	public bool? RollingRestartFlag { get; init; }

	/// <summary>Whether a rolling upgrade is in progress.</summary>
	[JsonPropertyName("rolling_upgrade_flag")]
	public bool? RollingUpgradeFlag { get; init; }

	/// <summary>How many more members can fail before the cluster loses its majority.</summary>
	[JsonPropertyName("max_failures_to_keep_majority")]
	public int? MaxFailuresToKeepMajority { get; init; }

	/// <summary>How long, in seconds, a member waits for searches before it goes down.</summary>
	[JsonPropertyName("decommission_search_jobs_wait_secs")]
	public int? DecommissionSearchJobsWaitSeconds { get; init; }

	/// <summary>Every property Splunk returned that this type does not model, keyed by its wire name.</summary>
	[JsonExtensionData]
	public IDictionary<string, JsonElement> AdditionalProperties { get; init; } = new Dictionary<string, JsonElement>();
}
