using System.Text.Json;
using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Search;

/// <summary>
/// Field statistics of a job's events so far (<c>search/jobs/{search_id}/summary</c>). Only jobs created with
/// <c>status_buckets</c> above 0 have one.
/// </summary>
public sealed class SearchJobSummary
{
	/// <summary>The earliest time of the summarised events.</summary>
	[JsonPropertyName("earliest_time")]
	public DateTimeOffset? EarliestTime { get; init; }

	/// <summary>The latest time of the summarised events.</summary>
	[JsonPropertyName("latest_time")]
	public DateTimeOffset? LatestTime { get; init; }

	/// <summary>The span of the summarised events, in seconds.</summary>
	[JsonPropertyName("duration")]
	public double Duration { get; init; }

	/// <summary>The number of events summarised.</summary>
	[JsonPropertyName("event_count")]
	public long EventCount { get; init; }

	/// <summary>Statistics for each field, keyed by field name.</summary>
	[JsonPropertyName("fields")]
	public IReadOnlyDictionary<string, SearchFieldSummary> Fields { get; init; } = new Dictionary<string, SearchFieldSummary>();

	/// <summary>Any other properties, such as a histogram when one is requested.</summary>
	[JsonExtensionData]
	public IDictionary<string, JsonElement> AdditionalProperties { get; init; } = new Dictionary<string, JsonElement>();
}
