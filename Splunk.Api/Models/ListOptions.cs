using Refit;

namespace Splunk.Api.Models;

/// <summary>
/// The paging, filtering and sorting parameters common to Splunk's collection (list) endpoints. Leave a property
/// <see langword="null"/> to use Splunk's default.
/// </summary>
public class ListOptions
{
	/// <summary>The maximum number of entries to return (<c>count</c>). Splunk's default is 30; 0 returns every entry.</summary>
	[AliasAs("count")]
	public int? Count { get; init; }

	/// <summary>The index of the first entry to return (<c>offset</c>).</summary>
	[AliasAs("offset")]
	public int? Offset { get; init; }

	/// <summary>
	/// A filter on entry names and properties (<c>search</c>), for example <c>disabled=0</c> or a plain substring.
	/// </summary>
	[AliasAs("search")]
	public string? Search { get; init; }

	/// <summary>The field to sort by (<c>sort_key</c>). Splunk's default is <c>name</c>.</summary>
	[AliasAs("sort_key")]
	public string? SortKey { get; init; }

	/// <summary>The sort direction (<c>sort_dir</c>).</summary>
	[AliasAs("sort_dir")]
	public SortDirection? SortDirection { get; init; }

	/// <summary>How values are compared when sorting (<c>sort_mode</c>).</summary>
	[AliasAs("sort_mode")]
	public SortMode? SortMode { get; init; }

	/// <summary>
	/// Limits each entry's content to these fields (<c>f</c>, repeated). Wildcards such as <c>dispatch.*</c> are allowed.
	/// </summary>
	[AliasAs("f")]
	[Query(CollectionFormat.Multi)]
	public IEnumerable<string>? Fields { get; init; }

	/// <summary>When <see langword="true"/>, Splunk returns a summary of each entry rather than every property (<c>summarize</c>).</summary>
	[AliasAs("summarize")]
	public bool? Summarize { get; init; }
}
