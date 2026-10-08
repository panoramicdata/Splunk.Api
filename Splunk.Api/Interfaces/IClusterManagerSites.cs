using Refit;
using Splunk.Api.Models;
using Splunk.Api.Models.Cluster;

namespace Splunk.Api.Interfaces;

/// <summary>The sites of a multisite indexer cluster, on the cluster manager (<c>cluster/manager/sites</c>).</summary>
/// <remarks>Call these on the cluster manager; other nodes answer HTTP 503.</remarks>
public interface IClusterManagerSites
{
	/// <summary>Lists the sites and their peers (<c>GET cluster/manager/sites</c>).</summary>
	/// <param name="options">Paging, filtering and sorting, or <see langword="null"/>.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with one entry per site.</returns>
	[Get("services/cluster/manager/sites")]
	Task<SplunkFeed<ClusterSite>> ListAsync([Query] ListOptions? options, CancellationToken cancellationToken);

	/// <summary>Gets a site (<c>GET cluster/manager/sites/{name}</c>).</summary>
	/// <param name="name">The site, for example <c>site1</c>.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with the site.</returns>
	[Get("services/cluster/manager/sites/{name}")]
	Task<SplunkFeed<ClusterSite>> GetAsync(string name, CancellationToken cancellationToken);
}
