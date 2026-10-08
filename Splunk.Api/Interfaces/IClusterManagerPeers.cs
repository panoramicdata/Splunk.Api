using Refit;
using Splunk.Api.Models;
using Splunk.Api.Models.Cluster;

namespace Splunk.Api.Interfaces;

/// <summary>The peers of the indexer cluster, on the cluster manager (<c>cluster/manager/peers</c>).</summary>
/// <remarks>Call these on the cluster manager; other nodes answer HTTP 503.</remarks>
public interface IClusterManagerPeers
{
	/// <summary>Lists the peers (<c>GET cluster/manager/peers</c>).</summary>
	/// <param name="options">Paging, filtering and sorting, or <see langword="null"/>.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with one entry per peer, named by GUID.</returns>
	[Get("services/cluster/manager/peers")]
	Task<SplunkFeed<ClusterPeer>> ListAsync([Query] ListOptions? options, CancellationToken cancellationToken);

	/// <summary>Gets a peer (<c>GET cluster/manager/peers/{name}</c>).</summary>
	/// <param name="name">The peer GUID.</param>
	/// <param name="options">Whether to list the peer's buckets, or <see langword="null"/>.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with the peer.</returns>
	[Get("services/cluster/manager/peers/{name}")]
	Task<SplunkFeed<ClusterPeer>> GetAsync(string name, [Query] ClusterPeerGetOptions? options, CancellationToken cancellationToken);
}
