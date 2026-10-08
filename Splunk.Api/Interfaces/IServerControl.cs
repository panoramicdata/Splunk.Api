using Refit;
using Splunk.Api.Models;

namespace Splunk.Api.Interfaces;

/// <summary>Server control actions such as restarting splunkd (<c>server/control</c>).</summary>
public interface IServerControl
{
	/// <summary>Lists the available control actions (<c>GET server/control</c>).</summary>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with no entries whose <see cref="SplunkFeed{T}.Links"/> name each action, for example <c>restart</c>.</returns>
	[Get("services/server/control")]
	Task<SplunkFeed<SplunkDynamicContent>> ListAsync(CancellationToken cancellationToken);

	/// <summary>Restarts splunkd and Splunk Web (<c>POST server/control/restart</c>).</summary>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A task that completes when Splunk accepts the request.</returns>
	/// <remarks>
	/// Needs the <c>restart_splunkd</c> capability. The server goes down shortly after answering; requests fail until it is
	/// back, and session keys from before the restart are no longer valid.
	/// </remarks>
	[Post("services/server/control/restart")]
	Task RestartAsync(CancellationToken cancellationToken);

	/// <summary>Restarts Splunk Web only (<c>POST server/control/restart_webui</c>).</summary>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A task that completes when Splunk accepts the request.</returns>
	[Post("services/server/control/restart_webui")]
	Task RestartWebUIAsync(CancellationToken cancellationToken);
}
