using Refit;
using Splunk.Api.Models;
using Splunk.Api.Models.Deployment;

namespace Splunk.Api.Interfaces;

/// <summary>This instance's deployment client configuration (<c>deployment/client</c>).</summary>
/// <remarks>Deployment endpoints are generally not available on Splunk Cloud Platform.</remarks>
public interface IDeploymentClientConfig
{
	/// <summary>
	/// Lists the deployment client configuration: whether this instance is a deployment client, its server classes and its
	/// deployment server (<c>GET deployment/client</c>).
	/// </summary>
	/// <param name="options">Paging, filtering and sorting, or <see langword="null"/>.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with one entry, <c>config</c>.</returns>
	[Get("services/deployment/client")]
	Task<SplunkFeed<DeploymentClientConfig>> ListAsync([Query] ListOptions? options, CancellationToken cancellationToken);

	/// <summary>Gets the deployment client configuration (<c>GET deployment/client/config</c>).</summary>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with one entry, <c>config</c>.</returns>
	[Get("services/deployment/client/config")]
	Task<SplunkFeed<DeploymentClientConfig>> GetAsync(CancellationToken cancellationToken);

	/// <summary>Gets whether this instance's deployment client is disabled (<c>GET deployment/client/config/listIsDisabled</c>).</summary>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with one entry, <c>default</c>, whose <see cref="SplunkContent.Disabled"/> answers.</returns>
	[Get("services/deployment/client/config/listIsDisabled")]
	Task<SplunkFeed<SplunkDynamicContent>> GetDisabledStatusAsync(CancellationToken cancellationToken);

	/// <summary>Reloads the deployment client configuration (<c>POST deployment/client/config/reload</c>).</summary>
	/// <remarks>The same as <see cref="ReloadAsync"/> with <c>config</c>. It does not enable a disabled deployment client.</remarks>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with the <c>config</c> entry.</returns>
	[Post("services/deployment/client/config/reload")]
	Task<SplunkFeed<DeploymentClientConfig>> ReloadConfigAsync(CancellationToken cancellationToken);

	/// <summary>Restarts and reloads a deployment client (<c>POST deployment/client/{name}/reload</c>).</summary>
	/// <remarks>
	/// The only client is <c>config</c>; Splunk 10.6 answers any name with the <c>config</c> entry rather than 404.
	/// </remarks>
	/// <param name="name">The client, <c>config</c>.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with the <c>config</c> entry.</returns>
	[Post("services/deployment/client/{name}/reload")]
	Task<SplunkFeed<DeploymentClientConfig>> ReloadAsync(string name, CancellationToken cancellationToken);
}
