using Refit;
using Splunk.Api.Models;
using Splunk.Api.Models.Introspection;

namespace Splunk.Api.Interfaces;

/// <summary>Information about the Splunk server (<c>server/info</c>).</summary>
public interface IServerInfo
{
	/// <summary>Gets the server's version, roles, license state, operating system and health (<c>GET server/info</c>).</summary>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with one entry, <c>server-info</c>.</returns>
	[Get("services/server/info")]
	Task<SplunkFeed<ServerInfo>> GetAsync(CancellationToken cancellationToken);
}
