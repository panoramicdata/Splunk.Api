using Splunk.Api.Serialization;
using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Search;

/// <summary>
/// A saved search, report or alert (<c>saved/searches</c>). Splunk returns several hundred properties, including the
/// defaults of every alert action (<c>action.&lt;name&gt;.*</c>) and display setting (<c>display.*</c>); the commonly used
/// ones are modelled here and the rest are in <see cref="SplunkContent.AdditionalProperties"/>.
/// </summary>
public sealed partial class SavedSearch : SplunkContent
{
	/// <summary>The search, in SPL.</summary>
	[JsonPropertyName("search")]
	public string? Search { get; init; }

	/// <summary>The search as Splunk runs it (with the implied leading <c>search</c> command resolved).</summary>
	[JsonPropertyName("qualifiedSearch")]
	public string? QualifiedSearch { get; init; }

	/// <summary>The description.</summary>
	[JsonPropertyName("description")]
	public string? Description { get; init; }

	/// <summary>Whether the search runs on a schedule.</summary>
	[JsonPropertyName("is_scheduled")]
	public bool IsScheduled { get; init; }

	/// <summary>The schedule, in cron syntax, for example <c>*/5 * * * *</c>.</summary>
	[JsonPropertyName("cron_schedule")]
	public string? CronSchedule { get; init; }

	/// <summary>When the search next runs, as Splunk formats it (for example <c>2026-10-08 13:50:00 UTC</c>); empty when not scheduled.</summary>
	[JsonPropertyName("next_scheduled_time")]
	public string? NextScheduledTime { get; init; }

	/// <summary>The times the search is scheduled to run, returned by <c>scheduled_times</c>; empty elsewhere.</summary>
	[JsonPropertyName("scheduled_times")]
	[JsonConverter(typeof(EpochSecondsListConverter))]
	public IReadOnlyList<DateTimeOffset> ScheduledTimes { get; init; } = [];

	/// <summary>Whether the search is shown in the UI's lists.</summary>
	[JsonPropertyName("is_visible")]
	public bool IsVisible { get; init; }

	/// <summary>The scheduling window in minutes, or <c>auto</c>.</summary>
	[JsonPropertyName("schedule_window")]
	public string? ScheduleWindow { get; init; }

	/// <summary>The scheduling priority: <c>default</c>, <c>higher</c> or <c>highest</c>.</summary>
	[JsonPropertyName("schedule_priority")]
	public string? SchedulePriority { get; init; }

	/// <summary>Whether a scheduled run that was missed is run in real-time scheduling mode (<c>realtime_schedule</c>).</summary>
	[JsonPropertyName("realtime_schedule")]
	public bool RealtimeSchedule { get; init; }

	/// <summary>The most concurrent instances of the search.</summary>
	[JsonPropertyName("max_concurrent")]
	public int MaxConcurrent { get; init; }

	/// <summary>Whether the search runs when Splunk starts.</summary>
	[JsonPropertyName("run_on_startup")]
	public bool RunOnStartup { get; init; }

	/// <summary>The number of times the search runs before it is disabled; 0 is unlimited.</summary>
	[JsonPropertyName("run_n_times")]
	public int RunNTimes { get; init; }

	/// <summary>The comma-separated alert actions enabled, for example <c>email,webhook</c>.</summary>
	[JsonPropertyName("actions")]
	public string? Actions { get; init; }

	/// <summary>Whether the email action is enabled.</summary>
	[JsonPropertyName("action.email")]
	public bool? ActionEmail { get; init; }

	/// <summary>The email action's recipients, comma-separated.</summary>
	[JsonPropertyName("action.email.to")]
	public string? ActionEmailTo { get; init; }

	/// <summary>Whether search acceleration (auto-summarization) is on.</summary>
	[JsonPropertyName("auto_summarize")]
	public bool AutoSummarize { get; init; }

	/// <summary>The app the UI opens the search in.</summary>
	[JsonPropertyName("request.ui_dispatch_app")]
	public string? RequestUiDispatchApp { get; init; }

	/// <summary>The view the UI opens the search in.</summary>
	[JsonPropertyName("request.ui_dispatch_view")]
	public string? RequestUiDispatchView { get; init; }

	/// <summary>The workload pool the search runs in.</summary>
	[JsonPropertyName("workload_pool")]
	public string? WorkloadPool { get; init; }
}
