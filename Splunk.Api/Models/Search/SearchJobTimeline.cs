using Splunk.Api.Serialization;
using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Search;

/// <summary>
/// How a job's untransformed events are distributed over time (<c>search/jobs/{search_id}/timeline</c>). Only jobs
/// created with <c>status_buckets</c> above 0 have one.
/// </summary>
public sealed class SearchJobTimeline
{
	/// <summary>The earliest time from which no events are later scanned, or <see langword="null"/> when not set.</summary>
	[JsonPropertyName("cursor_time")]
	[JsonConverter(typeof(EpochSecondsConverter))]
	public DateTimeOffset? CursorTime { get; init; }

	/// <summary>Whether the job reads events in time order.</summary>
	[JsonPropertyName("is_time_cursored")]
	public bool IsTimeCursored { get; init; }

	/// <summary>The number of events counted.</summary>
	[JsonPropertyName("event_count")]
	public long EventCount { get; init; }

	/// <summary>The time buckets, earliest first.</summary>
	[JsonPropertyName("buckets")]
	public IReadOnlyList<SearchTimelineBucket> Buckets { get; init; } = [];
}
