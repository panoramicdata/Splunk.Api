using Refit;

namespace Splunk.Api.Models.Search;

/// <summary>
/// Query parameters for reading saved searches (<c>GET saved/searches</c> and <c>GET saved/searches/{name}</c>; the
/// paging properties only apply to the list).
/// </summary>
public sealed class SavedSearchListOptions : ListOptions
{
	/// <summary>With <see cref="LatestTime"/>, lists each scheduled search's run times in this range (<c>earliest_time</c>).</summary>
	[AliasAs("earliest_time")]
	public string? EarliestTime { get; init; }

	/// <summary>The end of the range of run times to list (<c>latest_time</c>).</summary>
	[AliasAs("latest_time")]
	public string? LatestTime { get; init; }

	/// <summary>Whether to list the defaults of every alert action, even unused ones (<c>listDefaultActionArgs</c>).</summary>
	[AliasAs("listDefaultActionArgs")]
	public bool? ListDefaultActionArgs { get; init; }

	/// <summary>Whether to add an <c>orphan</c> field flagging searches whose owner no longer exists (<c>add_orphan_field</c>).</summary>
	[AliasAs("add_orphan_field")]
	public bool? AddOrphanField { get; init; }
}
