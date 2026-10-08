using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Search;

/// <summary>Changes the overall search concurrency limits (<c>POST search/concurrency-settings/search</c>).</summary>
public sealed class SearchConcurrencyUpdateRequest : SplunkFormRequest
{
	/// <summary>The concurrent historical searches allowed per CPU (<c>max_searches_per_cpu</c>).</summary>
	[JsonPropertyName("max_searches_per_cpu")]
	public int? MaxSearchesPerCpu { get; init; }

	/// <summary>The concurrent searches allowed in addition to the per-CPU limit (<c>base_max_searches</c>).</summary>
	[JsonPropertyName("base_max_searches")]
	public int? BaseMaxSearches { get; init; }

	/// <summary>The multiplier giving the real-time search limit (<c>max_rt_search_multiplier</c>).</summary>
	[JsonPropertyName("max_rt_search_multiplier")]
	public double? MaxRealtimeSearchMultiplier { get; init; }
}
