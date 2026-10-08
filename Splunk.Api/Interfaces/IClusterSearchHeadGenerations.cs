using Refit;
using Splunk.Api.Models;
using Splunk.Api.Models.Cluster;

namespace Splunk.Api.Interfaces;

/// <summary>The generations an indexer cluster search head knows (<c>cluster/searchhead/generation</c>).</summary>
/// <remarks>
/// Call these on a search head of an indexer cluster; other nodes answer HTTP 503 "Search head or cluster manager is not
/// enabled on this node.".
/// </remarks>
public interface IClusterSearchHeadGenerations
{
	/// <summary>Lists the generation and peers of each cluster this search head belongs to (<c>GET cluster/searchhead/generation</c>).</summary>
	/// <param name="options">Paging, filtering and sorting, or <see langword="null"/>.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with one entry per cluster manager.</returns>
	[Get("services/cluster/searchhead/generation")]
	Task<SplunkFeed<ClusterGeneration>> ListAsync([Query] ListOptions? options, CancellationToken cancellationToken);

	/// <summary>Gets the generation of one cluster (<c>GET cluster/searchhead/generation/{name}</c>).</summary>
	/// <remarks>
	/// The name is the manager URI; the reference's example addresses it with its slashes encoded twice
	/// (<c>https%3A%252F%252Fhost%3A8089</c>), so pass <c>https:%2F%2Fhost:8089</c> to send that form.
	/// </remarks>
	/// <param name="name">The cluster manager URI.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with the generation.</returns>
	[Get("services/cluster/searchhead/generation/{name}")]
	Task<SplunkFeed<ClusterGeneration>> GetAsync(string name, CancellationToken cancellationToken);
}
