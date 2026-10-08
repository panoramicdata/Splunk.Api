using Splunk.Api.Serialization;
using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Knowledge;

/// <summary>
/// The acceleration summary of an accelerated data model (<c>admin/summarization</c>, entry
/// <c>tstats:DM_{app}_{model}</c>).
/// </summary>
public sealed class DataModelSummary : SplunkContent
{
	/// <summary>The summarization search.</summary>
	[JsonPropertyName("search")]
	public string? Search { get; init; }

	/// <summary>The summary id, <c>DM_{app}_{model}</c> (<c>summary.id</c>).</summary>
	[JsonPropertyName("summary.id")]
	public string? SummaryId { get; init; }

	/// <summary>How complete the summary is, from 0 to 1 (<c>summary.complete</c>).</summary>
	[JsonPropertyName("summary.complete")]
	public double? Complete { get; init; }

	/// <summary>Whether a summary build is running (<c>summary.is_inprogress</c>).</summary>
	[JsonPropertyName("summary.is_inprogress")]
	public bool? IsInProgress { get; init; }

	/// <summary>The summary's size in bytes (<c>summary.size</c>).</summary>
	[JsonPropertyName("summary.size")]
	public long? Size { get; init; }

	/// <summary>The number of buckets in the summary (<c>summary.buckets</c>).</summary>
	[JsonPropertyName("summary.buckets")]
	public long? Buckets { get; init; }

	/// <summary>The size of the summary's buckets in megabytes (<c>summary.buckets_size</c>).</summary>
	[JsonPropertyName("summary.buckets_size")]
	public double? BucketsSizeMB { get; init; }

	/// <summary>The time range the summary covers, in seconds (<c>summary.time_range</c>).</summary>
	[JsonPropertyName("summary.time_range")]
	public long? TimeRangeSeconds { get; init; }

	/// <summary>The time of the earliest summarized event (<c>summary.earliest_time</c>).</summary>
	[JsonPropertyName("summary.earliest_time")]
	[JsonConverter(typeof(EpochSecondsConverter))]
	public DateTimeOffset? EarliestTime { get; init; }

	/// <summary>The time of the latest summarized event (<c>summary.latest_time</c>).</summary>
	[JsonPropertyName("summary.latest_time")]
	[JsonConverter(typeof(EpochSecondsConverter))]
	public DateTimeOffset? LatestTime { get; init; }

	/// <summary>When the summary was last modified (<c>summary.mod_time</c>).</summary>
	[JsonPropertyName("summary.mod_time")]
	[JsonConverter(typeof(EpochSecondsConverter))]
	public DateTimeOffset? ModifiedTime { get; init; }

	/// <summary>When the summary was last used (<c>summary.access_time</c>).</summary>
	[JsonPropertyName("summary.access_time")]
	[JsonConverter(typeof(EpochSecondsConverter))]
	public DateTimeOffset? AccessTime { get; init; }

	/// <summary>How many times the summary has been used (<c>summary.access_count</c>).</summary>
	[JsonPropertyName("summary.access_count")]
	public long? AccessCount { get; init; }

	/// <summary>The search id of the latest summarization search (<c>summary.last_sid</c>).</summary>
	[JsonPropertyName("summary.last_sid")]
	public string? LastSearchId { get; init; }

	/// <summary>Errors logged by the latest summarization search (<c>summary.last_error</c>).</summary>
	[JsonPropertyName("summary.last_error")]
	public string? LastError { get; init; }

	/// <summary>When the latest summarization search was dispatched (<c>summary.latest_dispatch_time</c>).</summary>
	[JsonPropertyName("summary.latest_dispatch_time")]
	[JsonConverter(typeof(EpochSecondsConverter))]
	public DateTimeOffset? LatestDispatchTime { get; init; }

	/// <summary>The run time of the latest summarization search, in seconds (<c>summary.latest_run_duration</c>).</summary>
	[JsonPropertyName("summary.latest_run_duration")]
	public double? LatestRunDuration { get; init; }

	/// <summary>The average run time of the last 48 summarization searches, in seconds (<c>summary.average_time</c>).</summary>
	[JsonPropertyName("summary.average_time")]
	public double? AverageTime { get; init; }

	/// <summary>The median summarization search run time, in seconds (<c>summary.p50</c>).</summary>
	[JsonPropertyName("summary.p50")]
	public double? P50 { get; init; }

	/// <summary>The 90th percentile summarization search run time, in seconds (<c>summary.p90</c>).</summary>
	[JsonPropertyName("summary.p90")]
	public double? P90 { get; init; }

	/// <summary>Up to 100 previous summarization searches, keyed by dispatch time (<c>summary.run_stats</c>); <see langword="null"/> before the first run.</summary>
	[JsonPropertyName("summary.run_stats")]
	public IReadOnlyDictionary<string, DataModelSummaryRun>? RunStats { get; init; }
}
