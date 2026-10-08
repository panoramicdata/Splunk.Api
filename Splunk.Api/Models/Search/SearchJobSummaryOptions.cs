using Refit;

namespace Splunk.Api.Models.Search;

/// <summary>Query parameters for a job's field summary (<c>GET search/jobs/{search_id}/summary</c>).</summary>
public sealed class SearchJobSummaryOptions
{
	/// <summary>The fields to summarise (<c>f</c>, repeated); every field by default.</summary>
	[AliasAs("f")]
	[Query(CollectionFormat.Multi)]
	public IEnumerable<string>? Fields { get; init; }

	/// <summary>Only events at or after this time (<c>earliest_time</c>).</summary>
	[AliasAs("earliest_time")]
	public string? EarliestTime { get; init; }

	/// <summary>Only events before this time (<c>latest_time</c>).</summary>
	[AliasAs("latest_time")]
	public string? LatestTime { get; init; }

	/// <summary>Only events containing this text in a value or tag (<c>search</c>).</summary>
	[AliasAs("search")]
	public string? Search { get; init; }

	/// <summary>The fraction of events, 0 to 1, a field must appear in to be listed (<c>min_freq</c>).</summary>
	[AliasAs("min_freq")]
	public double? MinFrequency { get; init; }

	/// <summary>How many of each field's most frequent values to return (<c>top_count</c>); Splunk's default is 10.</summary>
	[AliasAs("top_count")]
	public int? TopCount { get; init; }

	/// <summary>Whether to add histogram data (<c>histogram</c>).</summary>
	[AliasAs("histogram")]
	public bool? Histogram { get; init; }

	/// <summary>The strftime format of times in the output (<c>output_time_format</c>).</summary>
	[AliasAs("output_time_format")]
	public string? OutputTimeFormat { get; init; }

	/// <summary>The strftime format of absolute times in the time bounds (<c>time_format</c>).</summary>
	[AliasAs("time_format")]
	public string? TimeFormat { get; init; }
}
