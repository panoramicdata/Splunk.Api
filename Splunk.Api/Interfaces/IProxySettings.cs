using Refit;
using Splunk.Api.Models;
using Splunk.Api.Models.Server;

namespace Splunk.Api.Interfaces;

/// <summary>
/// splunkd's outbound HTTP proxy (<c>server/httpsettings/proxysettings</c>). There is one configuration, <c>proxyConfig</c>.
/// Every operation needs the <c>edit_server</c> capability.
/// </summary>
public interface IProxySettings
{
	/// <summary>Creates the proxy configuration (<c>POST server/httpsettings/proxysettings</c>).</summary>
	/// <param name="request">The proxy settings; the name is always <c>proxyConfig</c>.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with the new configuration.</returns>
	[Post("services/server/httpsettings/proxysettings")]
	Task<SplunkFeed<ProxySettings>> CreateAsync([Body] ProxySettingsCreateRequest request, CancellationToken cancellationToken);

	/// <summary>Gets the proxy configuration (<c>GET server/httpsettings/proxysettings/proxyConfig</c>).</summary>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with the configuration.</returns>
	[Get("services/server/httpsettings/proxysettings/proxyConfig")]
	Task<SplunkFeed<ProxySettings>> GetAsync(CancellationToken cancellationToken);

	/// <summary>Changes the proxy configuration (<c>POST server/httpsettings/proxysettings/proxyConfig</c>).</summary>
	/// <param name="request">The settings to change.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with the updated configuration.</returns>
	[Post("services/server/httpsettings/proxysettings/proxyConfig")]
	Task<SplunkFeed<ProxySettings>> UpdateAsync([Body] ProxySettingsUpdateRequest request, CancellationToken cancellationToken);

	/// <summary>Deletes the proxy configuration (<c>DELETE server/httpsettings/proxysettings/proxyConfig</c>).</summary>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A task that completes when the configuration is deleted.</returns>
	[Delete("services/server/httpsettings/proxysettings/proxyConfig")]
	Task DeleteAsync(CancellationToken cancellationToken);
}
