using Refit;
using Splunk.Api.Models.Search;

namespace Splunk.Api.Interfaces;

/// <summary>
/// Streaming export of search results (<c>search/v2/jobs/export</c>): Splunk runs the search and sends results as they
/// become available, without leaving a job to page through. Suits large result sets.
/// </summary>
/// <remarks>
/// Each method returns the response body as a stream that the caller reads and disposes; the body arrives while the
/// search runs. <see cref="SplunkSearch.ExportAsync(SearchExportRequest, CancellationToken)"/> reads the JSON stream
/// as <see cref="SearchResult"/> rows.
/// </remarks>
public interface ISearchExport
{
	/// <summary>Runs a search and streams its results as JSON lines (<c>POST search/v2/jobs/export</c>).</summary>
	/// <param name="request">The search and its parameters.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>One JSON object per line; read it with <see cref="SearchExportReader.ReadAsync"/>.</returns>
	[Post("services/search/v2/jobs/export")]
	Task<Stream> ExportAsync([Body] SearchExportRequest request, CancellationToken cancellationToken);

	/// <summary>Runs a search and streams its results as CSV (<c>POST search/v2/jobs/export?output_mode=csv</c>).</summary>
	/// <param name="request">The search and its parameters.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>CSV with a header row.</returns>
	[Post("services/search/v2/jobs/export?output_mode=csv")]
	Task<Stream> ExportCsvAsync([Body] SearchExportRequest request, CancellationToken cancellationToken);

	/// <summary>Runs a search and streams its events as raw text (<c>POST search/v2/jobs/export?output_mode=raw</c>).</summary>
	/// <param name="request">The search and its parameters.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>Each event's <c>_raw</c> text, one per line.</returns>
	[Post("services/search/v2/jobs/export?output_mode=raw")]
	Task<Stream> ExportRawAsync([Body] SearchExportRequest request, CancellationToken cancellationToken);
}
