using Splunk.Api.Serialization;
using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Cluster;

/// <summary>The state of this peer node (<c>cluster/peer/info</c>).</summary>
public sealed class ClusterPeerInfo : SplunkContent
{
	/// <summary>The peer status: <c>Up</c>, <c>Down</c>, <c>Pending</c>, <c>Detention</c>, <c>Restarting</c>, <c>DecommAwaitingPeer</c>, <c>DecommFixingBuckets</c> or <c>Decommissioned</c>.</summary>
	[JsonPropertyName("status")]
	public string? Status { get; init; }

	/// <summary>Whether the peer is registered with the manager.</summary>
	[JsonPropertyName("is_registered")]
	public bool? IsRegistered { get; init; }

	/// <summary>When the peer last tried to contact the manager.</summary>
	[JsonPropertyName("last_heartbeat_attempt")]
	[JsonConverter(typeof(EpochSecondsConverter))]
	public DateTimeOffset? LastHeartbeatAttempt { get; init; }

	/// <summary>The first generation the peer recognises.</summary>
	[JsonPropertyName("base_generation_id")]
	public long? BaseGenerationId { get; init; }

	/// <summary>The bundle the peer uses.</summary>
	[JsonPropertyName("active_bundle")]
	public ClusterBundleInfo? ActiveBundle { get; init; }

	/// <summary>The latest bundle the peer downloaded.</summary>
	[JsonPropertyName("latest_bundle")]
	public ClusterBundleInfo? LatestBundle { get; init; }

	/// <summary>The bundles that failed validation on the peer.</summary>
	[JsonPropertyName("invalid_bundle_ids")]
	public IReadOnlyList<string> InvalidBundleIds { get; init; } = [];

	/// <summary>Whether the peer needs a restart to apply its cluster configuration, for example <c>NoRestart</c>.</summary>
	[JsonPropertyName("restart_state")]
	public string? RestartState { get; init; }
}
