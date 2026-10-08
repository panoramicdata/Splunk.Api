using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Search;

/// <summary>
/// Runs a saved search now (<c>POST saved/searches/{name}/dispatch</c>). Search arguments (<c>args.&lt;name&gt;</c>, which
/// fill <c>$name$</c> tokens) and other <c>dispatch.*</c> overrides go in
/// <see cref="SplunkFormRequest.AdditionalParameters"/>.
/// </summary>
public sealed class SavedSearchDispatchRequest : SplunkFormRequest
{
	/// <summary>The time the run treats as now (<c>dispatch.now</c>).</summary>
	[JsonPropertyName("dispatch.now")]
	public string? DispatchNow { get; init; }

	/// <summary>Overrides the run's earliest time (<c>dispatch.earliest_time</c>).</summary>
	[JsonPropertyName("dispatch.earliest_time")]
	public string? DispatchEarliestTime { get; init; }

	/// <summary>Overrides the run's latest time (<c>dispatch.latest_time</c>).</summary>
	[JsonPropertyName("dispatch.latest_time")]
	public string? DispatchLatestTime { get; init; }

	/// <summary>Whether the run triggers the search's alert actions (<c>trigger_actions</c>).</summary>
	[JsonPropertyName("trigger_actions")]
	public bool? TriggerActions { get; init; }

	/// <summary>Whether to start a new run even if one is already running (<c>force_dispatch</c>).</summary>
	[JsonPropertyName("force_dispatch")]
	public bool? ForceDispatch { get; init; }

	/// <summary>Whose permissions the run uses: <c>owner</c> or <c>user</c> (<c>dispatchAs</c>).</summary>
	[JsonPropertyName("dispatchAs")]
	public string? DispatchAs { get; init; }

	/// <summary>For a real-time search, replays indexed data at this speed instead (<c>replay_speed</c>).</summary>
	[JsonPropertyName("replay_speed")]
	public double? ReplaySpeed { get; init; }

	/// <summary>The earliest time of the replay (<c>replay_et</c>).</summary>
	[JsonPropertyName("replay_et")]
	public string? ReplayEarliestTime { get; init; }

	/// <summary>The latest time of the replay (<c>replay_lt</c>).</summary>
	[JsonPropertyName("replay_lt")]
	public string? ReplayLatestTime { get; init; }
}
