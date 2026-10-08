using Splunk.Api.Serialization;
using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Cluster;

/// <summary>A search head cluster member, as the captain sees it (<c>shcluster/captain/members</c>).</summary>
public sealed class ShClusterMember : SplunkContent
{
	/// <summary>The member's name.</summary>
	[JsonPropertyName("label")]
	public string? Label { get; init; }

	/// <summary>The member status.</summary>
	[JsonPropertyName("status")]
	public ClusterPeerStatus Status { get; init; }

	/// <summary>The member's site.</summary>
	[JsonPropertyName("site")]
	public string? Site { get; init; }

	/// <summary>Whether the member is the captain.</summary>
	[JsonPropertyName("is_captain")]
	public bool? IsCaptain { get; init; }

	/// <summary>Whether the member prefers to be captain.</summary>
	[JsonPropertyName("preferred_captain")]
	public bool? PreferredCaptain { get; init; }

	/// <summary>Whether the member runs no scheduled searches.</summary>
	[JsonPropertyName("adhoc_searchhead")]
	public bool? AdhocSearchHead { get; init; }

	/// <summary>Whether the member says it needs a restart.</summary>
	[JsonPropertyName("advertise_restart_required")]
	public bool? AdvertiseRestartRequired { get; init; }

	/// <summary>The host and management port the member advertises.</summary>
	[JsonPropertyName("host_port_pair")]
	public string? HostPortPair { get; init; }

	/// <summary>The member's management URI.</summary>
	[JsonPropertyName("mgmt_uri")]
	public string? ManagementUri { get; init; }

	/// <summary>The member's URI.</summary>
	[JsonPropertyName("peer_scheme_host_port")]
	public string? PeerSchemeHostPort { get; init; }

	/// <summary>The host and port of the member's KV store.</summary>
	[JsonPropertyName("kv_store_host_port")]
	public string? KvStoreHostPort { get; init; }

	/// <summary>When the captain last heard from the member.</summary>
	[JsonPropertyName("last_heartbeat")]
	[JsonConverter(typeof(EpochSecondsConverter))]
	public DateTimeOffset? LastHeartbeat { get; init; }

	/// <summary>The number of artifacts on the member.</summary>
	[JsonPropertyName("artifact_count")]
	public int? ArtifactCount { get; init; }

	/// <summary>The number of jobs the captain has pending on the member.</summary>
	[JsonPropertyName("pending_job_count")]
	public int? PendingJobCount { get; init; }

	/// <summary>The number of replications the member takes part in.</summary>
	[JsonPropertyName("replication_count")]
	public int? ReplicationCount { get; init; }

	/// <summary>The member's replication port.</summary>
	[JsonPropertyName("replication_port")]
	public int? ReplicationPort { get; init; }

	/// <summary>Whether the member replicates over TLS.</summary>
	[JsonPropertyName("replication_use_ssl")]
	public bool? ReplicationUseSsl { get; init; }

	/// <summary>Whether the member takes no artifact replications.</summary>
	[JsonPropertyName("no_artifact_replications")]
	public bool? NoArtifactReplications { get; init; }

	/// <summary>The artifacts waiting to be deleted from the member.</summary>
	[JsonPropertyName("delayed_artifacts_to_discard")]
	public IReadOnlyList<string> DelayedArtifactsToDiscard { get; init; } = [];

	/// <summary>Artifacts awaiting fixup.</summary>
	[JsonPropertyName("fixup_set")]
	public IReadOnlyList<string> FixupSet { get; init; } = [];

	/// <summary>The number of artifacts by status.</summary>
	[JsonPropertyName("status_counter")]
	public IReadOnlyDictionary<string, int> StatusCounter { get; init; } = new Dictionary<string, int>();
}
