using System.Text.Json;
using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Search;

/// <summary>The statistics of one field in a <see cref="SearchJobSummary"/>.</summary>
public sealed class SearchFieldSummary
{
	/// <summary>The number of events with the field.</summary>
	[JsonPropertyName("count")]
	public long Count { get; init; }

	/// <summary>The number of events in which the field is numeric.</summary>
	[JsonPropertyName("numeric_count")]
	public long NumericCount { get; init; }

	/// <summary>The number of distinct values.</summary>
	[JsonPropertyName("distinct_count")]
	public long DistinctCount { get; init; }

	/// <summary>Whether the counts are exact rather than estimated.</summary>
	[JsonPropertyName("is_exact")]
	public bool IsExact { get; init; }

	/// <summary>Whether Splunk considers the field relevant to the search.</summary>
	[JsonPropertyName("relevant")]
	public bool Relevant { get; init; }

	/// <summary>The smallest value, for a numeric field.</summary>
	[JsonPropertyName("min")]
	public string? Min { get; init; }

	/// <summary>The largest value, for a numeric field.</summary>
	[JsonPropertyName("max")]
	public string? Max { get; init; }

	/// <summary>The mean, for a numeric field.</summary>
	[JsonPropertyName("mean")]
	public double? Mean { get; init; }

	/// <summary>The standard deviation, for a numeric field.</summary>
	[JsonPropertyName("stdev")]
	public double? StandardDeviation { get; init; }

	/// <summary>The most frequent values, most frequent first.</summary>
	[JsonPropertyName("modes")]
	public IReadOnlyList<SearchFieldMode> Modes { get; init; } = [];

	/// <summary>Any other statistics Splunk returned.</summary>
	[JsonExtensionData]
	public IDictionary<string, JsonElement> AdditionalProperties { get; init; } = new Dictionary<string, JsonElement>();
}
