using Refit;
using Splunk.Api.Models;
using Splunk.Api.Models.Introspection;

namespace Splunk.Api.Interfaces;

/// <summary>The health report of splunkd and of a distributed deployment (<c>server/health</c>). Needs <c>list_health</c>.</summary>
public interface IHealth
{
	/// <summary>Gets the overall health of the distributed deployment (<c>GET server/health/deployment</c>).</summary>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with one entry, <c>deployment</c>.</returns>
	[Get("services/server/health/deployment")]
	Task<SplunkFeed<HealthReport>> GetDeploymentAsync(CancellationToken cancellationToken);

	/// <summary>Gets the deployment's health with every feature (<c>GET server/health/deployment/details</c>).</summary>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with one entry, <c>deployment</c>, whose <see cref="HealthReport.Features"/> is filled.</returns>
	[Get("services/server/health/deployment/details")]
	Task<SplunkFeed<HealthReport>> GetDeploymentDetailsAsync(CancellationToken cancellationToken);

	/// <summary>Gets the overall health of splunkd (<c>GET server/health/splunkd</c>).</summary>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with one entry, <c>splunkd</c>.</returns>
	[Get("services/server/health/splunkd")]
	Task<SplunkFeed<HealthReport>> GetSplunkdAsync(CancellationToken cancellationToken);

	/// <summary>Gets splunkd's health with every feature and indicator (<c>GET server/health/splunkd/details</c>).</summary>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with one entry, <c>splunkd</c>, whose <see cref="HealthReport.Features"/> is filled.</returns>
	[Get("services/server/health/splunkd/details")]
	Task<SplunkFeed<HealthReport>> GetSplunkdDetailsAsync(CancellationToken cancellationToken);
}
