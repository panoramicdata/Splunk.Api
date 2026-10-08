using Refit;
using Splunk.Api.Models;
using Splunk.Api.Models.Deployment;

namespace Splunk.Api.Interfaces;

/// <summary>The apps a deployment server distributes (<c>deployment/server/applications</c>).</summary>
public interface IDeploymentServerApplications
{
	/// <summary>Lists the distributed apps and their state (<c>GET deployment/server/applications</c>).</summary>
	/// <remarks>Filtering by an unknown <see cref="DeploymentApplicationListOptions.ClientId"/> raises HTTP 400 "No client id=...".</remarks>
	/// <param name="options">Filters, paging and sorting, or <see langword="null"/>.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with one entry per app.</returns>
	[Get("services/deployment/server/applications")]
	Task<SplunkFeed<DeploymentApplication>> ListAsync([Query] DeploymentApplicationListOptions? options, CancellationToken cancellationToken);

	/// <summary>Gets how an app is distributed (<c>GET deployment/server/applications/{name}</c>).</summary>
	/// <param name="name">The app name.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with the app.</returns>
	[Get("services/deployment/server/applications/{name}")]
	Task<SplunkFeed<DeploymentApplication>> GetAsync(string name, CancellationToken cancellationToken);

	/// <summary>
	/// Changes how an app is distributed: maps it to a server class, unmaps it, or removes it everywhere
	/// (<c>POST deployment/server/applications/{name}</c>).
	/// </summary>
	/// <param name="name">The app name.</param>
	/// <param name="request">The changes.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with the app.</returns>
	[Post("services/deployment/server/applications/{name}")]
	Task<SplunkFeed<DeploymentApplication>> UpdateAsync(string name, [Body] DeploymentApplicationUpdateRequest request, CancellationToken cancellationToken);
}
