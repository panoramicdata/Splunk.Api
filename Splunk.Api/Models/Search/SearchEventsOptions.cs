using Refit;

namespace Splunk.Api.Models.Search;

/// <summary>Query parameters for reading a job's events (<c>GET search/v2/jobs/{search_id}/events</c>).</summary>
public sealed class SearchEventsOptions : SearchPageOptions
{
	/// <summary>Only events at or after this time (<c>earliest_time</c>).</summary>
	[AliasAs("earliest_time")]
	public string? EarliestTime { get; init; }

	/// <summary>Only events before this time (<c>latest_time</c>).</summary>
	[AliasAs("latest_time")]
	public string? LatestTime { get; init; }

	/// <summary>The most lines of <c>_raw</c> to return per event (<c>max_lines</c>); 0 is no limit.</summary>
	[AliasAs("max_lines")]
	public int? MaxLines { get; init; }

	/// <summary>How <see cref="MaxLines"/> shortens an event (<c>truncation_mode</c>).</summary>
	[AliasAs("truncation_mode")]
	public TruncationMode? TruncationMode { get; init; }

	/// <summary>The segmentation to apply (<c>segmentation</c>), for example <c>raw</c> or <c>full</c>.</summary>
	[AliasAs("segmentation")]
	public string? Segmentation { get; init; }

	/// <summary>The strftime format of times in the output (<c>output_time_format</c>).</summary>
	[AliasAs("output_time_format")]
	public string? OutputTimeFormat { get; init; }

	/// <summary>The strftime format of absolute times in <see cref="EarliestTime"/> and <see cref="LatestTime"/> (<c>time_format</c>).</summary>
	[AliasAs("time_format")]
	public string? TimeFormat { get; init; }
}
