using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Search;

public sealed partial class SavedSearch
{
	/// <summary>The earliest time of each run (<c>dispatch.earliest_time</c>), for example <c>-15m</c>.</summary>
	[JsonPropertyName("dispatch.earliest_time")]
	public string? DispatchEarliestTime { get; init; }

	/// <summary>The latest time of each run (<c>dispatch.latest_time</c>), for example <c>now</c>.</summary>
	[JsonPropertyName("dispatch.latest_time")]
	public string? DispatchLatestTime { get; init; }

	/// <summary>How long each run's job is kept (<c>dispatch.ttl</c>): seconds, or a multiple of the period such as <c>2p</c>.</summary>
	[JsonPropertyName("dispatch.ttl")]
	public string? DispatchTtl { get; init; }

	/// <summary>The most results each run keeps (<c>dispatch.max_count</c>).</summary>
	[JsonPropertyName("dispatch.max_count")]
	public int DispatchMaxCount { get; init; }

	/// <summary>The seconds a run may take before it is finalized (<c>dispatch.max_time</c>); 0 is unlimited.</summary>
	[JsonPropertyName("dispatch.max_time")]
	public int DispatchMaxTime { get; init; }

	/// <summary>The timeline buckets each run keeps (<c>dispatch.buckets</c>).</summary>
	[JsonPropertyName("dispatch.buckets")]
	public int DispatchBuckets { get; init; }

	/// <summary>Whether lookups are applied (<c>dispatch.lookups</c>).</summary>
	[JsonPropertyName("dispatch.lookups")]
	public bool DispatchLookups { get; init; }

	/// <summary>Whose permissions a run uses: <c>owner</c> or <c>user</c> (<c>dispatchAs</c>).</summary>
	[JsonPropertyName("dispatchAs")]
	public string? DispatchAs { get; init; }

	/// <summary>The alert condition type, for example <c>always</c>, <c>custom</c> or <c>number of events</c> (<c>alert_type</c>).</summary>
	[JsonPropertyName("alert_type")]
	public string? AlertType { get; init; }

	/// <summary>The comparison for a count condition, for example <c>greater than</c> (<c>alert_comparator</c>).</summary>
	[JsonPropertyName("alert_comparator")]
	public string? AlertComparator { get; init; }

	/// <summary>The value a count condition compares against (<c>alert_threshold</c>).</summary>
	[JsonPropertyName("alert_threshold")]
	public string? AlertThreshold { get; init; }

	/// <summary>The search that decides a <c>custom</c> alert condition (<c>alert_condition</c>).</summary>
	[JsonPropertyName("alert_condition")]
	public string? AlertCondition { get; init; }

	/// <summary>The alert severity, 1 (debug) to 6 (fatal) (<c>alert.severity</c>).</summary>
	[JsonPropertyName("alert.severity")]
	public int AlertSeverity { get; init; }

	/// <summary>Whether a trigger raises one alert for all results (<see langword="true"/>) or one per result (<c>alert.digest_mode</c>).</summary>
	[JsonPropertyName("alert.digest_mode")]
	public bool AlertDigestMode { get; init; }

	/// <summary>How long triggered alerts are kept, for example <c>24h</c> (<c>alert.expires</c>).</summary>
	[JsonPropertyName("alert.expires")]
	public string? AlertExpires { get; init; }

	/// <summary>Whether alert throttling is on (<c>alert.suppress</c>).</summary>
	[JsonPropertyName("alert.suppress")]
	public bool AlertSuppress { get; init; }

	/// <summary>How long to throttle after a trigger, for example <c>1h</c> (<c>alert.suppress.period</c>).</summary>
	[JsonPropertyName("alert.suppress.period")]
	public string? AlertSuppressPeriod { get; init; }

	/// <summary>The fields whose values throttle separately, comma-separated (<c>alert.suppress.fields</c>).</summary>
	[JsonPropertyName("alert.suppress.fields")]
	public string? AlertSuppressFields { get; init; }

	/// <summary>
	/// Whether triggered alerts are listed in <c>alerts/fired_alerts</c>: <c>true</c>, <c>false</c> or <c>auto</c>
	/// (<c>alert.track</c>).
	/// </summary>
	[JsonPropertyName("alert.track")]
	public string? AlertTrack { get; init; }
}
