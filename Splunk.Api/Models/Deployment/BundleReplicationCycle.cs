using Splunk.Api.Serialization;
using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Deployment;

/// <summary>A knowledge bundle replication cycle of a search head (<c>search/distributed/bundle/replication/cycles</c>).</summary>
public sealed class BundleReplicationCycle : SplunkContent
{
	/// <summary>The bundle, as <c>host-creation_time</c>.</summary>
	[JsonPropertyName("bundle_id")]
	public string? BundleId { get; init; }

	/// <summary>The path of the active bundle.</summary>
	[JsonPropertyName("current_bundle")]
	public string? CurrentBundle { get; init; }

	/// <summary>When the cycle started.</summary>
	[JsonPropertyName("current_repl_start_time")]
	[JsonConverter(typeof(EpochSecondsConverter))]
	public DateTimeOffset? CurrentReplicationStartTime { get; init; }

	/// <summary>The cycle identifier.</summary>
	[JsonPropertyName("cycle_id")]
	public string? CycleId { get; init; }

	/// <summary>The path of the delta bundle, if one was made.</summary>
	[JsonPropertyName("delta_path")]
	public string? DeltaPath { get; init; }

	/// <summary>Whether the cycle is still running.</summary>
	[JsonPropertyName("is_repl_in_progress")]
	public bool? IsReplicationInProgress { get; init; }

	/// <summary>The state of each search peer, by peer URI.</summary>
	[JsonPropertyName("peers_status")]
	public IReadOnlyDictionary<string, BundleReplicationPeerStatus> PeersStatus { get; init; } = new Dictionary<string, BundleReplicationPeerStatus>();

	/// <summary>The replication policy.</summary>
	[JsonPropertyName("replicationPolicy")]
	public string? ReplicationPolicy { get; init; }
}
