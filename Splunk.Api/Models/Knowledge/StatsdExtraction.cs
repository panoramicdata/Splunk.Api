using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Knowledge;

/// <summary>A StatsD dimension extraction, a <c>statsd-dims:</c> stanza in <c>transforms.conf</c> (<c>data/transforms/statsdextractions</c>).</summary>
public sealed class StatsdExtraction : SplunkContent
{
	/// <summary>The regular expression whose named groups become dimensions (<c>REGEX</c>).</summary>
	[JsonPropertyName("REGEX")]
	public string? Regex { get; init; }

	/// <summary>Whether matched dimension values are removed from the metric name (<c>REMOVE_DIMS_FROM_METRIC_NAME</c>).</summary>
	[JsonPropertyName("REMOVE_DIMS_FROM_METRIC_NAME")]
	public bool? RemoveDimensionsFromMetricName { get; init; }
}
