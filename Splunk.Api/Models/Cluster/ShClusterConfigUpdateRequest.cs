using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Cluster;

/// <summary>Configures search head cluster members (<c>POST shcluster/config/config</c>): the rolling restart mode, or manual detention of a member.</summary>
public sealed class ShClusterConfigUpdateRequest : SplunkFormRequest
{
	/// <summary>The rolling restart mode.</summary>
	[JsonPropertyName("rolling_restart")]
	public ShClusterRollingRestartMode? RollingRestart { get; init; }

	/// <summary>How long, in seconds, a member waits for running searches before it restarts (default 180).</summary>
	[JsonPropertyName("decommission_search_jobs_wait_secs")]
	public int? DecommissionSearchJobsWaitSeconds { get; init; }

	/// <summary>The manual detention state for <see cref="TargetUri"/> (<c>off</c> or <c>on</c>).</summary>
	[JsonPropertyName("manual_detention")]
	public ManualDetentionMode? ManualDetention { get; init; }

	/// <summary>The member to put in or take out of manual detention.</summary>
	[JsonPropertyName("target_uri")]
	public string? TargetUri { get; init; }
}
