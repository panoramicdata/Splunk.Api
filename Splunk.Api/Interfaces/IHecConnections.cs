using Refit;
using Splunk.Api.Models;
using Splunk.Api.Models.Inputs;

namespace Splunk.Api.Interfaces;

/// <summary>
/// Recent HTTP Event Collector senders (<c>data/inputs/http/connections</c>). Requires the <c>list_inputs</c>
/// capability. Only senders still in the collector's cache (<c>hecCacheCapacity</c>) are listed.
/// </summary>
public interface IHecConnections
{
	/// <summary>Lists recent senders and when each last sent an event (<c>GET data/inputs/http/connections</c>).</summary>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with one entry per sender IP address.</returns>
	[Get("services/data/inputs/http/connections")]
	Task<SplunkFeed<HecConnection>> ListAsync(CancellationToken cancellationToken);

	/// <summary>Gets one sender (<c>GET data/inputs/http/connections/{ip_address}</c>); 404 when it is not in the cache.</summary>
	/// <param name="ipAddress">The sender's IP address.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with the sender.</returns>
	[Get("services/data/inputs/http/connections/{ipAddress}")]
	Task<SplunkFeed<HecConnection>> GetAsync(string ipAddress, CancellationToken cancellationToken);
}
