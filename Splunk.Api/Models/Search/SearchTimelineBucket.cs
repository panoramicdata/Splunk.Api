using Splunk.Api.Serialization;
using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Search;

/// <summary>One time bucket of a <see cref="SearchJobTimeline"/>.</summary>
public sealed class SearchTimelineBucket
{
	/// <summary>The bucket's start.</summary>
	[JsonPropertyName("earliest_time")]
	[JsonConverter(typeof(EpochSecondsConverter))]
	public DateTimeOffset? EarliestTime { get; init; }

	/// <summary>The bucket's start, formatted by Splunk.</summary>
	[JsonPropertyName("earliest_strftime")]
	public string? EarliestStrftime { get; init; }

	/// <summary>The bucket's length in seconds.</summary>
	[JsonPropertyName("duration")]
	public double Duration { get; init; }

	/// <summary>The number of events in the bucket.</summary>
	[JsonPropertyName("total_count")]
	public long TotalCount { get; init; }

	/// <summary>The number of those events that can be retrieved (generally capped at 10000).</summary>
	[JsonPropertyName("available_count")]
	public long AvailableCount { get; init; }

	/// <summary>Whether the bucket's counts are final.</summary>
	[JsonPropertyName("is_finalized")]
	public bool IsFinalized { get; init; }

	/// <summary>The time zone offset of the bucket's start, in seconds.</summary>
	[JsonPropertyName("earliest_time_offset")]
	public int EarliestTimeOffset { get; init; }

	/// <summary>The time zone offset of the bucket's end, in seconds.</summary>
	[JsonPropertyName("latest_time_offset")]
	public int LatestTimeOffset { get; init; }
}
