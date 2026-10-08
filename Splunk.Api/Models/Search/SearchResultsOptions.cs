using Refit;

namespace Splunk.Api.Models.Search;

/// <summary>Query parameters for reading a job's results or preview (<c>GET search/v2/jobs/{search_id}/results</c>).</summary>
public sealed class SearchResultsOptions : SearchPageOptions
{
	/// <summary>Whether to add field summary statistics to <see cref="SearchResults.Fields"/> (<c>add_summary_to_metadata</c>).</summary>
	[AliasAs("add_summary_to_metadata")]
	public bool? AddSummaryToMetadata { get; init; }
}
