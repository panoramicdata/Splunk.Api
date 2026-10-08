using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Search;

/// <summary>
/// Search concurrency limits (<c>search/concurrency-settings</c>). The <c>scheduler</c> entry has the scheduler
/// percentages; the <c>search</c> entry has the overall limits. Properties an entry does not have are
/// <see langword="null"/>.
/// </summary>
public sealed class SearchConcurrencySettings : SplunkContent
{
	/// <summary>The percentage of the concurrent search limit the scheduler may use (<c>max_searches_perc</c>).</summary>
	[JsonPropertyName("max_searches_perc")]
	public int? MaxSearchesPercent { get; init; }

	/// <summary>The percentage of the scheduler's searches that auto-summarization may use (<c>auto_summary_perc</c>).</summary>
	[JsonPropertyName("auto_summary_perc")]
	public int? AutoSummaryPercent { get; init; }

	/// <summary>The concurrent historical searches allowed per CPU (<c>max_searches_per_cpu</c>).</summary>
	[JsonPropertyName("max_searches_per_cpu")]
	public int? MaxSearchesPerCpu { get; init; }

	/// <summary>The concurrent searches allowed in addition to the per-CPU limit (<c>base_max_searches</c>).</summary>
	[JsonPropertyName("base_max_searches")]
	public int? BaseMaxSearches { get; init; }

	/// <summary>The multiplier giving the real-time search limit (<c>max_rt_search_multiplier</c>).</summary>
	[JsonPropertyName("max_rt_search_multiplier")]
	public double? MaxRealtimeSearchMultiplier { get; init; }

	/// <summary>The total concurrent search limit, a number or <c>auto</c> (<c>total_search_concurrency_limit</c>).</summary>
	[JsonPropertyName("total_search_concurrency_limit")]
	public string? TotalSearchConcurrencyLimit { get; init; }

	/// <summary>Search head cluster ad hoc quota enforcement, for example <c>off</c> (<c>shc_adhoc_quota_enforcement</c>).</summary>
	[JsonPropertyName("shc_adhoc_quota_enforcement")]
	public string? ShcAdhocQuotaEnforcement { get; init; }
}
