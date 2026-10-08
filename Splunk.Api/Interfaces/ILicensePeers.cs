using Refit;
using Splunk.Api.Models;
using Splunk.Api.Models.Licensing;

namespace Splunk.Api.Interfaces;

/// <summary>The license peers registered with this license manager (<c>licenser/peers</c>).</summary>
public interface ILicensePeers
{
	/// <summary>Lists the license peers (<c>GET licenser/peers</c>).</summary>
	/// <param name="options">Paging and filtering, or <see langword="null"/> for Splunk's defaults.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>The peers, named by GUID; every peer that has contacted this manager, whether allocated to a pool or not.</returns>
	[Get("services/licenser/peers")]
	Task<SplunkFeed<LicensePeer>> ListAsync([Query] ListOptions? options, CancellationToken cancellationToken);

	/// <summary>Gets one license peer (<c>GET licenser/peers/{name}</c>).</summary>
	/// <param name="name">The peer's GUID.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with the one peer.</returns>
	[Get("services/licenser/peers/{name}")]
	Task<SplunkFeed<LicensePeer>> GetAsync(string name, CancellationToken cancellationToken);
}
