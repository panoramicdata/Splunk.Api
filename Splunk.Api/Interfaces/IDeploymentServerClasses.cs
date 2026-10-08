using Refit;
using Splunk.Api.Models;
using Splunk.Api.Models.Deployment;

namespace Splunk.Api.Interfaces;

/// <summary>The server classes of a deployment server (<c>deployment/server/serverclasses</c>).</summary>
/// <remarks>Server classes are written to serverclass.conf; creating the first one makes the instance act as a deployment server.</remarks>
public interface IDeploymentServerClasses
{
	/// <summary>Lists the server classes (<c>GET deployment/server/serverclasses</c>).</summary>
	/// <param name="options">Paging, filtering and sorting, or <see langword="null"/>.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with one entry per server class.</returns>
	[Get("services/deployment/server/serverclasses")]
	Task<SplunkFeed<DeploymentServerClass>> ListAsync([Query] ListOptions? options, CancellationToken cancellationToken);

	/// <summary>Creates a server class (<c>POST deployment/server/serverclasses</c>).</summary>
	/// <param name="request">The new server class.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with the new server class.</returns>
	[Post("services/deployment/server/serverclasses")]
	Task<SplunkFeed<DeploymentServerClass>> CreateAsync([Body] DeploymentServerClassCreateRequest request, CancellationToken cancellationToken);

	/// <summary>Renames a server class (<c>POST deployment/server/serverclasses/rename</c>).</summary>
	/// <remarks>An unknown old name raises HTTP 500 <c>serverclass=... ("from") does not exist</c>.</remarks>
	/// <param name="request">The old and new names.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>The feed Splunk answers with.</returns>
	[Post("services/deployment/server/serverclasses/rename")]
	Task<SplunkFeed<DeploymentServerClass>> RenameAsync([Body] DeploymentServerClassRenameRequest request, CancellationToken cancellationToken);

	/// <summary>Gets a server class (<c>GET deployment/server/serverclasses/{name}</c>).</summary>
	/// <param name="name">The server class name.</param>
	/// <param name="filter">Filters, or <see langword="null"/>.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with the server class.</returns>
	[Get("services/deployment/server/serverclasses/{name}")]
	Task<SplunkFeed<DeploymentServerClass>> GetAsync(string name, [Query] DeploymentServerClassFilter? filter, CancellationToken cancellationToken);

	/// <summary>Changes a server class (<c>POST deployment/server/serverclasses/{name}</c>).</summary>
	/// <param name="name">The server class name.</param>
	/// <param name="request">The settings to change.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>The feed Splunk answers with.</returns>
	[Post("services/deployment/server/serverclasses/{name}")]
	Task<SplunkFeed<DeploymentServerClass>> UpdateAsync(string name, [Body] DeploymentServerClassUpdateRequest request, CancellationToken cancellationToken);

	/// <summary>Deletes a server class (<c>DELETE deployment/server/serverclasses/{name}</c>).</summary>
	/// <remarks>An unknown name raises HTTP 500 "No config found: sc=..." rather than 404.</remarks>
	/// <param name="name">The server class name.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A task that completes when the server class is deleted.</returns>
	[Delete("services/deployment/server/serverclasses/{name}")]
	Task DeleteAsync(string name, CancellationToken cancellationToken);
}
