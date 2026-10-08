namespace Splunk.Api;

/// <summary>How <see cref="SplunkSearch.RunAsync(Models.Search.SearchJobCreateRequest, SearchRunOptions, CancellationToken)"/> runs a search.</summary>
public sealed class SearchRunOptions : SearchWaitOptions
{
	/// <summary>The number of results read per request. The default is 10000; Splunk caps it at <c>maxresultrows</c> (50000 by default).</summary>
	public int PageSize { get; init; } = 10_000;

	/// <summary>
	/// Whether to delete the job once its results are read (the default). A job that fails, times out or is cancelled
	/// is always deleted.
	/// </summary>
	public bool DeleteJobWhenDone { get; init; } = true;
}
