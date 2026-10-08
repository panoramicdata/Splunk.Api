using Refit;
using Splunk.Api.Models;
using Splunk.Api.Models.Cluster;

namespace Splunk.Api.Interfaces;

/// <summary>Cluster generations, on the cluster manager (<c>cluster/manager/generation</c>).</summary>
/// <remarks>Call these on the cluster manager; other nodes answer HTTP 503.</remarks>
public interface IClusterManagerGenerations
{
	/// <summary>Lists the peers in the current generation (<c>GET cluster/manager/generation</c>).</summary>
	/// <param name="options">Paging, filtering and sorting, or <see langword="null"/>.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with one entry, <c>manager</c>.</returns>
	[Get("services/cluster/manager/generation")]
	Task<SplunkFeed<ClusterGeneration>> ListAsync([Query] ListOptions? options, CancellationToken cancellationToken);

	/// <summary>Creates a cluster generation for a search head (<c>POST cluster/manager/generation</c>).</summary>
	/// <param name="request">The search head.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with the generation.</returns>
	[Post("services/cluster/manager/generation")]
	Task<SplunkFeed<ClusterGeneration>> CreateAsync([Body] ClusterGenerationCreateRequest request, CancellationToken cancellationToken);

	/// <summary>Gets the generation of a search head (<c>GET cluster/manager/generation/{name}</c>).</summary>
	/// <param name="name">The search head GUID, or <c>manager</c>.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with the generation.</returns>
	[Get("services/cluster/manager/generation/{name}")]
	Task<SplunkFeed<ClusterGeneration>> GetAsync(string name, CancellationToken cancellationToken);

	/// <summary>Creates a new generation for a search head (<c>POST cluster/manager/generation/{name}</c>).</summary>
	/// <param name="name">The search head GUID.</param>
	/// <param name="request">The search head's settings.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with the generation.</returns>
	[Post("services/cluster/manager/generation/{name}")]
	Task<SplunkFeed<ClusterGeneration>> UpdateAsync(string name, [Body] ClusterGenerationUpdateRequest request, CancellationToken cancellationToken);
}
