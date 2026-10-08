using Refit;
using Splunk.Api.Models;
using Splunk.Api.Models.Cluster;

namespace Splunk.Api.Interfaces;

/// <summary>The search head cluster captain: its state and control actions (<c>shcluster/captain/info</c>, <c>shcluster/captain/control</c>).</summary>
/// <remarks>
/// Call these on the captain; a node without search head clustering answers HTTP 503 "Search Head Clustering is not
/// enabled on this node. REST endpoint is not available".
/// </remarks>
public interface IShClusterCaptain
{
	/// <summary>Gets the captain's state (<c>GET shcluster/captain/info</c>).</summary>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with one entry, <c>captain</c>.</returns>
	[Get("services/shcluster/captain/info")]
	Task<SplunkFeed<ShClusterCaptainInfo>> GetInfoAsync(CancellationToken cancellationToken);

	/// <summary>Starts a rolling restart of the search head cluster (<c>POST shcluster/captain/control/default/restart</c>).</summary>
	/// <param name="request">Restart options.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with the result, <c>restart</c>.</returns>
	[Post("services/shcluster/captain/control/default/restart")]
	Task<SplunkFeed<ShClusterControlResult>> RestartAsync([Body] ShClusterRestartRequest request, CancellationToken cancellationToken);

	/// <summary>
	/// Rotates the splunk.secret file on every member of the search head cluster
	/// (<c>POST shcluster/captain/control/control/rotate-splunk-secret</c>).
	/// </summary>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>The feed Splunk answers with.</returns>
	[Post("services/shcluster/captain/control/control/rotate-splunk-secret")]
	Task<SplunkFeed<ShClusterControlResult>> RotateSplunkSecretAsync(CancellationToken cancellationToken);

	/// <summary>Starts a rolling upgrade of the search head cluster (<c>POST shcluster/captain/control/control/upgrade-init</c>).</summary>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with the result, <c>upgrade-init</c>.</returns>
	[Post("services/shcluster/captain/control/control/upgrade-init")]
	Task<SplunkFeed<ShClusterControlResult>> InitializeUpgradeAsync(CancellationToken cancellationToken);

	/// <summary>Finishes a rolling upgrade of the search head cluster (<c>POST shcluster/captain/control/control/upgrade-finalize</c>).</summary>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with the result, <c>upgrade-finalize</c>.</returns>
	[Post("services/shcluster/captain/control/control/upgrade-finalize")]
	Task<SplunkFeed<ShClusterControlResult>> FinalizeUpgradeAsync(CancellationToken cancellationToken);
}
