using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Introspection;

/// <summary>The search concurrency limits of a standalone instance (<c>server/status/limits/search-concurrency</c>).</summary>
public sealed class SearchConcurrencyLimits : SplunkContent
{
	/// <summary>The maximum concurrent historical searches (<c>max_hist_searches</c>).</summary>
	[JsonPropertyName("max_hist_searches")]
	public int MaxHistoricalSearches { get; init; }

	/// <summary>The maximum concurrent scheduled historical searches (<c>max_hist_scheduled_searches</c>).</summary>
	[JsonPropertyName("max_hist_scheduled_searches")]
	public int MaxHistoricalScheduledSearches { get; init; }

	/// <summary>The maximum concurrent real-time searches (<c>max_rt_searches</c>).</summary>
	[JsonPropertyName("max_rt_searches")]
	public int MaxRealTimeSearches { get; init; }

	/// <summary>The maximum concurrent scheduled real-time searches (<c>max_rt_scheduled_searches</c>).</summary>
	[JsonPropertyName("max_rt_scheduled_searches")]
	public int MaxRealTimeScheduledSearches { get; init; }

	/// <summary>The maximum concurrent summarization searches (<c>max_auto_summary_searches</c>).</summary>
	[JsonPropertyName("max_auto_summary_searches")]
	public int MaxAutoSummarySearches { get; init; }
}
