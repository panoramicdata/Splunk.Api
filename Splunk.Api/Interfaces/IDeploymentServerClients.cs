using Refit;
using Splunk.Api.Models;
using Splunk.Api.Models.Deployment;

namespace Splunk.Api.Interfaces;

/// <summary>The deployment clients a deployment server knows (<c>deployment/server/clients</c>).</summary>
public interface IDeploymentServerClients
{
	/// <summary>Lists the deployment clients (<c>GET deployment/server/clients</c>).</summary>
	/// <param name="options">Filters, paging and sorting, or <see langword="null"/>.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with one entry per client.</returns>
	[Get("services/deployment/server/clients")]
	Task<SplunkFeed<DeploymentServerClient>> ListAsync([Query] DeploymentServerClientListOptions? options, CancellationToken cancellationToken);

	/// <summary>
	/// Counts the deployment clients by machine type (<c>GET deployment/server/clients/countClients_by_machineType</c>).
	/// </summary>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with one entry, <c>default</c>.</returns>
	[Get("services/deployment/server/clients/countClients_by_machineType")]
	Task<SplunkFeed<DeploymentMachineTypeCounts>> CountByMachineTypeAsync(CancellationToken cancellationToken);

	/// <summary>
	/// Counts the app downloads from the deployment server in a recent period
	/// (<c>GET deployment/server/clients/countRecentDownloads</c>).
	/// </summary>
	/// <param name="maxAgeSeconds">The period to count, in seconds back from now (<c>maxAgeSecs</c>).</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with one entry, <c>default</c>.</returns>
	[Get("services/deployment/server/clients/countRecentDownloads")]
	Task<SplunkFeed<DeploymentDownloadCount>> CountRecentDownloadsAsync([AliasAs("maxAgeSecs")] int maxAgeSeconds, CancellationToken cancellationToken);

	/// <summary>Gets a deployment client (<c>GET deployment/server/clients/{name}</c>).</summary>
	/// <remarks>
	/// Splunk 10.6 checks the filters before the name: an <see cref="DeploymentServerClientFilter.Application"/> no server
	/// class uses raises HTTP 500 "Bad client selector".
	/// </remarks>
	/// <param name="name">The client's identifier (its <c>guid</c>).</param>
	/// <param name="filter">Filters, or <see langword="null"/>.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with the client.</returns>
	[Get("services/deployment/server/clients/{name}")]
	Task<SplunkFeed<DeploymentServerClient>> GetAsync(string name, [Query] DeploymentServerClientFilter? filter, CancellationToken cancellationToken);

	/// <summary>
	/// Removes a client from the deployment server's registry; it is added again when it next phones home
	/// (<c>DELETE deployment/server/clients/{name}</c>).
	/// </summary>
	/// <remarks>
	/// Splunk 10.6 no longer supports this: it answers HTTP 404 "This functionality has been deprecated", although the
	/// reference still documents it.
	/// </remarks>
	/// <param name="name">The client's identifier.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A task that completes when the client is removed.</returns>
	[Delete("services/deployment/server/clients/{name}")]
	Task DeleteAsync(string name, CancellationToken cancellationToken);
}
