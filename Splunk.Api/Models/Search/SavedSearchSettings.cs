using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Search;

/// <summary>
/// The commonly used settings of a saved search, for <see cref="SavedSearchCreateRequest"/> and
/// <see cref="SavedSearchUpdateRequest"/>. Leave a property <see langword="null"/> to keep Splunk's value. Splunk accepts
/// several hundred more (every <c>action.&lt;name&gt;.*</c>, <c>dispatch.*</c>, <c>display.*</c> and
/// <c>auto_summarize.*</c> setting); put those in <see cref="SplunkFormRequest.AdditionalParameters"/>, for example
/// <c>["action.webhook.param.url"] = "https://example.com/hook"</c>.
/// </summary>
public abstract partial class SavedSearchSettings : SplunkFormRequest
{
	/// <summary>The description (<c>description</c>).</summary>
	[JsonPropertyName("description")]
	public string? Description { get; init; }

	/// <summary>Whether the search runs on a schedule (<c>is_scheduled</c>); needs <see cref="CronSchedule"/>.</summary>
	[JsonPropertyName("is_scheduled")]
	public bool? IsScheduled { get; init; }

	/// <summary>The schedule in cron syntax (<c>cron_schedule</c>), for example <c>*/5 * * * *</c>.</summary>
	[JsonPropertyName("cron_schedule")]
	public string? CronSchedule { get; init; }

	/// <summary>Whether the search is disabled (<c>disabled</c>).</summary>
	[JsonPropertyName("disabled")]
	public bool? Disabled { get; init; }

	/// <summary>Whether the search is shown in the UI's lists (<c>is_visible</c>).</summary>
	[JsonPropertyName("is_visible")]
	public bool? IsVisible { get; init; }

	/// <summary>The scheduling window in minutes, or <c>auto</c> (<c>schedule_window</c>).</summary>
	[JsonPropertyName("schedule_window")]
	public string? ScheduleWindow { get; init; }

	/// <summary>The scheduling priority: <c>default</c>, <c>higher</c> or <c>highest</c> (<c>schedule_priority</c>).</summary>
	[JsonPropertyName("schedule_priority")]
	public string? SchedulePriority { get; init; }

	/// <summary>Whether missed runs use real-time scheduling (<c>realtime_schedule</c>).</summary>
	[JsonPropertyName("realtime_schedule")]
	public bool? RealtimeSchedule { get; init; }

	/// <summary>The most concurrent instances (<c>max_concurrent</c>).</summary>
	[JsonPropertyName("max_concurrent")]
	public int? MaxConcurrent { get; init; }

	/// <summary>Whether the search runs when Splunk starts (<c>run_on_startup</c>).</summary>
	[JsonPropertyName("run_on_startup")]
	public bool? RunOnStartup { get; init; }

	/// <summary>The earliest time of each run (<c>dispatch.earliest_time</c>), for example <c>-15m</c>.</summary>
	[JsonPropertyName("dispatch.earliest_time")]
	public string? DispatchEarliestTime { get; init; }

	/// <summary>The latest time of each run (<c>dispatch.latest_time</c>), for example <c>now</c>.</summary>
	[JsonPropertyName("dispatch.latest_time")]
	public string? DispatchLatestTime { get; init; }

	/// <summary>How long each run's job is kept (<c>dispatch.ttl</c>): seconds, or periods such as <c>2p</c>.</summary>
	[JsonPropertyName("dispatch.ttl")]
	public string? DispatchTtl { get; init; }

	/// <summary>The most results each run keeps (<c>dispatch.max_count</c>).</summary>
	[JsonPropertyName("dispatch.max_count")]
	public int? DispatchMaxCount { get; init; }

	/// <summary>The seconds a run may take before it is finalized (<c>dispatch.max_time</c>).</summary>
	[JsonPropertyName("dispatch.max_time")]
	public int? DispatchMaxTime { get; init; }

	/// <summary>Whose permissions a run uses: <c>owner</c> or <c>user</c> (<c>dispatchAs</c>).</summary>
	[JsonPropertyName("dispatchAs")]
	public string? DispatchAs { get; init; }

	/// <summary>The app the UI opens the search in (<c>request.ui_dispatch_app</c>).</summary>
	[JsonPropertyName("request.ui_dispatch_app")]
	public string? RequestUiDispatchApp { get; init; }

	/// <summary>The view the UI opens the search in (<c>request.ui_dispatch_view</c>).</summary>
	[JsonPropertyName("request.ui_dispatch_view")]
	public string? RequestUiDispatchView { get; init; }

	/// <summary>The workload pool the search runs in (<c>workload_pool</c>).</summary>
	[JsonPropertyName("workload_pool")]
	public string? WorkloadPool { get; init; }
}
