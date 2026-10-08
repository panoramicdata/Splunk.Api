using Refit;
using Splunk.Api.Models;
using Splunk.Api.Models.Search;

namespace Splunk.Api.Interfaces;

/// <summary>
/// Dashboards scheduled for PDF delivery (<c>scheduled/views</c>). A view's schedule is named
/// <c>_ScheduledView__&lt;view name&gt;</c>; reading one that has never been scheduled returns an unscheduled entry.
/// </summary>
public interface IScheduledViews
{
	/// <summary>Lists the scheduled views (<c>GET scheduled/views</c>).</summary>
	/// <param name="options">Paging and filtering; <see langword="null"/> for Splunk's defaults.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed of scheduled views.</returns>
	[Get("services/scheduled/views")]
	Task<SplunkFeed<ScheduledView>> ListAsync([Query] ListOptions? options, CancellationToken cancellationToken);

	/// <summary>Gets a view's delivery schedule (<c>GET scheduled/views/{name}</c>).</summary>
	/// <param name="name">The name, <c>_ScheduledView__&lt;view name&gt;</c>.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with one entry.</returns>
	[Get("services/scheduled/views/{name}")]
	Task<SplunkFeed<ScheduledView>> GetAsync(string name, CancellationToken cancellationToken);

	/// <summary>Schedules a view's delivery, or changes it (<c>POST scheduled/views/{name}</c>).</summary>
	/// <param name="name">The name, <c>_ScheduledView__&lt;view name&gt;</c>.</param>
	/// <param name="request">The schedule and email settings.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with one entry, the schedule.</returns>
	[Post("services/scheduled/views/{name}")]
	Task<SplunkFeed<ScheduledView>> UpdateAsync(string name, [Body] ScheduledViewUpdateRequest request, CancellationToken cancellationToken);

	/// <summary>Removes a view's delivery schedule (<c>DELETE scheduled/views/{name}</c>); the view itself remains.</summary>
	/// <param name="name">The name, <c>_ScheduledView__&lt;view name&gt;</c>.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A task that completes when the schedule is removed.</returns>
	[Delete("services/scheduled/views/{name}")]
	Task DeleteAsync(string name, CancellationToken cancellationToken);

	/// <summary>Runs a view's delivery search now (<c>POST scheduled/views/{name}/dispatch</c>).</summary>
	/// <param name="name">The name, <c>_ScheduledView__&lt;view name&gt;</c>.</param>
	/// <param name="request">Overrides for this run.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>The new job's search ID.</returns>
	[Post("services/scheduled/views/{name}/dispatch")]
	Task<SearchJobCreated> DispatchAsync(string name, [Body] ScheduledViewDispatchRequest request, CancellationToken cancellationToken);

	/// <summary>Lists the jobs that rendered a view (<c>GET scheduled/views/{name}/history</c>).</summary>
	/// <param name="name">The name, <c>_ScheduledView__&lt;view name&gt;</c>.</param>
	/// <param name="options">Paging and filtering; <see langword="null"/> for Splunk's defaults.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed of jobs, each named by its search ID.</returns>
	[Get("services/scheduled/views/{name}/history")]
	Task<SplunkFeed<SavedSearchHistoryEntry>> GetHistoryAsync(string name, [Query] ListOptions? options, CancellationToken cancellationToken);

	/// <summary>Makes a view's delivery run next at a given time, then on its schedule (<c>POST scheduled/views/{name}/reschedule</c>).</summary>
	/// <param name="name">The name, <c>_ScheduledView__&lt;view name&gt;</c>.</param>
	/// <param name="request">The time of the next run.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>An empty feed.</returns>
	[Post("services/scheduled/views/{name}/reschedule")]
	Task<SplunkFeed<ScheduledView>> RescheduleAsync(string name, [Body] RescheduleRequest request, CancellationToken cancellationToken);

	/// <summary>Lists when a view's delivery will run in a time range (<c>GET scheduled/views/{name}/scheduled_times</c>).</summary>
	/// <param name="name">The name, <c>_ScheduledView__&lt;view name&gt;</c>.</param>
	/// <param name="earliestTime">The start of the range (<c>earliest_time</c>).</param>
	/// <param name="latestTime">The end of the range (<c>latest_time</c>).</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with the schedule.</returns>
	/// <remarks>
	/// Unlike <see cref="ISavedSearches.GetScheduledTimesAsync"/>, Splunk 10.6 returns the schedule here without a
	/// <c>scheduled_times</c> list, so <see cref="ScheduledView.ScheduledTimes"/> is empty; work the times out from
	/// <see cref="ScheduledView.CronSchedule"/>.
	/// </remarks>
	[Get("services/scheduled/views/{name}/scheduled_times")]
	Task<SplunkFeed<ScheduledView>> GetScheduledTimesAsync(
		string name,
		[AliasAs("earliest_time")] string earliestTime,
		[AliasAs("latest_time")] string latestTime,
		CancellationToken cancellationToken);
}
