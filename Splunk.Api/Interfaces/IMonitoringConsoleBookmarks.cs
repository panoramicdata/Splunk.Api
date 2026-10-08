using Refit;
using Splunk.Api.Models;
using Splunk.Api.Models.Introspection;

namespace Splunk.Api.Interfaces;

/// <summary>Links to the monitoring consoles of other deployments (<c>saved/bookmarks/monitoring_console</c>).</summary>
public interface IMonitoringConsoleBookmarks
{
	/// <summary>Lists the bookmarks (<c>GET saved/bookmarks/monitoring_console</c>).</summary>
	/// <param name="options">Paging, filtering and sorting, or <see langword="null"/> for Splunk's defaults.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with one entry per bookmark.</returns>
	[Get("services/saved/bookmarks/monitoring_console")]
	Task<SplunkFeed<MonitoringConsoleBookmark>> ListAsync([Query] ListOptions? options, CancellationToken cancellationToken);

	/// <summary>Adds a bookmark (<c>POST saved/bookmarks/monitoring_console</c>).</summary>
	/// <param name="request">The name and URL.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with the new bookmark.</returns>
	[Post("services/saved/bookmarks/monitoring_console")]
	Task<SplunkFeed<MonitoringConsoleBookmark>> CreateAsync([Body] MonitoringConsoleBookmarkCreateRequest request, CancellationToken cancellationToken);

	/// <summary>Deletes a bookmark (<c>DELETE saved/bookmarks/monitoring_console/{name}</c>).</summary>
	/// <param name="name">The bookmark name.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A task that completes when the bookmark is deleted.</returns>
	/// <remarks>
	/// The reference lists this as <c>DELETE saved/bookmarks/monitoring_console</c>; Splunk 10.6 answers that with "Cannot
	/// perform action DELETE without a target name", so the bookmark is addressed by name in the path.
	/// </remarks>
	[Delete("services/saved/bookmarks/monitoring_console/{name}")]
	Task DeleteAsync(string name, CancellationToken cancellationToken);
}
