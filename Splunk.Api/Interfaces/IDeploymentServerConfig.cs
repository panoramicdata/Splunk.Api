using Refit;
using Splunk.Api.Models;
using Splunk.Api.Models.Deployment;

namespace Splunk.Api.Interfaces;

/// <summary>The deployment server configuration (<c>deployment/server/config</c>).</summary>
public interface IDeploymentServerConfig
{
	/// <summary>Posts to the deployment server configuration collection (<c>POST deployment/server/config</c>).</summary>
	/// <remarks>
	/// The reference documents this as a POST that lists the configuration, but its example is a GET, and Splunk 10.6
	/// rejects the POST with HTTP 400 "Cannot perform action "POST" without a target name to act on." whatever the fields.
	/// The configuration itself is the <c>config</c> entry of <c>GET deployment/server/config</c>.
	/// </remarks>
	/// <param name="fields">The form fields to send.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>The feed Splunk answers with.</returns>
	[Post("services/deployment/server/config")]
	Task<SplunkFeed<DeploymentServerConfig>> PostAsync([Body] IDictionary<string, string?> fields, CancellationToken cancellationToken);

	/// <summary>
	/// Lists the deployment server settings Splunk Web cannot change
	/// (<c>GET deployment/server/config/attributesUnsupportedInUI</c>).
	/// </summary>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with one entry per setting.</returns>
	[Get("services/deployment/server/config/attributesUnsupportedInUI")]
	Task<SplunkFeed<DeploymentUnsupportedSetting>> ListUnsupportedAttributesAsync(CancellationToken cancellationToken);

	/// <summary>Gets whether the deployment server is disabled (<c>GET deployment/server/config/listIsDisabled</c>).</summary>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with one entry, <c>default</c>, whose <see cref="SplunkContent.Disabled"/> answers.</returns>
	[Get("services/deployment/server/config/listIsDisabled")]
	Task<SplunkFeed<SplunkDynamicContent>> GetDisabledStatusAsync(CancellationToken cancellationToken);
}
