using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Cluster;

/// <summary>Options of a search head cluster rolling restart (<c>POST shcluster/captain/control/default/restart</c>).</summary>
public sealed class ShClusterRestartRequest : SplunkFormRequest
{
	/// <summary>Whether to keep searches available during the restart.</summary>
	[JsonPropertyName("searchable")]
	public bool? Searchable { get; init; }

	/// <summary>Whether to restart despite failed health checks (searchable restart only).</summary>
	[JsonPropertyName("force")]
	public bool? Force { get; init; }

	/// <summary>How long, in seconds, a searchable restart waits for running searches (default 180).</summary>
	[JsonPropertyName("decommission_search_jobs_wait_secs")]
	public int? DecommissionSearchJobsWaitSeconds { get; init; }
}
