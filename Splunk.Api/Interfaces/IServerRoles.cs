using Refit;
using Splunk.Api.Models;
using Splunk.Api.Models.Server;

namespace Splunk.Api.Interfaces;

/// <summary>The roles the server plays (<c>server/roles</c>).</summary>
public interface IServerRoles
{
	/// <summary>Gets the server's roles (<c>GET server/roles</c>).</summary>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with one entry, <c>result</c>.</returns>
	[Get("services/server/roles")]
	Task<SplunkFeed<ServerRoles>> GetAsync(CancellationToken cancellationToken);
}
