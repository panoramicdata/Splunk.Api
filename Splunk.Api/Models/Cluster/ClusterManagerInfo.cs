using Splunk.Api.Serialization;
using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Cluster;

/// <summary>The state of the cluster manager (<c>cluster/manager/info</c>).</summary>
public sealed class ClusterManagerInfo : SplunkContent
{
	/// <summary>The manager's name.</summary>
	[JsonPropertyName("label")]
	public string? Label { get; init; }

	/// <summary>When the manager started.</summary>
	[JsonPropertyName("start_time")]
	[JsonConverter(typeof(EpochSecondsConverter))]
	public DateTimeOffset? StartTime { get; init; }

	/// <summary>The active configuration bundle.</summary>
	[JsonPropertyName("active_bundle")]
	public ClusterBundleInfo? ActiveBundle { get; init; }

	/// <summary>The latest bundle in manager-apps; it differs from the active one while a push is pending.</summary>
	[JsonPropertyName("latest_bundle")]
	public ClusterBundleInfo? LatestBundle { get; init; }

	/// <summary>The previously active bundle, for rollback.</summary>
	[JsonPropertyName("previous_active_bundle")]
	public ClusterBundleInfo? PreviousActiveBundle { get; init; }

	/// <summary>The last bundle that passed validation.</summary>
	[JsonPropertyName("last_validated_bundle")]
	public ClusterBundleInfo? LastValidatedBundle { get; init; }

	/// <summary>The last dry-run bundle.</summary>
	[JsonPropertyName("last_dry_run_bundle")]
	public ClusterBundleInfo? LastDryRunBundle { get; init; }

	/// <summary>The progress of bundle operations.</summary>
	[JsonPropertyName("apply_bundle_status")]
	public ClusterApplyBundleStatus? ApplyBundleStatus { get; init; }

	/// <summary>Whether the last validation found that applying the bundle needs a restart.</summary>
	[JsonPropertyName("last_check_restart_bundle_result")]
	public bool? LastCheckRestartBundleResult { get; init; }

	/// <summary>Whether primaries are backed up and restored during maintenance.</summary>
	[JsonPropertyName("backup_and_restore_primaries")]
	public bool? BackupAndRestorePrimaries { get; init; }

	/// <summary>The state of the primary bucket backup.</summary>
	[JsonPropertyName("primaries_backup_status")]
	public string? PrimariesBackupStatus { get; init; }

	/// <summary>Whether a controlled (shutdown-based) rolling restart is in progress.</summary>
	[JsonPropertyName("controlled_rolling_restart_flag")]
	public bool? ControlledRollingRestartFlag { get; init; }

	/// <summary>Whether the cluster is ready for indexing.</summary>
	[JsonPropertyName("indexing_ready_flag")]
	public bool? IndexingReadyFlag { get; init; }

	/// <summary>Whether the cluster is initialized.</summary>
	[JsonPropertyName("initialized_flag")]
	public bool? InitializedFlag { get; init; }

	/// <summary>Whether the cluster is in maintenance mode.</summary>
	[JsonPropertyName("maintenance_mode")]
	public bool? MaintenanceMode { get; init; }

	/// <summary>Whether the cluster is multisite.</summary>
	[JsonPropertyName("multisite")]
	public bool? Multisite { get; init; }

	/// <summary>Whether the manager is in its quiet period.</summary>
	[JsonPropertyName("quiet_period_flag")]
	public bool? QuietPeriodFlag { get; init; }

	/// <summary>Whether a rolling restart is in progress.</summary>
	[JsonPropertyName("rolling_restart_flag")]
	public bool? RollingRestartFlag { get; init; }

	/// <summary>Whether a rolling restart or upgrade is in progress.</summary>
	[JsonPropertyName("rolling_restart_or_upgrade")]
	public bool? RollingRestartOrUpgrade { get; init; }

	/// <summary>Whether the manager is ready to provide services.</summary>
	[JsonPropertyName("service_ready_flag")]
	public bool? ServiceReadyFlag { get; init; }

	/// <summary>Whether summary replication is on.</summary>
	[JsonPropertyName("summary_replication")]
	public bool? SummaryReplication { get; init; }
}
