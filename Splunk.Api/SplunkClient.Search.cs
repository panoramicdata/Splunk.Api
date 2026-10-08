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

	/// <summary>Search language parsing (<c>search/v2/parser</c>).</summary>
	public ISearchParser SearchParser => field ??= For<ISearchParser>();

	/// <summary>Time argument parsing (<c>search/timeparser</c>).</summary>
	public ISearchTimeParser TimeParser => field ??= For<ISearchTimeParser>();

	/// <summary>Search auto-complete (<c>search/typeahead</c>).</summary>
	public ISearchTypeahead Typeahead => field ??= For<ISearchTypeahead>();

	/// <summary>The search scheduler's state (<c>search/scheduler</c>).</summary>
	public ISearchScheduler SearchScheduler => field ??= For<ISearchScheduler>();

	/// <summary>Search concurrency limits (<c>search/concurrency-settings</c>).</summary>
	public ISearchConcurrencySettings SearchConcurrencySettings => field ??= For<ISearchConcurrencySettings>();

	/// <summary>Custom search commands (<c>data/commands</c>).</summary>
	public ISearchCommands SearchCommands => field ??= For<ISearchCommands>();
}
