using Splunk.Api.Serialization;
using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Search;

/// <summary>
/// One triggered instance of an alert (<c>GET alerts/fired_alerts/{name}</c>). The entry's name identifies the instance
/// and is what <see cref="Interfaces.IFiredAlerts.DeleteAsync"/> takes.
/// </summary>
public sealed class FiredAlert : SplunkContent
{
	/// <summary>The saved search that triggered.</summary>
	[JsonPropertyName("savedsearch_name")]
	public string? SavedSearchName { get; init; }

	/// <summary>The search ID of the job that triggered.</summary>
	[JsonPropertyName("sid")]
	public string? Sid { get; init; }

	/// <summary>When the alert triggered.</summary>
	[JsonPropertyName("trigger_time")]
	[JsonConverter(typeof(EpochSecondsConverter))]
	public DateTimeOffset? TriggerTime { get; init; }

	/// <summary>When the alert triggered, as Splunk formats it.</summary>
	[JsonPropertyName("trigger_time_rendered")]
	public string? TriggerTimeRendered { get; init; }

	/// <summary>When the instance expires, as Splunk formats it.</summary>
	[JsonPropertyName("expiration_time_rendered")]
	public string? ExpirationTimeRendered { get; init; }

	/// <summary>The alert type: <c>historical</c> (scheduled) or <c>real-time</c>.</summary>
	[JsonPropertyName("alert_type")]
	public string? AlertType { get; init; }

	/// <summary>Whether the instance covers all results (digest) rather than one result.</summary>
	[JsonPropertyName("digest_mode")]
	public bool DigestMode { get; init; }

	/// <summary>The severity, 1 (debug) to 6 (fatal).</summary>
	[JsonPropertyName("severity")]
	public int Severity { get; init; }

	/// <summary>The number of alerts this instance stands for.</summary>
	[JsonPropertyName("triggered_alerts")]
	public int TriggeredAlerts { get; init; }

	/// <summary>The actions that ran, comma-separated, or <see langword="null"/> for none.</summary>
	[JsonPropertyName("actions")]
	public string? Actions { get; init; }
}
