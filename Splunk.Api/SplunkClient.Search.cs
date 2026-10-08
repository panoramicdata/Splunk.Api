using Splunk.Api.Interfaces;

namespace Splunk.Api;

public sealed partial class SplunkClient
{
	/// <summary>Runs searches end to end: a job to completion with all its results, a oneshot search, or a streamed export.</summary>
	public SplunkSearch Search => field ??= new SplunkSearch(this);

	/// <summary>Search jobs (<c>search/jobs</c>).</summary>
	public ISearchJobs SearchJobs => field ??= For<ISearchJobs>();

	/// <summary>Search job results, events and previews (<c>search/v2/jobs/{search_id}/results</c>, <c>events</c>, <c>results_preview</c>).</summary>
	public ISearchJobResults SearchJobResults => field ??= For<ISearchJobResults>();

	/// <summary>Streaming search export (<c>search/v2/jobs/export</c>).</summary>
	public ISearchExport SearchExport => field ??= For<ISearchExport>();
}
