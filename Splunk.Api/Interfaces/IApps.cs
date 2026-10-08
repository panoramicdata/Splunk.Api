using Refit;
using Splunk.Api.Models;
using Splunk.Api.Models.Applications;

namespace Splunk.Api.Interfaces;

/// <summary>
/// Installed apps (<c>apps/local</c>). When <c>enable_install_apps</c> is set in <c>limits.conf</c>, changing apps
/// requires <c>install_apps</c> and <c>edit_local_apps</c>; otherwise <c>admin_all_objects</c>.
/// </summary>
public interface IApps
{
	/// <summary>Lists the installed apps (<c>GET apps/local</c>).</summary>
	/// <param name="options">Paging and filtering, or <see langword="null"/> for Splunk's defaults (the first 30).</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>The apps.</returns>
	[Get("services/apps/local")]
	Task<SplunkFeed<App>> ListAsync([Query] ListOptions? options, CancellationToken cancellationToken);

	/// <summary>Creates an app from a template, or installs one from a file or Splunkbase (<c>POST apps/local</c>).</summary>
	/// <param name="request">The app.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>The created app.</returns>
	[Post("services/apps/local")]
	Task<SplunkFeed<App>> CreateAsync([Body] AppCreateRequest request, CancellationToken cancellationToken);

	/// <summary>Gets one app (<c>GET apps/local/{name}</c>).</summary>
	/// <param name="name">The app name (its folder name).</param>
	/// <param name="options">Whether to reload the app's objects first, or <see langword="null"/>.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with the one app.</returns>
	[Get("services/apps/local/{name}")]
	Task<SplunkFeed<App>> GetAsync(string name, [Query] AppGetOptions? options, CancellationToken cancellationToken);

	/// <summary>Updates an app's properties (<c>POST apps/local/{name}</c>).</summary>
	/// <param name="name">The app name.</param>
	/// <param name="request">The properties to change.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>The updated app.</returns>
	[Post("services/apps/local/{name}")]
	Task<SplunkFeed<App>> UpdateAsync(string name, [Body] AppUpdateRequest request, CancellationToken cancellationToken);

	/// <summary>Uninstalls an app (<c>DELETE apps/local/{name}</c>).</summary>
	/// <param name="name">The app name.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A task that completes when the app is removed.</returns>
	/// <remarks>Splunk may report that a restart is needed. An unknown app raises 404 <c>Could not find object id=...</c>.</remarks>
	[Delete("services/apps/local/{name}")]
	Task DeleteAsync(string name, CancellationToken cancellationToken);

	/// <summary>Gets an app's setup information (<c>GET apps/local/{name}/setup</c>).</summary>
	/// <param name="name">The app name.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with the one app's setup XML.</returns>
	/// <remarks>A disabled app raises 400 <c>Application is disabled: ...</c>.</remarks>
	[Get("services/apps/local/{name}/setup")]
	Task<SplunkFeed<AppSetup>> GetSetupAsync(string name, CancellationToken cancellationToken);

	/// <summary>Checks Splunkbase for an update to an app (<c>GET apps/local/{name}/update</c>).</summary>
	/// <param name="name">The app name.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with the one app; its <c>update.*</c> properties are set only when an update is available.</returns>
	[Get("services/apps/local/{name}/update")]
	Task<SplunkFeed<AppUpdateCheck>> CheckForUpdateAsync(string name, CancellationToken cancellationToken);
}
