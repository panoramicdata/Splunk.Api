using Refit;
using Splunk.Api.Models;
using Splunk.Api.Models.Search;

namespace Splunk.Api.Interfaces;

/// <summary>Search concurrency limits (<c>search/concurrency-settings</c>).</summary>
public interface ISearchConcurrencySettings
{
	/// <summary>Lists the concurrency settings (<c>GET search/concurrency-settings</c>).</summary>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with two entries, <c>scheduler</c> and <c>search</c>.</returns>
	[Get("services/search/concurrency-settings")]
	Task<SplunkFeed<SearchConcurrencySettings>> ListAsync(CancellationToken cancellationToken);

	/// <summary>Changes the scheduler's concurrency limits (<c>POST search/concurrency-settings/scheduler</c>).</summary>
	/// <param name="request">The new percentages.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with the <c>scheduler</c> entry.</returns>
	/// <remarks>Needs the <c>edit_search_concurrency_scheduled</c> capability.</remarks>
	[Post("services/search/concurrency-settings/scheduler")]
	Task<SplunkFeed<SearchConcurrencySettings>> UpdateSchedulerAsync([Body] SchedulerConcurrencyUpdateRequest request, CancellationToken cancellationToken);

	/// <summary>Changes the overall search concurrency limits (<c>POST search/concurrency-settings/search</c>).</summary>
	/// <param name="request">The new limits.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with the <c>search</c> entry.</returns>
	/// <remarks>Needs the <c>edit_search_concurrency_all</c> capability.</remarks>
	[Post("services/search/concurrency-settings/search")]
	Task<SplunkFeed<SearchConcurrencySettings>> UpdateSearchAsync([Body] SearchConcurrencyUpdateRequest request, CancellationToken cancellationToken);
}
