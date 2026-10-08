using Refit;
using Splunk.Api.Models;
using Splunk.Api.Models.Search;

namespace Splunk.Api.Interfaces;

/// <summary>
/// Search jobs (<c>search/jobs</c>): create, inspect, control and delete them. Read a job's results and events with
/// <see cref="ISearchJobResults"/>; <see cref="SplunkSearch"/> runs a search end to end.
/// </summary>
public interface ISearchJobs
{
	/// <summary>Lists the current search jobs the caller can see (<c>GET search/jobs</c>).</summary>
	/// <param name="options">Paging and filtering, for example <c>Search = "isDone=1"</c>; <see langword="null"/> for Splunk's defaults.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed of jobs, each named by its search string; the search ID is <see cref="SearchJob.Sid"/>.</returns>
	[Get("services/search/jobs")]
	Task<SplunkFeed<SearchJob>> ListAsync([Query] ListOptions? options, CancellationToken cancellationToken);

	/// <summary>Creates (dispatches) a search job (<c>POST search/jobs</c>).</summary>
	/// <param name="request">The search and its parameters.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>The new job's search ID. With <see cref="SearchExecutionMode.Blocking"/> the job has finished by then.</returns>
	/// <remarks>Needs the <c>search</c> capability. A job belongs to its creator and expires after its time to live.</remarks>
	[Post("services/search/jobs")]
	Task<SearchJobCreated> CreateAsync([Body] SearchJobCreateRequest request, CancellationToken cancellationToken);

	/// <summary>
	/// Runs a oneshot search and returns its results (<c>POST search/jobs</c> with <c>exec_mode=oneshot</c>); no job is
	/// left behind.
	/// </summary>
	/// <param name="request">The search and its parameters.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>The results: the first 100 unless <see cref="SearchOneshotRequest.Count"/> says otherwise.</returns>
	/// <remarks>The request waits until the search finishes, so it suits small, quick searches.</remarks>
	[Post("services/search/jobs")]
	Task<SearchResults> RunOneshotAsync([Body] SearchOneshotRequest request, CancellationToken cancellationToken);

	/// <summary>Gets a search job's properties (<c>GET search/jobs/{search_id}</c>).</summary>
	/// <param name="searchId">The search ID.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with one entry, the job.</returns>
	[Get("services/search/jobs/{searchId}")]
	Task<SplunkFeed<SearchJob>> GetAsync(string searchId, CancellationToken cancellationToken);

	/// <summary>Sets custom properties on a search job (<c>POST search/jobs/{search_id}</c>).</summary>
	/// <param name="searchId">The search ID.</param>
	/// <param name="request">The custom properties.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with one entry, the job.</returns>
	[Post("services/search/jobs/{searchId}")]
	Task<SplunkFeed<SearchJob>> UpdateAsync(string searchId, [Body] SearchJobUpdateRequest request, CancellationToken cancellationToken);

	/// <summary>Cancels a search job if it is running and deletes it and its results (<c>DELETE search/jobs/{search_id}</c>).</summary>
	/// <param name="searchId">The search ID.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A task that completes when the job is deleted.</returns>
	[Delete("services/search/jobs/{searchId}")]
	Task DeleteAsync(string searchId, CancellationToken cancellationToken);

	/// <summary>Pauses, finalizes, cancels or otherwise controls a search job (<c>POST search/jobs/{search_id}/control</c>).</summary>
	/// <param name="searchId">The search ID.</param>
	/// <param name="request">The action and its argument.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>Splunk's confirmation, for example <c>The ttl of the search job was changed to 120.</c></returns>
	[Post("services/search/jobs/{searchId}/control")]
	Task<SearchMessagesResponse> ControlAsync(string searchId, [Body] SearchJobControlRequest request, CancellationToken cancellationToken);

	/// <summary>Gets field statistics of a job's events so far (<c>GET search/jobs/{search_id}/summary</c>).</summary>
	/// <param name="searchId">The search ID.</param>
	/// <param name="options">Which fields and events to summarise; <see langword="null"/> for every field.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>The summary, or <see langword="null"/> when Splunk answers 204 No Content (the job has no events yet).</returns>
	/// <remarks>Only jobs created with <c>status_buckets</c> above 0 summarise their events.</remarks>
	[Get("services/search/jobs/{searchId}/summary")]
	Task<SearchJobSummary?> GetSummaryAsync(string searchId, [Query] SearchJobSummaryOptions? options, CancellationToken cancellationToken);

	/// <summary>Gets how a job's events are distributed over time (<c>GET search/jobs/{search_id}/timeline</c>).</summary>
	/// <param name="searchId">The search ID.</param>
	/// <param name="options">Time formats; <see langword="null"/> for Splunk's defaults.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>The timeline, or <see langword="null"/> when Splunk answers 204 No Content (the job has no events).</returns>
	/// <remarks>Only jobs created with <c>status_buckets</c> above 0 keep a timeline.</remarks>
	[Get("services/search/jobs/{searchId}/timeline")]
	Task<SearchJobTimeline?> GetTimelineAsync(string searchId, [Query] SearchJobTimelineOptions? options, CancellationToken cancellationToken);

	/// <summary>Gets a job's search log (<c>GET search/jobs/{search_id}/search.log</c>).</summary>
	/// <param name="searchId">The search ID.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>The log as plain text, one line per entry.</returns>
	[Get("services/search/jobs/{searchId}/search.log")]
	Task<string> GetSearchLogAsync(string searchId, CancellationToken cancellationToken);
}
