using Refit;

namespace Splunk.Api.Models.Search;

/// <summary>Query parameters for <c>GET saved/searches/{name}/history</c>.</summary>
public sealed class SavedSearchHistoryOptions : ListOptions
{
	/// <summary>
	/// The saved search as a <c>user:app:name</c> triplet (<c>savedsearch</c>), which reaches a search's history from
	/// another user or app context.
	/// </summary>
	[AliasAs("savedsearch")]
	public string? SavedSearch { get; init; }
}
