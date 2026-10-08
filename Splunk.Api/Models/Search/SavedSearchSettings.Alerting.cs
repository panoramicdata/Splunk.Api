using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Search;

public abstract partial class SavedSearchSettings
{
	/// <summary>The alert actions to run, comma-separated, for example <c>email,webhook</c>; empty for none (<c>actions</c>).</summary>
	[JsonPropertyName("actions")]
	public string? Actions { get; init; }

	/// <summary>The email action's recipients, comma-separated (<c>action.email.to</c>).</summary>
	[JsonPropertyName("action.email.to")]
	public string? ActionEmailTo { get; init; }

	/// <summary>The email action's subject (<c>action.email.subject</c>).</summary>
	[JsonPropertyName("action.email.subject")]
	public string? ActionEmailSubject { get; init; }

	/// <summary>
	/// The alert condition type (<c>alert_type</c>): <c>always</c>, <c>custom</c>, <c>number of events</c>,
	/// <c>number of hosts</c> or <c>number of sources</c>.
	/// </summary>
	[JsonPropertyName("alert_type")]
	public string? AlertType { get; init; }

	/// <summary>
	/// The comparison for a count condition (<c>alert_comparator</c>): <c>greater than</c>, <c>less than</c>,
	/// <c>equal to</c>, <c>rises by</c>, <c>drops by</c>, <c>rises by perc</c> or <c>drops by perc</c>.
	/// </summary>
	[JsonPropertyName("alert_comparator")]
	public string? AlertComparator { get; init; }

	/// <summary>The value a count condition compares against (<c>alert_threshold</c>), a number or a percentage.</summary>
	[JsonPropertyName("alert_threshold")]
	public string? AlertThreshold { get; init; }

	/// <summary>The search that decides a <c>custom</c> condition (<c>alert_condition</c>).</summary>
	[JsonPropertyName("alert_condition")]
	public string? AlertCondition { get; init; }

	/// <summary>The alert severity, 1 (debug) to 6 (fatal) (<c>alert.severity</c>).</summary>
	[JsonPropertyName("alert.severity")]
	public int? AlertSeverity { get; init; }

	/// <summary>Whether one alert is raised for all results rather than one per result (<c>alert.digest_mode</c>).</summary>
	[JsonPropertyName("alert.digest_mode")]
	public bool? AlertDigestMode { get; init; }

	/// <summary>How long triggered alerts are kept, for example <c>24h</c> (<c>alert.expires</c>).</summary>
	[JsonPropertyName("alert.expires")]
	public string? AlertExpires { get; init; }

	/// <summary>Whether alert throttling is on (<c>alert.suppress</c>).</summary>
	[JsonPropertyName("alert.suppress")]
	public bool? AlertSuppress { get; init; }

	/// <summary>How long to throttle after a trigger, for example <c>1h</c> (<c>alert.suppress.period</c>).</summary>
	[JsonPropertyName("alert.suppress.period")]
	public string? AlertSuppressPeriod { get; init; }

	/// <summary>The fields whose values throttle separately, comma-separated (<c>alert.suppress.fields</c>).</summary>
	[JsonPropertyName("alert.suppress.fields")]
	public string? AlertSuppressFields { get; init; }

	/// <summary>Whether triggered alerts are listed in <c>alerts/fired_alerts</c> (<c>alert.track</c>).</summary>
	[JsonPropertyName("alert.track")]
	public bool? AlertTrack { get; init; }
}
