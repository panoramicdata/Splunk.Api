using Splunk.Api.Models.Search;
using System.Runtime.CompilerServices;

namespace Splunk.Api;

public sealed partial class SplunkSearch
{
	/// <summary>Runs a oneshot search and returns all of its results; no job is left behind.</summary>
	/// <param name="search">The search, for example <c>| makeresults count=5</c>.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>Every result (the request sets <c>count=0</c>).</returns>
	/// <remarks>The request lasts as long as the search, so keep oneshot searches small and quick.</remarks>
	public Task<SearchResults> OneshotAsync(string search, CancellationToken cancellationToken)
		=> OneshotAsync(new SearchOneshotRequest { Search = search, Count = 0 }, cancellationToken);

	/// <summary>Runs a oneshot search and returns its results; no job is left behind.</summary>
	/// <param name="request">The search, its parameters and which results to return.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>The results.</returns>
	public Task<SearchResults> OneshotAsync(SearchOneshotRequest request, CancellationToken cancellationToken)
		=> _client.SearchJobs.RunOneshotAsync(request, cancellationToken);

	/// <summary>Runs a search and streams its final results as Splunk produces them.</summary>
	/// <param name="search">The search.</param>
	/// <param name="cancellationToken">Stops the export; the connection is closed.</param>
	/// <returns>The results, as they arrive.</returns>
	public IAsyncEnumerable<SearchResult> ExportAsync(string search, CancellationToken cancellationToken)
		=> ExportAsync(new SearchExportRequest { Search = search }, cancellationToken);

	/// <summary>
	/// Runs a search and streams its final results as Splunk produces them (<c>search/v2/jobs/export</c>), reading the
	/// response incrementally. Preview rows of a transforming search are skipped.
	/// </summary>
	/// <param name="request">The search and its parameters.</param>
	/// <param name="cancellationToken">Stops the export; the connection is closed.</param>
	/// <returns>The results, as they arrive.</returns>
	/// <exception cref="SplunkSearchException">The stream reported an error.</exception>
	public async IAsyncEnumerable<SearchResult> ExportAsync(SearchExportRequest request, [EnumeratorCancellation] CancellationToken cancellationToken)
	{
		var stream = await _client.SearchExport.ExportAsync(request, cancellationToken).ConfigureAwait(false);
		await using (stream.ConfigureAwait(false))
		{
			await foreach (var record in SearchExportReader.ReadAsync(stream, cancellationToken).ConfigureAwait(false))
			{
				if (record.Messages.Any(m => SplunkSearchException.IsError(m.Type)))
				{
					throw new SplunkSearchException(record.Messages, null);
				}

				if (!record.Preview && record.Result is { } result)
				{
					yield return result;
				}
			}
		}
	}
}
