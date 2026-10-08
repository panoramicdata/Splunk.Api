using Refit;
using Splunk.Api.Models;
using Splunk.Api.Models.Search;

namespace Splunk.Api.Interfaces;

/// <summary>The search scheduler's state (<c>search/scheduler</c>).</summary>
public interface ISearchScheduler
{
	/// <summary>Gets whether the scheduler runs scheduled searches (<c>GET search/scheduler</c>).</summary>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with one entry, <c>status</c>.</returns>
	[Get("services/search/scheduler")]
	Task<SplunkFeed<SearchSchedulerStatus>> GetStatusAsync(CancellationToken cancellationToken);

	/// <summary>Enables or disables the search scheduler (<c>POST search/scheduler/status</c>).</summary>
	/// <param name="request">Whether to disable it.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>An empty feed; read the state again with <see cref="GetStatusAsync"/>.</returns>
	/// <remarks>Disabling stops every scheduled search and alert on the instance until it is enabled again.</remarks>
	[Post("services/search/scheduler/status")]
	Task<SplunkFeed<SearchSchedulerStatus>> SetStatusAsync([Body] SearchSchedulerStatusRequest request, CancellationToken cancellationToken);
}
