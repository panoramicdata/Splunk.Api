using Splunk.Api.Serialization;
using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Cluster;

/// <summary>An indexer cluster peer, as the cluster manager sees it (<c>cluster/manager/peers</c>).</summary>
public sealed class ClusterPeer : SplunkContent
{
	/// <summary>The peer's name.</summary>
	[JsonPropertyName("label")]
	public string? Label { get; init; }

	/// <summary>The peer status.</summary>
	[JsonPropertyName("status")]
	public ClusterPeerStatus Status { get; init; }

	/// <summary>The peer's site.</summary>
	[JsonPropertyName("site")]
	public string? Site { get; init; }

	/// <summary>The peer's Splunk version.</summary>
	[JsonPropertyName("splunk_version")]
	public string? SplunkVersion { get; init; }

	/// <summary>The replication host and port the peer advertises.</summary>
	[JsonPropertyName("host_port_pair")]
	public string? HostPortPair { get; init; }

	/// <summary>Whether the peer is in the committed generation and searchable.</summary>
	[JsonPropertyName("is_searchable")]
	public bool? IsSearchable { get; init; }

	/// <summary>Whether the peer has started sending heartbeats.</summary>
	[JsonPropertyName("heartbeat_started")]
	public bool? HeartbeatStarted { get; init; }

	/// <summary>When the manager last received a heartbeat from the peer.</summary>
	[JsonPropertyName("last_heartbeat")]
	[JsonConverter(typeof(EpochSecondsConverter))]
	public DateTimeOffset? LastHeartbeat { get; init; }

	/// <summary>The ID of the bundle the manager has active.</summary>
	[JsonPropertyName("active_bundle_id")]
	public string? ActiveBundleId { get; init; }

	/// <summary>The ID of the bundle the peer uses.</summary>
	[JsonPropertyName("latest_bundle_id")]
	public string? LatestBundleId { get; init; }

	/// <summary>The peer's bundle state.</summary>
	[JsonPropertyName("apply_bundle_status")]
	public ClusterPeerApplyBundleStatus? ApplyBundleStatus { get; init; }

	/// <summary>The first generation the peer recognises.</summary>
	[JsonPropertyName("base_generation_id")]
	public long? BaseGenerationId { get; init; }

	/// <summary>The number of buckets on the peer.</summary>
	[JsonPropertyName("bucket_count")]
	public int? BucketCount { get; init; }

	/// <summary>The number of buckets on the peer, by index.</summary>
	[JsonPropertyName("bucket_count_by_index")]
	public IReadOnlyDictionary<string, int> BucketCountByIndex { get; init; } = new Dictionary<string, int>();

	/// <summary>The number of primary copies the peer holds for its site.</summary>
	[JsonPropertyName("primary_count")]
	public int? PrimaryCount { get; init; }

	/// <summary>The number of primary copies the peer holds for other sites.</summary>
	[JsonPropertyName("primary_count_remote")]
	public int? PrimaryCountRemote { get; init; }

	/// <summary>The number of replications the peer takes part in.</summary>
	[JsonPropertyName("replication_count")]
	public int? ReplicationCount { get; init; }

	/// <summary>The number of jobs the manager has pending on the peer.</summary>
	[JsonPropertyName("pending_job_count")]
	public int? PendingJobCount { get; init; }

	/// <summary>The peer's replication port.</summary>
	[JsonPropertyName("replication_port")]
	public int? ReplicationPort { get; init; }

	/// <summary>Whether the peer replicates over TLS.</summary>
	[JsonPropertyName("replication_use_ssl")]
	public bool? ReplicationUseSsl { get; init; }

	/// <summary>The buckets waiting to be discarded from the peer.</summary>
	[JsonPropertyName("delayed_buckets_to_discard")]
	public IReadOnlyList<string> DelayedBucketsToDiscard { get; init; } = [];

	/// <summary>The buckets that need fixing when the peer goes offline.</summary>
	[JsonPropertyName("fixup_set")]
	public IReadOnlyList<string> FixupSet { get; init; } = [];

	/// <summary>The number of buckets by search state.</summary>
	[JsonPropertyName("search_state_counter")]
	public IReadOnlyDictionary<string, int> SearchStateCounter { get; init; } = new Dictionary<string, int>();

	/// <summary>The number of buckets by bucket status.</summary>
	[JsonPropertyName("status_counter")]
	public IReadOnlyDictionary<string, int> StatusCounter { get; init; } = new Dictionary<string, int>();
}
