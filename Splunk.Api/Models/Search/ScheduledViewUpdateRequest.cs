using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Search;

/// <summary>
/// Schedules a view's PDF delivery, or changes it (<c>POST scheduled/views/{name}</c>). Other email settings
/// (<c>action.email.*</c>) go in <see cref="SplunkFormRequest.AdditionalParameters"/>.
/// </summary>
/// <remarks>
/// Splunk 10.6 requires <see cref="CronSchedule"/> and <see cref="IsScheduled"/> on every update. The reference also lists
/// <c>action.email.to</c> as required; a live server needs it only while the email action is enabled and no recipients
/// are saved yet (<c>action.email.to is required if email action is enabled</c>), so it is optional here.
/// </remarks>
public sealed class ScheduledViewUpdateRequest : SplunkFormRequest
{
	/// <summary>The schedule in cron syntax (<c>cron_schedule</c>).</summary>
	[JsonPropertyName("cron_schedule")]
	public required string CronSchedule { get; init; }

	/// <summary>Whether the delivery is scheduled (<c>is_scheduled</c>).</summary>
	[JsonPropertyName("is_scheduled")]
	public required bool IsScheduled { get; init; }

	/// <summary>The recipients, comma-separated (<c>action.email.to</c>).</summary>
	[JsonPropertyName("action.email.to")]
	public string? ActionEmailTo { get; init; }

	/// <summary>The description (<c>description</c>).</summary>
	[JsonPropertyName("description")]
	public string? Description { get; init; }

	/// <summary>Whether the schedule is disabled (<c>disabled</c>).</summary>
	[JsonPropertyName("disabled")]
	public bool? Disabled { get; init; }

	/// <summary>The time of the next delivery (<c>next_scheduled_time</c>).</summary>
	[JsonPropertyName("next_scheduled_time")]
	public string? NextScheduledTime { get; init; }
}
