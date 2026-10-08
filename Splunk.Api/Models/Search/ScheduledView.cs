using Splunk.Api.Serialization;
using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Search;

/// <summary>
/// A dashboard scheduled for PDF delivery by email (<c>scheduled/views</c>). Its name is
/// <c>_ScheduledView__&lt;view name&gt;</c>. The email action's other settings (<c>action.email.*</c>) are in
/// <see cref="SplunkContent.AdditionalProperties"/>.
/// </summary>
public sealed class ScheduledView : SplunkContent
{
	/// <summary>The description.</summary>
	[JsonPropertyName("description")]
	public string? Description { get; init; }

	/// <summary>Whether the delivery is scheduled.</summary>
	[JsonPropertyName("is_scheduled")]
	public bool IsScheduled { get; init; }

	/// <summary>The schedule, in cron syntax.</summary>
	[JsonPropertyName("cron_schedule")]
	public string? CronSchedule { get; init; }

	/// <summary>When the delivery next runs, as Splunk formats it (for example <c>2026-10-09 03:00:00 UTC</c>).</summary>
	[JsonPropertyName("next_scheduled_time")]
	public string? NextScheduledTime { get; init; }

	/// <summary>The times the delivery is scheduled to run, returned by <c>scheduled_times</c>; empty elsewhere.</summary>
	[JsonPropertyName("scheduled_times")]
	[JsonConverter(typeof(EpochSecondsListConverter))]
	public IReadOnlyList<DateTimeOffset> ScheduledTimes { get; init; } = [];

	/// <summary>The scheduling window in minutes, or <c>auto</c>.</summary>
	[JsonPropertyName("schedule_window")]
	public string? ScheduleWindow { get; init; }

	/// <summary>The scheduling priority.</summary>
	[JsonPropertyName("schedule_priority")]
	public string? SchedulePriority { get; init; }

	/// <summary>Whether the email action is enabled.</summary>
	[JsonPropertyName("action.email")]
	public bool ActionEmail { get; init; }

	/// <summary>The recipients, comma-separated.</summary>
	[JsonPropertyName("action.email.to")]
	public string? ActionEmailTo { get; init; }

	/// <summary>The view rendered to PDF.</summary>
	[JsonPropertyName("action.email.pdfview")]
	public string? ActionEmailPdfView { get; init; }

	/// <summary>The email subject for the view.</summary>
	[JsonPropertyName("action.email.subject.view")]
	public string? ActionEmailSubjectView { get; init; }
}
