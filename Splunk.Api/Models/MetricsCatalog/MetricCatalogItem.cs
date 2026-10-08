using System.Text.Json.Serialization;

namespace Splunk.Api.Models.MetricsCatalog;

/// <summary>
/// A metric name, dimension name or dimension value from the metrics catalog (<c>catalog/metricstore/...</c>). The
/// entry's name is the item; the content carries only the indexes, when asked for.
/// </summary>
public sealed class MetricCatalogItem : SplunkContent
{
	/// <summary>The metric indexes the metric is in, when listed with <see cref="MetricListOptions.ListIndexes"/>.</summary>
	[JsonPropertyName("index")]
	public IReadOnlyList<string> Indexes { get; init; } = [];
}
