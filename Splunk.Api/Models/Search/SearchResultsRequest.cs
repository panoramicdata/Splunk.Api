using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Search;

/// <summary>Reads a job's results or preview with POST (<c>POST search/v2/jobs/{search_id}/results</c>).</summary>
public sealed class SearchResultsRequest : SearchPageRequest
{
	/// <summary>Whether to add field summary statistics to <see cref="SearchResults.Fields"/> (<c>add_summary_to_metadata</c>).</summary>
	[JsonPropertyName("add_summary_to_metadata")]
	public bool? AddSummaryToMetadata { get; init; }
}
