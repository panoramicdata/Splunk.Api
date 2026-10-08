using Refit;
using Splunk.Api.Models;
using Splunk.Api.Models.Deployment;

namespace Splunk.Api.Interfaces;

/// <summary>Distributed search settings and search peers (<c>search/distributed/config</c> and <c>search/distributed/peers</c>).</summary>
public interface IDistributedSearch
{
	/// <summary>Gets the distributed search settings (<c>GET search/distributed/config</c>).</summary>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with one entry, <c>distributedSearch</c>.</returns>
	[Get("services/search/distributed/config")]
	Task<SplunkFeed<DistributedSearchConfig>> GetConfigAsync(CancellationToken cancellationToken);

	/// <summary>Lists the search peers this search head distributes searches to, enabled or not (<c>GET search/distributed/peers</c>).</summary>
	/// <param name="options">Paging, filtering and sorting, or <see langword="null"/>.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with one entry per peer, named <c>host:port</c>.</returns>
	[Get("services/search/distributed/peers")]
	Task<SplunkFeed<DistributedPeer>> ListPeersAsync([Query] ListOptions? options, CancellationToken cancellationToken);

	/// <summary>Adds a search peer (<c>POST search/distributed/peers</c>).</summary>
	/// <remarks>
	/// Distributed search must be enabled. Splunk logs in to the peer with the given credentials to exchange keys, and
	/// every later search of this search head is sent to the peer.
	/// </remarks>
	/// <param name="request">The peer and an admin login on it.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>The feed Splunk answers with.</returns>
	[Post("services/search/distributed/peers")]
	Task<SplunkFeed<DistributedPeer>> AddPeerAsync([Body] DistributedPeerCreateRequest request, CancellationToken cancellationToken);

	/// <summary>Changes the login used for a search peer (<c>POST search/distributed/peers/{name}</c>).</summary>
	/// <remarks>The reference documents only POST here, and its URL block shows the collection path.</remarks>
	/// <param name="name">The peer, as <c>host:port</c>.</param>
	/// <param name="request">An admin login on the peer.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>The feed Splunk answers with.</returns>
	[Post("services/search/distributed/peers/{name}")]
	Task<SplunkFeed<DistributedPeer>> UpdatePeerAsync(string name, [Body] DistributedPeerUpdateRequest request, CancellationToken cancellationToken);
}
