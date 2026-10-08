using Splunk.Api.Serialization;
using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Cluster;

/// <summary>The state of this search head cluster member (<c>shcluster/member/info</c>).</summary>
public sealed class ShClusterMemberInfo : SplunkContent
{
	/// <summary>The member status.</summary>
	[JsonPropertyName("status")]
	public ClusterPeerStatus Status { get; init; }

	/// <summary>Whether the member is registered with the captain.</summary>
	[JsonPropertyName("is_registered")]
	public bool? IsRegistered { get; init; }

	/// <summary>When the member last tried to contact the captain.</summary>
	[JsonPropertyName("last_heartbeat_attempt")]
	[JsonConverter(typeof(EpochSecondsConverter))]
	public DateTimeOffset? LastHeartbeatAttempt { get; init; }

	/// <summary>Whether the member runs no scheduled searches.</summary>
	[JsonPropertyName("adhoc_searchhead")]
	public bool? AdhocSearchHead { get; init; }

	/// <summary>Whether the cluster is in maintenance mode.</summary>
	[JsonPropertyName("maintenance_mode")]
	public bool? MaintenanceMode { get; init; }

	/// <summary>Whether the member needs a restart to apply its cluster configuration, for example <c>NoRestart</c>.</summary>
	[JsonPropertyName("restart_state")]
	public string? RestartState { get; init; }

	/// <summary>The number of historical searches running.</summary>
	[JsonPropertyName("active_historical_search_count")]
	public int? ActiveHistoricalSearchCount { get; init; }

	/// <summary>The number of real-time searches running.</summary>
	[JsonPropertyName("active_realtime_search_count")]
	public int? ActiveRealtimeSearchCount { get; init; }

	/// <summary>The number of scheduled searches run in the last minute.</summary>
	[JsonPropertyName("peer_load_stats_gla_1m")]
	public int? ScheduledSearchesLast1Minute { get; init; }

	/// <summary>The number of scheduled searches run in the last five minutes.</summary>
	[JsonPropertyName("peer_load_stats_gla_5m")]
	public int? ScheduledSearchesLast5Minutes { get; init; }

	/// <summary>The number of scheduled searches run in the last 15 minutes.</summary>
	[JsonPropertyName("peer_load_stats_gla_15m")]
	public int? ScheduledSearchesLast15Minutes { get; init; }

	/// <summary>Load statistics: the longest run time.</summary>
	[JsonPropertyName("peer_load_stats_max_runtime")]
	public long? MaxRuntime { get; init; }

	/// <summary>Load statistics: the total run time.</summary>
	[JsonPropertyName("peer_load_stats_total_runtime")]
	public long? TotalRuntime { get; init; }

	/// <summary>Load statistics: the number of report acceleration searches.</summary>
	[JsonPropertyName("peer_load_stats_num_autosummary")]
	public int? AutosummarySearchCount { get; init; }

	/// <summary>Load statistics: the number of historical searches.</summary>
	[JsonPropertyName("peer_load_stats_num_historical")]
	public int? HistoricalSearchCount { get; init; }

	/// <summary>Load statistics: the number of real-time searches.</summary>
	[JsonPropertyName("peer_load_stats_num_realtime")]
	public int? RealtimeSearchCount { get; init; }

	/// <summary>Load statistics: the number of running searches.</summary>
	[JsonPropertyName("peer_load_stats_num_running")]
	public int? RunningSearchCount { get; init; }
}
