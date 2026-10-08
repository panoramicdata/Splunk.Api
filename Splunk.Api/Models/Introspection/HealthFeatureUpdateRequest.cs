using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Introspection;

/// <summary>
/// Changes a feature's health report settings (<c>POST server/health-config/feature:{name}</c>). Indicator-level settings
/// (<c>alert:&lt;indicator&gt;.disabled</c>, <c>indicator:&lt;indicator&gt;:yellow</c> and so on) go in
/// <see cref="SplunkFormRequest.AdditionalParameters"/>.
/// </summary>
public sealed class HealthFeatureUpdateRequest : SplunkFormRequest
{
	/// <summary>Whether alerting is disabled for the feature (<c>alert.disabled</c>).</summary>
	[JsonPropertyName("alert.disabled")]
	public bool? AlertDisabled { get; init; }

	/// <summary>How long in seconds a status must last before an alert (<c>alert.min_duration_sec</c>).</summary>
	[JsonPropertyName("alert.min_duration_sec")]
	public int? AlertMinDurationSec { get; init; }

	/// <summary>The color that triggers an alert, <c>yellow</c> or <c>red</c> (<c>alert.threshold_color</c>).</summary>
	[JsonPropertyName("alert.threshold_color")]
	public string? AlertThresholdColor { get; init; }

	/// <summary>Whether the feature's health reporting is disabled (<c>disabled</c>).</summary>
	[JsonPropertyName("disabled")]
	public bool? Disabled { get; init; }

	/// <summary>Whether the feature is left out of the distributed health report (<c>distributed_disabled</c>).</summary>
	[JsonPropertyName("distributed_disabled")]
	public bool? DistributedDisabled { get; init; }

	/// <summary>Snoozes alerts until this time, in epoch seconds (<c>snooze_end_time</c>).</summary>
	[JsonPropertyName("snooze_end_time")]
	public long? SnoozeEndTime { get; init; }
}
