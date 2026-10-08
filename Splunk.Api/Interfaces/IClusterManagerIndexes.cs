using Refit;
using Splunk.Api.Models;
using Splunk.Api.Models.Cluster;

namespace Splunk.Api.Interfaces;

/// <summary>Clustered indexes, on the cluster manager (<c>cluster/manager/indexes</c>).</summary>
/// <remarks>Call these on the cluster manager; other nodes answer HTTP 503.</remarks>
public interface IClusterManagerIndexes
{
	/// <summary>Lists the clustered indexes and their copy counts (<c>GET cluster/manager/indexes</c>).</summary>
	/// <param name="options">Paging, filtering and sorting, or <see langword="null"/>.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with one entry per index.</returns>
	[Get("services/cluster/manager/indexes")]
	Task<SplunkFeed<ClusterIndex>> ListAsync([Query] ListOptions? options, CancellationToken cancellationToken);

	/// <summary>Gets a clustered index (<c>GET cluster/manager/indexes/{name}</c>).</summary>
	/// <param name="name">The index name.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with the index.</returns>
	[Get("services/cluster/manager/indexes/{name}")]
	Task<SplunkFeed<ClusterIndex>> GetAsync(string name, CancellationToken cancellationToken);
}
