using Refit;
using Splunk.Api.Models;
using Splunk.Api.Models.Cluster;

namespace Splunk.Api.Interfaces;

/// <summary>The search artifacts the search head cluster captain manages (<c>shcluster/captain/artifacts</c>).</summary>
/// <remarks>Call these on the captain; a node without search head clustering answers HTTP 503.</remarks>
public interface IShClusterCaptainArtifacts
{
	/// <summary>Lists the artifacts and their replicas across the members (<c>GET shcluster/captain/artifacts</c>).</summary>
	/// <param name="options">Whether to include remote searches, paging, filtering and sorting, or <see langword="null"/>.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with one entry per artifact, named by search ID.</returns>
	[Get("services/shcluster/captain/artifacts")]
	Task<SplunkFeed<ShClusterArtifact>> ListAsync([Query] ShClusterArtifactListOptions? options, CancellationToken cancellationToken);

	/// <summary>Gets an artifact (<c>GET shcluster/captain/artifacts/{name}</c>).</summary>
	/// <param name="name">The artifact's search ID.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with the artifact.</returns>
	[Get("services/shcluster/captain/artifacts/{name}")]
	Task<SplunkFeed<ShClusterArtifact>> GetAsync(string name, CancellationToken cancellationToken);
}
