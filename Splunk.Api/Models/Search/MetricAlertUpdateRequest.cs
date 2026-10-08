using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Search;

/// <summary>
/// Changes a streaming metric alert (<c>POST alerts/metric_alerts/{alert_name}</c>); only the properties set are changed.
/// </summary>
/// <remarks>
/// The reference marks <c>condition</c> and <c>metric_indexes</c> as required on update, but Splunk 10.6 accepts an
/// update without them, so both are optional here.
/// </remarks>
public sealed class MetricAlertUpdateRequest : MetricAlertSettings
{
	/// <summary>The trigger condition (<c>condition</c>).</summary>
	[JsonPropertyName("condition")]
	public string? Condition { get; init; }

	/// <summary>The metric indexes to watch, comma-separated (<c>metric_indexes</c>).</summary>
	[JsonPropertyName("metric_indexes")]
	public string? MetricIndexes { get; init; }
}
