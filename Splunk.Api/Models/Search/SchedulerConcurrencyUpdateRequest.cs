using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Search;

/// <summary>Changes the scheduler's concurrency limits (<c>POST search/concurrency-settings/scheduler</c>).</summary>
public sealed class SchedulerConcurrencyUpdateRequest : SplunkFormRequest
{
	/// <summary>The percentage, 1 to 100, of the concurrent search limit the scheduler may use (<c>max_searches_perc</c>).</summary>
	[JsonPropertyName("max_searches_perc")]
	public int? MaxSearchesPercent { get; init; }

	/// <summary>The percentage, 1 to 100, of the scheduler's searches auto-summarization may use (<c>auto_summary_perc</c>).</summary>
	[JsonPropertyName("auto_summary_perc")]
	public int? AutoSummaryPercent { get; init; }
}
