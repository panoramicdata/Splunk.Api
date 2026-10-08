using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Cluster;

/// <summary>The rolling restart status of the cluster (<c>cluster/manager/status</c>).</summary>
public sealed class ClusterManagerStatus : SplunkContent
{
	/// <summary>Whether a rolling restart is in progress.</summary>
	[JsonPropertyName("rolling_restart_flag")]
	public bool? RollingRestartFlag { get; init; }

	/// <summary>Whether a rolling restart or upgrade is in progress.</summary>
	[JsonPropertyName("rolling_restart_or_upgrade")]
	public bool? RollingRestartOrUpgrade { get; init; }

	/// <summary>Whether a searchable rolling restart or upgrade is in progress.</summary>
	[JsonPropertyName("searchable_rolling")]
	public bool? SearchableRolling { get; init; }

	/// <summary>Whether the cluster is in maintenance mode.</summary>
	[JsonPropertyName("maintenance_mode")]
	public bool? MaintenanceMode { get; init; }

	/// <summary>Whether the cluster is multisite.</summary>
	[JsonPropertyName("multisite")]
	public bool? Multisite { get; init; }

	/// <summary>Whether the cluster is ready.</summary>
	[JsonPropertyName("service_ready_flag")]
	public bool? ServiceReadyFlag { get; init; }

	/// <summary>How long, in seconds, the manager waits for a decommissioning peer during a <c>searchable_force</c> restart.</summary>
	[JsonPropertyName("decommission_force_timeout")]
	public int? DecommissionForceTimeout { get; init; }

	/// <summary>How long, in seconds, the manager waits for a peer to rejoin during a <c>searchable_force</c> restart; 0 waits forever.</summary>
	[JsonPropertyName("restart_inactivity_timeout")]
	public int? RestartInactivityTimeout { get; init; }

	/// <summary>Messages from the manager.</summary>
	[JsonPropertyName("messages")]
	public string? Messages { get; init; }

	/// <summary>The peers, by GUID.</summary>
	[JsonPropertyName("peers")]
	public IReadOnlyDictionary<string, ClusterStatusPeer> Peers { get; init; } = new Dictionary<string, ClusterStatusPeer>();

	/// <summary>The restart progress.</summary>
	[JsonPropertyName("restart_progress")]
	public ClusterRestartProgress? RestartProgress { get; init; }
}
