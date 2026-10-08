using Refit;
using Splunk.Api.Models;
using Splunk.Api.Models.Cluster;

namespace Splunk.Api.Interfaces;

/// <summary>The indexer clusters this search head belongs to (<c>cluster/searchhead/searchheadconfig</c>).</summary>
/// <remarks>
/// Call these on a search head of an indexer cluster; other nodes answer HTTP 503 "Searchhead is not enabled on this
/// node". Entries are named by manager URI; the reference's examples address them with the slashes encoded twice
/// (<c>https%3A%252F%252Fhost%3A8089</c>), so pass <c>https:%2F%2Fhost:8089</c> to send that form.
/// </remarks>
public interface IClusterSearchHeadConfigs
{
	/// <summary>Lists the clusters this search head belongs to (<c>GET cluster/searchhead/searchheadconfig</c>).</summary>
	/// <param name="options">Paging, filtering and sorting, or <see langword="null"/>.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with one entry per cluster manager.</returns>
	[Get("services/cluster/searchhead/searchheadconfig")]
	Task<SplunkFeed<ClusterSearchHeadConfig>> ListAsync([Query] ListOptions? options, CancellationToken cancellationToken);

	/// <summary>Makes this server a search head of a cluster (<c>POST cluster/searchhead/searchheadconfig</c>).</summary>
	/// <param name="request">The cluster manager and secret.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>The feed Splunk answers with.</returns>
	[Post("services/cluster/searchhead/searchheadconfig")]
	Task<SplunkFeed<ClusterSearchHeadConfig>> CreateAsync([Body] ClusterSearchHeadConfigCreateRequest request, CancellationToken cancellationToken);

	/// <summary>Gets one cluster this search head belongs to (<c>GET cluster/searchhead/searchheadconfig/{name}</c>).</summary>
	/// <param name="name">The cluster manager URI.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with the cluster.</returns>
	[Get("services/cluster/searchhead/searchheadconfig/{name}")]
	Task<SplunkFeed<ClusterSearchHeadConfig>> GetAsync(string name, CancellationToken cancellationToken);

	/// <summary>Changes the search head's configuration for one cluster (<c>POST cluster/searchhead/searchheadconfig/{name}</c>).</summary>
	/// <param name="name">The cluster manager URI.</param>
	/// <param name="request">The settings to change.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>The feed Splunk answers with.</returns>
	[Post("services/cluster/searchhead/searchheadconfig/{name}")]
	Task<SplunkFeed<ClusterSearchHeadConfig>> UpdateAsync(string name, [Body] ClusterSearchHeadConfigUpdateRequest request, CancellationToken cancellationToken);

	/// <summary>Removes this search head from a cluster (<c>DELETE cluster/searchhead/searchheadconfig/{name}</c>).</summary>
	/// <param name="name">The cluster manager URI.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A task that completes when the search head has left the cluster.</returns>
	[Delete("services/cluster/searchhead/searchheadconfig/{name}")]
	Task DeleteAsync(string name, CancellationToken cancellationToken);
}
