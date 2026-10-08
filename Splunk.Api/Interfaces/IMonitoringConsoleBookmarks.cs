using Refit;
using Splunk.Api.Models;
using Splunk.Api.Models.Knowledge;

namespace Splunk.Api.Interfaces;

/// <summary>Bookmarks to the monitoring consoles of other deployments (<c>saved/bookmarks/monitoring_console</c>).</summary>
public interface IMonitoringConsoleBookmarks
{
	/// <summary>Lists monitoring console bookmarks (<c>GET saved/bookmarks/monitoring_console</c>).</summary>
	/// <param name="options">Paging, filtering and sorting; <see langword="null"/> for Splunk's defaults.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with one entry per bookmark.</returns>
	[Get("services/saved/bookmarks/monitoring_console")]
	Task<SplunkFeed<MonitoringConsoleBookmark>> ListAsync([Query] ListOptions? options, CancellationToken cancellationToken);

	/// <summary>Adds a monitoring console bookmark (<c>POST saved/bookmarks/monitoring_console</c>).</summary>
	/// <param name="request">The bookmark name and URL.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with the new bookmark.</returns>
	[Post("services/saved/bookmarks/monitoring_console")]
	Task<SplunkFeed<MonitoringConsoleBookmark>> CreateAsync([Body] MonitoringConsoleBookmarkCreateRequest request, CancellationToken cancellationToken);

	/// <summary>Removes a monitoring console bookmark (<c>DELETE saved/bookmarks/monitoring_console/{name}</c>).</summary>
	/// <param name="name">The bookmark name.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A task that completes when the bookmark is removed.</returns>
	/// <remarks>The reference lists DELETE on the collection path, but its own example and Splunk 10.6 need the bookmark name.</remarks>
	[Delete("services/saved/bookmarks/monitoring_console/{name}")]
	Task DeleteAsync(string name, CancellationToken cancellationToken);
}
