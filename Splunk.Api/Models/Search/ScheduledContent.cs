using Splunk.Api.Serialization;
using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Search;

/// <summary>
/// The schedule Splunk reports on a scheduled object: a saved search (<see cref="SavedSearch"/>) or a dashboard scheduled
/// for PDF delivery (<see cref="ScheduledView"/>).
/// </summary>
public abstract class ScheduledContent : SplunkContent
{
	/// <summary>The description.</summary>
	[JsonPropertyName("description")]
	public string? Description { get; init; }

	/// <summary>Whether the object runs on a schedule.</summary>
	[JsonPropertyName("is_scheduled")]
	public bool IsScheduled { get; init; }

	/// <summary>The schedule, in cron syntax, for example <c>*/5 * * * *</c>.</summary>
	[JsonPropertyName("cron_schedule")]
	public string? CronSchedule { get; init; }

	/// <summary>When the object next runs, as Splunk formats it (for example <c>2026-10-08 13:50:00 UTC</c>); empty when not scheduled.</summary>
	[JsonPropertyName("next_scheduled_time")]
	public string? NextScheduledTime { get; init; }

	/// <summary>The times the object is scheduled to run, returned by <c>scheduled_times</c>; empty elsewhere.</summary>
	[JsonPropertyName("scheduled_times")]
	[JsonConverter(typeof(EpochSecondsListConverter))]
	public IReadOnlyList<DateTimeOffset> ScheduledTimes { get; init; } = [];

	/// <summary>The scheduling window in minutes, or <c>auto</c>.</summary>
	[JsonPropertyName("schedule_window")]
	public string? ScheduleWindow { get; init; }

	/// <summary>The scheduling priority: <c>default</c>, <c>higher</c> or <c>highest</c>.</summary>
	[JsonPropertyName("schedule_priority")]
	public string? SchedulePriority { get; init; }
}
