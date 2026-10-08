using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Knowledge;

/// <summary>Creates a StatsD dimension extraction (<c>POST data/transforms/statsdextractions</c>).</summary>
/// <remarks>The reference calls the name field <c>unique_transforms_stanza_name</c>; Splunk 10.6 takes <c>name</c> and stores the stanza as <c>statsd-dims:{name}</c>.</remarks>
public sealed class StatsdExtractionCreateRequest : SplunkFormRequest
{
	/// <summary>The extraction name.</summary>
	[JsonPropertyName("name")]
	public required string Name { get; init; }

	/// <summary>The regular expression whose named groups become dimensions (<c>REGEX</c>).</summary>
	[JsonPropertyName("REGEX")]
	public required string Regex { get; init; }

	/// <summary>Whether matched dimension values are removed from the metric name (<c>REMOVE_DIMS_FROM_METRIC_NAME</c>, default true).</summary>
	[JsonPropertyName("REMOVE_DIMS_FROM_METRIC_NAME")]
	public bool? RemoveDimensionsFromMetricName { get; init; }
}
