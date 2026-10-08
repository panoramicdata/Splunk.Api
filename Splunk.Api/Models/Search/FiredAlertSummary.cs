using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Search;

/// <summary>
/// An alert with unexpired triggered instances (<c>GET alerts/fired_alerts</c>): one entry per alert, named after it,
/// plus an entry named <c>-</c> that totals them all.
/// </summary>
public sealed class FiredAlertSummary : SplunkContent
{
	/// <summary>The number of unexpired triggered instances.</summary>
	[JsonPropertyName("triggered_alert_count")]
	public int TriggeredAlertCount { get; init; }

	/// <summary>Whether the alert is a streaming metric alert.</summary>
	[JsonPropertyName("is_streaming_alert")]
	public bool IsStreamingAlert { get; init; }
}
