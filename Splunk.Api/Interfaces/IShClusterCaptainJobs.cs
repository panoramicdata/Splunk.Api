using Refit;
using Splunk.Api.Models;
using Splunk.Api.Models.Cluster;

namespace Splunk.Api.Interfaces;

/// <summary>The scheduled jobs the search head cluster captain dispatches (<c>shcluster/captain/jobs</c>).</summary>
/// <remarks>Call these on the captain; a node without search head clustering answers HTTP 503.</remarks>
public interface IShClusterCaptainJobs
{
	/// <summary>Lists the running and recently finished jobs of every member (<c>GET shcluster/captain/jobs</c>).</summary>
	/// <param name="options">Paging, filtering and sorting, or <see langword="null"/>.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with one entry per job.</returns>
	[Get("services/shcluster/captain/jobs")]
	Task<SplunkFeed<ShClusterJob>> ListAsync([Query] ListOptions? options, CancellationToken cancellationToken);

	/// <summary>Gets a job (<c>GET shcluster/captain/jobs/{name}</c>).</summary>
	/// <param name="name">The job name.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with the job.</returns>
	[Get("services/shcluster/captain/jobs/{name}")]
	Task<SplunkFeed<ShClusterJob>> GetAsync(string name, CancellationToken cancellationToken);
}
