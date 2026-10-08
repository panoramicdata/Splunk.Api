using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Search;

/// <summary>
/// The search parameters shared by creating a job (<see cref="SearchJobCreateRequest"/>), running a oneshot search
/// (<see cref="SearchOneshotRequest"/>) and exporting (<see cref="SearchExportRequest"/>). Leave a property
/// <see langword="null"/> to use Splunk's default; put any other documented parameter (for example <c>rt_blocking</c>,
/// <c>replay_speed</c> or <c>custom.*</c>) in <see cref="SplunkFormRequest.AdditionalParameters"/>.
/// </summary>
public abstract partial class SearchJobParameters : SplunkFormRequest
{
	/// <summary>
	/// The search, in SPL (<c>search</c>). A search that does not start with a generating command (such as
	/// <c>| makeresults</c> or <c>| tstats</c>) must start with <c>search</c>, for example <c>search index=_internal</c>.
	/// </summary>
	[JsonPropertyName("search")]
	public required string Search { get; init; }

	/// <summary>The inclusive earliest time (<c>earliest_time</c>): a relative time such as <c>-24h@h</c>, or an absolute time.</summary>
	[JsonPropertyName("earliest_time")]
	public string? EarliestTime { get; init; }

	/// <summary>The exclusive latest time (<c>latest_time</c>), for example <c>now</c>.</summary>
	[JsonPropertyName("latest_time")]
	public string? LatestTime { get; init; }

	/// <summary>The time relative times are measured from (<c>now</c>); defaults to the current time.</summary>
	[JsonPropertyName("now")]
	public string? Now { get; init; }

	/// <summary>The earliest index time (<c>index_earliest</c>).</summary>
	[JsonPropertyName("index_earliest")]
	public string? IndexEarliest { get; init; }

	/// <summary>The latest index time (<c>index_latest</c>).</summary>
	[JsonPropertyName("index_latest")]
	public string? IndexLatest { get; init; }

	/// <summary>The format of absolute times in the time parameters (<c>time_format</c>); Splunk's default is <c>%FT%T.%Q%:z</c>.</summary>
	[JsonPropertyName("time_format")]
	public string? TimeFormat { get; init; }

	/// <summary>The search mode (<c>search_mode</c>): normal or real-time.</summary>
	[JsonPropertyName("search_mode")]
	public SearchMode? SearchMode { get; init; }

	/// <summary>The ad hoc search level (<c>adhoc_search_level</c>): how many fields are extracted.</summary>
	[JsonPropertyName("adhoc_search_level")]
	public AdhocSearchLevel? AdhocSearchLevel { get; init; }

	/// <summary>The fields the search must return even if not otherwise needed (<c>rf</c>, repeated).</summary>
	[JsonPropertyName("rf")]
	public IReadOnlyList<string>? RequiredFields { get; init; }

	/// <summary>The app whose knowledge objects the search uses (<c>namespace</c>).</summary>
	[JsonPropertyName("namespace")]
	public string? Namespace { get; init; }

	/// <summary>Whether lookups are applied (<c>enable_lookups</c>); Splunk's default is true.</summary>
	[JsonPropertyName("enable_lookups")]
	public bool? EnableLookups { get; init; }

	/// <summary>Whether macro definitions are reloaded from macros.conf (<c>reload_macros</c>); Splunk's default is true.</summary>
	[JsonPropertyName("reload_macros")]
	public bool? ReloadMacros { get; init; }

	/// <summary>Whether the job returns partial results if a search peer fails (<c>allow_partial_results</c>).</summary>
	[JsonPropertyName("allow_partial_results")]
	public bool? AllowPartialResults { get; init; }

	/// <summary>The workload pool to run the search in (<c>workload_pool</c>).</summary>
	[JsonPropertyName("workload_pool")]
	public string? WorkloadPool { get; init; }
}
