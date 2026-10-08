using Refit;
using Splunk.Api.Models.Search;

namespace Splunk.Api.Interfaces;

/// <summary>
/// A search job's results, events and result previews (<c>search/v2/jobs/{search_id}/...</c>). The v1 paths are
/// disabled by default since Splunk 9.0.1 and are not offered.
/// </summary>
/// <remarks>
/// GET reads a page; POST reads a page through an optional post-process search. Results are the rows after every
/// transforming command; events are the rows before the first one, and need a job with <c>status_buckets</c> above 0
/// or a search without transforming commands.
/// </remarks>
public interface ISearchJobResults
{
	/// <summary>Gets a page of a job's results (<c>GET search/v2/jobs/{search_id}/results</c>).</summary>
	/// <param name="searchId">The search ID.</param>
	/// <param name="options">Paging and fields; <see langword="null"/> for the first 100 rows.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>The page.</returns>
	[Get("services/search/v2/jobs/{searchId}/results")]
	Task<SearchResults> GetResultsAsync(string searchId, [Query] SearchResultsOptions? options, CancellationToken cancellationToken);

	/// <summary>Gets a page of a job's results as CSV (<c>GET search/v2/jobs/{search_id}/results?output_mode=csv</c>).</summary>
	/// <param name="searchId">The search ID.</param>
	/// <param name="options">Paging and fields; <see langword="null"/> for the first 100 rows.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>The CSV, with a header row. The caller disposes the stream.</returns>
	[Get("services/search/v2/jobs/{searchId}/results?output_mode=csv")]
	Task<Stream> GetResultsCsvAsync(string searchId, [Query] SearchResultsOptions? options, CancellationToken cancellationToken);

	/// <summary>Gets a page of a job's results through a post-process search (<c>POST search/v2/jobs/{search_id}/results</c>).</summary>
	/// <param name="searchId">The search ID.</param>
	/// <param name="request">The post-process search, paging and fields.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>The page; <see cref="SearchResults.PostProcessCount"/> counts the post-processed rows.</returns>
	[Post("services/search/v2/jobs/{searchId}/results")]
	Task<SearchResults> PostProcessResultsAsync(string searchId, [Body] SearchResultsRequest request, CancellationToken cancellationToken);

	/// <summary>Gets a page of a running job's preview results (<c>GET search/v2/jobs/{search_id}/results_preview</c>).</summary>
	/// <param name="searchId">The search ID.</param>
	/// <param name="options">Paging and fields; <see langword="null"/> for the first 100 rows.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>The page; <see cref="SearchResults.Preview"/> is <see langword="false"/> once the job has finished.</returns>
	[Get("services/search/v2/jobs/{searchId}/results_preview")]
	Task<SearchResults> GetPreviewAsync(string searchId, [Query] SearchResultsOptions? options, CancellationToken cancellationToken);

	/// <summary>
	/// Gets a page of a running job's preview results through a post-process search
	/// (<c>POST search/v2/jobs/{search_id}/results_preview</c>).
	/// </summary>
	/// <param name="searchId">The search ID.</param>
	/// <param name="request">The post-process search, paging and fields.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>The page.</returns>
	[Post("services/search/v2/jobs/{searchId}/results_preview")]
	Task<SearchResults> PostProcessPreviewAsync(string searchId, [Body] SearchResultsRequest request, CancellationToken cancellationToken);

	/// <summary>Gets a page of a job's untransformed events (<c>GET search/v2/jobs/{search_id}/events</c>).</summary>
	/// <param name="searchId">The search ID.</param>
	/// <param name="options">Paging, fields and time bounds; <see langword="null"/> for the first 100 events.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>The page.</returns>
	[Get("services/search/v2/jobs/{searchId}/events")]
	Task<SearchResults> GetEventsAsync(string searchId, [Query] SearchEventsOptions? options, CancellationToken cancellationToken);

	/// <summary>Gets a page of a job's events as raw text (<c>GET search/v2/jobs/{search_id}/events?output_mode=raw</c>).</summary>
	/// <param name="searchId">The search ID.</param>
	/// <param name="options">Paging and time bounds; <see langword="null"/> for the first 100 events.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>Each event's <c>_raw</c> text, one per line. The caller disposes the stream.</returns>
	[Get("services/search/v2/jobs/{searchId}/events?output_mode=raw")]
	Task<Stream> GetEventsRawAsync(string searchId, [Query] SearchEventsOptions? options, CancellationToken cancellationToken);

	/// <summary>Gets a page of a job's events through a post-process search (<c>POST search/v2/jobs/{search_id}/events</c>).</summary>
	/// <param name="searchId">The search ID.</param>
	/// <param name="request">The post-process search, paging, fields and time bounds.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>The page.</returns>
	[Post("services/search/v2/jobs/{searchId}/events")]
	Task<SearchResults> PostProcessEventsAsync(string searchId, [Body] SearchEventsRequest request, CancellationToken cancellationToken);
}
