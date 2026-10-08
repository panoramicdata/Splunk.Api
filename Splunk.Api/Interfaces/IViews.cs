using Refit;
using Splunk.Api.Models;
using Splunk.Api.Models.Knowledge;

namespace Splunk.Api.Interfaces;

/// <summary>Views and dashboards (<c>data/ui/views</c>).</summary>
/// <remarks>Views belong to an app: use a namespace (<see cref="SplunkClient.InNamespace(string, string)"/>), as the reference's <c>servicesNS/{user}/{app}</c> URL does.</remarks>
public interface IViews
{
	/// <summary>Lists views (<c>GET data/ui/views</c>).</summary>
	/// <param name="options">Paging and filtering, for example <c>Search = "isDashboard=1"</c>; <see langword="null"/> for Splunk's defaults.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with one entry per view.</returns>
	[Get("services/data/ui/views")]
	Task<SplunkFeed<View>> ListAsync([Query] ListOptions? options, CancellationToken cancellationToken);

	/// <summary>Creates a view (<c>POST data/ui/views</c>).</summary>
	/// <param name="request">The view name and source.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with the new view.</returns>
	[Post("services/data/ui/views")]
	Task<SplunkFeed<View>> CreateAsync([Body] ViewCreateRequest request, CancellationToken cancellationToken);

	/// <summary>Gets one view (<c>GET data/ui/views/{name}</c>).</summary>
	/// <param name="name">The view name.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with one entry.</returns>
	[Get("services/data/ui/views/{name}")]
	Task<SplunkFeed<View>> GetAsync(string name, CancellationToken cancellationToken);

	/// <summary>Replaces a view's source, recording a revision (<c>POST data/ui/views/{name}</c>).</summary>
	/// <param name="name">The view name.</param>
	/// <param name="request">The new source and an optional change message.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with the updated view.</returns>
	[Post("services/data/ui/views/{name}")]
	Task<SplunkFeed<View>> UpdateAsync(string name, [Body] ViewUpdateRequest request, CancellationToken cancellationToken);

	/// <summary>Deletes a view (<c>DELETE data/ui/views/{name}</c>).</summary>
	/// <param name="name">The view name.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A task that completes when the view is deleted.</returns>
	[Delete("services/data/ui/views/{name}")]
	Task DeleteAsync(string name, CancellationToken cancellationToken);

	/// <summary>Deletes a view with a change message (<c>DELETE data/ui/views/{name}</c> with <c>eai:changelog</c>).</summary>
	/// <param name="name">The view name.</param>
	/// <param name="request">The change message, sent as a form body.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A task that completes when the view is deleted.</returns>
	[Delete("services/data/ui/views/{name}")]
	Task DeleteAsync(string name, [Body] ViewDeleteRequest request, CancellationToken cancellationToken);

	/// <summary>Deactivates a dashboard, blocking access to it in Splunk Web (<c>POST data/ui/views/{dashboard_id}/disable</c>).</summary>
	/// <param name="name">The view name.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with the view, now <see cref="SplunkContent.Disabled"/>.</returns>
	/// <remarks>Requires the <c>deactivate_dashboards</c> capability. Splunk records the state in the source as <c>dashboard.disabled="true"</c>.</remarks>
	[Post("services/data/ui/views/{name}/disable")]
	Task<SplunkFeed<View>> DisableAsync(string name, CancellationToken cancellationToken);

	/// <summary>Reactivates a deactivated dashboard (<c>POST data/ui/views/{dashboard_id}/enable</c>).</summary>
	/// <param name="name">The view name.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with the view.</returns>
	/// <remarks>Requires the <c>deactivate_dashboards</c> capability.</remarks>
	[Post("services/data/ui/views/{name}/enable")]
	Task<SplunkFeed<View>> EnableAsync(string name, CancellationToken cancellationToken);

	/// <summary>Lists a view's revisions, newest first (<c>GET data/ui/views/{name}/history</c>).</summary>
	/// <param name="name">The view name.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with one entry per revision, named by position (<c>0000000</c> is the latest).</returns>
	[Get("services/data/ui/views/{name}/history")]
	Task<SplunkFeed<ViewRevision>> ListHistoryAsync(string name, CancellationToken cancellationToken);

	/// <summary>Lists only the revisions recorded with a change message (<c>GET data/ui/views/{name}/history?with_message=</c>).</summary>
	/// <param name="name">The view name.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with one entry per revision that has a message.</returns>
	/// <remarks><c>with_message</c> is a flag: Splunk 10.6 refuses any value ("Parameter 'with_message' must be empty").</remarks>
	[Get("services/data/ui/views/{name}/history?with_message=")]
	Task<SplunkFeed<ViewRevision>> ListHistoryWithMessagesAsync(string name, CancellationToken cancellationToken);

	/// <summary>Gets a view's source at one revision (<c>GET data/ui/views/{name}/revision</c>).</summary>
	/// <param name="name">The view name.</param>
	/// <param name="revisionId">The revision's <see cref="ViewRevision.Sha"/> from <see cref="ListHistoryAsync"/>.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with one entry, <c>revision</c>, including the source.</returns>
	/// <remarks>The reference's example sends <c>revision_id</c> as a form body on a GET; Splunk 10.6 reads it from the query string.</remarks>
	[Get("services/data/ui/views/{name}/revision")]
	Task<SplunkFeed<ViewRevision>> GetRevisionAsync(string name, [AliasAs("revision_id")] string revisionId, CancellationToken cancellationToken);
}
