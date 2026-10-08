using Refit;
using Splunk.Api.Models;
using Splunk.Api.Models.Search;

namespace Splunk.Api.Interfaces;

/// <summary>
/// Saved searches, reports and alerts (<c>saved/searches</c>). Use <see cref="SplunkClient.InNamespace(string, string)"/>
/// to choose the owner and app a search is created in or read from; <c>services/</c> uses the caller's default app.
/// </summary>
public interface ISavedSearches
{
	/// <summary>Lists saved searches (<c>GET saved/searches</c>).</summary>
	/// <param name="options">Paging, filtering and saved-search options; <see langword="null"/> for Splunk's defaults.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed of saved searches.</returns>
	[Get("services/saved/searches")]
	Task<SplunkFeed<SavedSearch>> ListAsync([Query] SavedSearchListOptions? options, CancellationToken cancellationToken);

	/// <summary>Creates a saved search (<c>POST saved/searches</c>).</summary>
	/// <param name="request">The name, search and settings.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with one entry, the new search.</returns>
	[Post("services/saved/searches")]
	Task<SplunkFeed<SavedSearch>> CreateAsync([Body] SavedSearchCreateRequest request, CancellationToken cancellationToken);

	/// <summary>Gets a saved search (<c>GET saved/searches/{name}</c>).</summary>
	/// <param name="name">The name.</param>
	/// <param name="options">Saved-search options (the paging properties are ignored); <see langword="null"/> for none.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with one entry.</returns>
	[Get("services/saved/searches/{name}")]
	Task<SplunkFeed<SavedSearch>> GetAsync(string name, [Query] SavedSearchListOptions? options, CancellationToken cancellationToken);

	/// <summary>Changes a saved search (<c>POST saved/searches/{name}</c>).</summary>
	/// <param name="name">The name.</param>
	/// <param name="request">The settings to change.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with one entry, the changed search.</returns>
	[Post("services/saved/searches/{name}")]
	Task<SplunkFeed<SavedSearch>> UpdateAsync(string name, [Body] SavedSearchUpdateRequest request, CancellationToken cancellationToken);

	/// <summary>Deletes a saved search (<c>DELETE saved/searches/{name}</c>).</summary>
	/// <param name="name">The name.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A task that completes when the search is deleted.</returns>
	[Delete("services/saved/searches/{name}")]
	Task DeleteAsync(string name, CancellationToken cancellationToken);

	/// <summary>Runs a saved search now (<c>POST saved/searches/{name}/dispatch</c>).</summary>
	/// <param name="name">The name.</param>
	/// <param name="request">Overrides for this run; an empty request runs it as saved.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>The new job's search ID.</returns>
	[Post("services/saved/searches/{name}/dispatch")]
	Task<SearchJobCreated> DispatchAsync(string name, [Body] SavedSearchDispatchRequest request, CancellationToken cancellationToken);

	/// <summary>Lists the jobs a saved search has run that still exist (<c>GET saved/searches/{name}/history</c>).</summary>
	/// <param name="name">The name.</param>
	/// <param name="options">Paging, filtering and the <c>savedsearch</c> triplet; <see langword="null"/> for Splunk's defaults.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed of jobs, each named by its search ID.</returns>
	[Get("services/saved/searches/{name}/history")]
	Task<SplunkFeed<SavedSearchHistoryEntry>> GetHistoryAsync(string name, [Query] SavedSearchHistoryOptions? options, CancellationToken cancellationToken);

	/// <summary>Makes a scheduled search run next at a given time, then on its schedule (<c>POST saved/searches/{name}/reschedule</c>).</summary>
	/// <param name="name">The name.</param>
	/// <param name="request">The time of the next run.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with the saved search.</returns>
	[Post("services/saved/searches/{name}/reschedule")]
	Task<SplunkFeed<SavedSearch>> RescheduleAsync(string name, [Body] RescheduleRequest request, CancellationToken cancellationToken);

	/// <summary>Lists when a scheduled search will run in a time range (<c>GET saved/searches/{name}/scheduled_times</c>).</summary>
	/// <param name="name">The name.</param>
	/// <param name="earliestTime">The start of the range (<c>earliest_time</c>), for example <c>now</c>.</param>
	/// <param name="latestTime">The end of the range (<c>latest_time</c>), for example <c>+1d</c>.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with the saved search, whose <see cref="ScheduledContent.ScheduledTimes"/> lists the run times.</returns>
	[Get("services/saved/searches/{name}/scheduled_times")]
	Task<SplunkFeed<SavedSearch>> GetScheduledTimesAsync(
		string name,
		[AliasAs("earliest_time")] string earliestTime,
		[AliasAs("latest_time")] string latestTime,
		CancellationToken cancellationToken);

	/// <summary>Gets an alert's throttling state (<c>GET saved/searches/{name}/suppress</c>).</summary>
	/// <param name="name">The name.</param>
	/// <param name="options">The suppression key and expiry; <see langword="null"/> for the search's own.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with one entry, the suppression state.</returns>
	[Get("services/saved/searches/{name}/suppress")]
	Task<SplunkFeed<SavedSearchSuppression>> GetSuppressionAsync(string name, [Query] SavedSearchSuppressionOptions? options, CancellationToken cancellationToken);

	/// <summary>Lifts an alert's throttling (<c>POST saved/searches/{name}/acknowledge</c>).</summary>
	/// <param name="name">The name.</param>
	/// <param name="request">The suppression key.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with one entry, the suppression state after acknowledging.</returns>
	[Post("services/saved/searches/{name}/acknowledge")]
	Task<SplunkFeed<SavedSearchSuppression>> AcknowledgeAsync(string name, [Body] SavedSearchAcknowledgeRequest request, CancellationToken cancellationToken);
}
