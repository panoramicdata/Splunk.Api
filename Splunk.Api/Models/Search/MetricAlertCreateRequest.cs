using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Search;

/// <summary>Creates a streaming metric alert (<c>POST alerts/metric_alerts</c>). Needs the <c>metric_alerts</c> capability.</summary>
public sealed class MetricAlertCreateRequest : MetricAlertSettings
{
	/// <summary>The name (<c>name</c>).</summary>
	[JsonPropertyName("name")]
	public required string Name { get; init; }

	/// <summary>
	/// The trigger condition (<c>condition</c>): an eval expression over quoted aggregations, for example
	/// <c>'avg(cpu.usage)' &gt; 90</c>. Every dimension it names must be in <see cref="MetricAlertSettings.GroupBy"/>.
	/// </summary>
	[JsonPropertyName("condition")]
	public required string Condition { get; init; }

	/// <summary>The metric indexes to watch, comma-separated (<c>metric_indexes</c>).</summary>
	[JsonPropertyName("metric_indexes")]
	public required string MetricIndexes { get; init; }
}
