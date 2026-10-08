using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Introspection;

/// <summary>
/// A health report setting (<c>server/health-config</c>): a <c>feature:&lt;name&gt;</c>, an <c>alert_action:&lt;name&gt;</c>
/// or <c>distributed_health_reporter</c> stanza of health.conf. Indicator thresholds such as
/// <c>indicator:&lt;name&gt;:yellow</c> are in <see cref="SplunkContent.AdditionalProperties"/>.
/// </summary>
public sealed class HealthConfigStanza : SplunkContent
{
	/// <summary>The feature's display name (<c>display_name</c>).</summary>
	[JsonPropertyName("display_name")]
	public string? DisplayName { get; init; }

	/// <summary>What the feature monitors (<c>friendly_description</c>).</summary>
	[JsonPropertyName("friendly_description")]
	public string? FriendlyDescription { get; init; }

	/// <summary>Whether alerting is disabled for the feature (<c>alert.disabled</c>).</summary>
	[JsonPropertyName("alert.disabled")]
	public bool? AlertDisabled { get; init; }

	/// <summary>How long in seconds a status must last before an alert (<c>alert.min_duration_sec</c>).</summary>
	[JsonPropertyName("alert.min_duration_sec")]
	public int? AlertMinDurationSec { get; init; }

	/// <summary>The color that triggers an alert, <c>yellow</c> or <c>red</c> (<c>alert.threshold_color</c>).</summary>
	[JsonPropertyName("alert.threshold_color")]
	public string? AlertThresholdColor { get; init; }

	/// <summary>Whether the feature is left out of the distributed health report (<c>distributed_disabled</c>).</summary>
	[JsonPropertyName("distributed_disabled")]
	public bool? DistributedDisabled { get; init; }

	/// <summary>When alert snoozing ends, in epoch seconds (<c>snooze_end_time</c>).</summary>
	[JsonPropertyName("snooze_end_time")]
	public long? SnoozeEndTime { get; init; }

	/// <summary>An alert action's recipients (<c>action.to</c>).</summary>
	[JsonPropertyName("action.to")]
	public string? ActionTo { get; init; }

	/// <summary>An alert action's copy recipients (<c>action.cc</c>).</summary>
	[JsonPropertyName("action.cc")]
	public string? ActionCc { get; init; }

	/// <summary>An alert action's blind copy recipients (<c>action.bcc</c>).</summary>
	[JsonPropertyName("action.bcc")]
	public string? ActionBcc { get; init; }
}
