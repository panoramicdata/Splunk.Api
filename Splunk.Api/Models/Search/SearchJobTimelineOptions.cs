using Refit;

namespace Splunk.Api.Models.Search;

/// <summary>Query parameters for a job's timeline (<c>GET search/jobs/{search_id}/timeline</c>).</summary>
public sealed class SearchJobTimelineOptions
{
	/// <summary>The strftime format of times in the output (<c>output_time_format</c>).</summary>
	[AliasAs("output_time_format")]
	public string? OutputTimeFormat { get; init; }

	/// <summary>The strftime format of absolute times (<c>time_format</c>).</summary>
	[AliasAs("time_format")]
	public string? TimeFormat { get; init; }
}
