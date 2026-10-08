using Refit;
using Splunk.Api.Models;
using Splunk.Api.Models.Server;

namespace Splunk.Api.Interfaces;

/// <summary>The server's general settings (<c>server/settings</c>).</summary>
public interface IServerSettings
{
	/// <summary>Gets the server settings (<c>GET server/settings</c>).</summary>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with one entry, <c>settings</c>.</returns>
	[Get("services/server/settings")]
	Task<SplunkFeed<ServerSettings>> GetAsync(CancellationToken cancellationToken);
}
