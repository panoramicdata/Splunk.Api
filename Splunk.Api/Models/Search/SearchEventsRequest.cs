using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Search;

/// <summary>Reads a job's events with POST (<c>POST search/v2/jobs/{search_id}/events</c>).</summary>
public sealed class SearchEventsRequest : SearchPageRequest
{
	/// <summary>Only events at or after this time (<c>earliest_time</c>).</summary>
	[JsonPropertyName("earliest_time")]
	public string? EarliestTime { get; init; }

	/// <summary>Only events before this time (<c>latest_time</c>).</summary>
	[JsonPropertyName("latest_time")]
	public string? LatestTime { get; init; }

	/// <summary>The most lines of <c>_raw</c> to return per event (<c>max_lines</c>); 0 is no limit.</summary>
	[JsonPropertyName("max_lines")]
	public int? MaxLines { get; init; }

	/// <summary>How <see cref="MaxLines"/> shortens an event (<c>truncation_mode</c>).</summary>
	[JsonPropertyName("truncation_mode")]
	public TruncationMode? TruncationMode { get; init; }

	/// <summary>The segmentation to apply (<c>segmentation</c>).</summary>
	[JsonPropertyName("segmentation")]
	public string? Segmentation { get; init; }

	/// <summary>The strftime format of times in the output (<c>output_time_format</c>).</summary>
	[JsonPropertyName("output_time_format")]
	public string? OutputTimeFormat { get; init; }

	/// <summary>The strftime format of absolute times in the time bounds (<c>time_format</c>).</summary>
	[JsonPropertyName("time_format")]
	public string? TimeFormat { get; init; }
}
