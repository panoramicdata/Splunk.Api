using Refit;
using Splunk.Api.Models;
using Splunk.Api.Models.Access;

namespace Splunk.Api.Interfaces;

/// <summary>
/// ProxySSO authentication configurations (<c>admin/ProxySSO-auth</c>). Single sign-on mode must be enabled in
/// <c>web.conf</c> before ProxySSO can be configured. Splunk Enterprise only.
/// </summary>
public interface IProxySsoConfigurations
{
	/// <summary>Lists the ProxySSO configurations (<c>GET admin/ProxySSO-auth</c>).</summary>
	/// <param name="options">Paging and filtering, or <see langword="null"/> for Splunk's defaults.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>The configurations.</returns>
	[Get("services/admin/ProxySSO-auth")]
	Task<SplunkFeed<ProxySsoConfiguration>> ListAsync([Query] ListOptions? options, CancellationToken cancellationToken);

	/// <summary>Creates a ProxySSO configuration (<c>POST admin/ProxySSO-auth</c>).</summary>
	/// <param name="request">The configuration.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>The created configuration.</returns>
	[Post("services/admin/ProxySSO-auth")]
	Task<SplunkFeed<ProxySsoConfiguration>> CreateAsync([Body] ProxySsoConfigurationCreateRequest request, CancellationToken cancellationToken);

	/// <summary>Gets one ProxySSO configuration (<c>GET admin/ProxySSO-auth/{proxy_name}</c>).</summary>
	/// <param name="name">The configuration name.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with the one configuration.</returns>
	[Get("services/admin/ProxySSO-auth/{name}")]
	Task<SplunkFeed<ProxySsoConfiguration>> GetAsync(string name, CancellationToken cancellationToken);

	/// <summary>Updates a ProxySSO configuration (<c>POST admin/ProxySSO-auth/{proxy_name}</c>).</summary>
	/// <param name="name">The configuration name.</param>
	/// <param name="request">The settings to change.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>The updated configuration.</returns>
	[Post("services/admin/ProxySSO-auth/{name}")]
	Task<SplunkFeed<ProxySsoConfiguration>> UpdateAsync(string name, [Body] ProxySsoConfigurationUpdateRequest request, CancellationToken cancellationToken);

	/// <summary>Deletes a ProxySSO configuration (<c>DELETE admin/ProxySSO-auth/{proxy_name}</c>).</summary>
	/// <param name="name">The configuration name.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A task that completes when the configuration is deleted.</returns>
	[Delete("services/admin/ProxySSO-auth/{name}")]
	Task DeleteAsync(string name, CancellationToken cancellationToken);

	/// <summary>Disables a ProxySSO configuration (<c>GET admin/ProxySSO-auth/{proxy_name}/disable</c>).</summary>
	/// <param name="name">The configuration name.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A task that completes when the configuration is disabled.</returns>
	/// <remarks>Splunk changes state on this GET, as the reference documents.</remarks>
	[Get("services/admin/ProxySSO-auth/{name}/disable")]
	Task DisableAsync(string name, CancellationToken cancellationToken);

	/// <summary>
	/// Enables a ProxySSO configuration, creating it if it does not exist (<c>GET admin/ProxySSO-auth/{proxy_name}/enable</c>).
	/// </summary>
	/// <param name="name">The configuration name.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A task that completes when the configuration is enabled.</returns>
	/// <remarks>
	/// Splunk changes state on this GET, as the reference documents. Enabling ProxySSO changes how every user signs in.
	/// </remarks>
	[Get("services/admin/ProxySSO-auth/{name}/enable")]
	Task EnableAsync(string name, CancellationToken cancellationToken);
}
