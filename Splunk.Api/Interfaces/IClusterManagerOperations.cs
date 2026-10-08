using Refit;
using Splunk.Api.Models;
using Splunk.Api.Models.Cluster;

namespace Splunk.Api.Interfaces;

/// <summary>
/// Configuration bundle, maintenance mode and restart operations of the cluster manager
/// (<c>cluster/manager/control/default</c>).
/// </summary>
/// <remarks>Call these on the cluster manager; other nodes answer HTTP 503.</remarks>
public interface IClusterManagerOperations
{
	/// <summary>Aborts a rolling restart of the indexer cluster (<c>POST cluster/manager/control/default/abort_restart</c>).</summary>
	/// <remarks>Needs the admin role or <c>edit_indexer_cluster</c>. The feed's messages list the peers that were not restarted.</remarks>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>The feed Splunk answers with.</returns>
	[Post("services/cluster/manager/control/default/abort_restart")]
	Task<SplunkFeed<SplunkDynamicContent>> AbortRestartAsync(CancellationToken cancellationToken);

	/// <summary>Pushes the configuration bundle in manager-apps to the peers (<c>POST cluster/manager/control/default/apply</c>).</summary>
	/// <remarks>The push may restart the peers.</remarks>
	/// <param name="request">Push options.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with the pushed bundle.</returns>
	[Post("services/cluster/manager/control/default/apply")]
	Task<SplunkFeed<ClusterBundleResult>> ApplyBundleAsync([Body] ClusterApplyBundleRequest request, CancellationToken cancellationToken);

	/// <summary>
	/// Cancels and resets a bundle push, for when the manager gets no validation response from a peer
	/// (<c>POST cluster/manager/control/default/cancel_bundle_push</c>).
	/// </summary>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>The feed Splunk answers with.</returns>
	[Post("services/cluster/manager/control/default/cancel_bundle_push")]
	Task<SplunkFeed<SplunkDynamicContent>> CancelBundlePushAsync(CancellationToken cancellationToken);

	/// <summary>Turns the cluster's maintenance mode on or off (<c>POST cluster/manager/control/default/maintenance</c>).</summary>
	/// <param name="request">The mode.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>The feed Splunk answers with.</returns>
	[Post("services/cluster/manager/control/default/maintenance")]
	Task<SplunkFeed<SplunkDynamicContent>> SetMaintenanceModeAsync([Body] ClusterMaintenanceModeRequest request, CancellationToken cancellationToken);

	/// <summary>Rolls the peers back to the previously active bundle (<c>POST cluster/manager/control/default/rollback</c>).</summary>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with the bundle rolled back to.</returns>
	[Post("services/cluster/manager/control/default/rollback")]
	Task<SplunkFeed<ClusterBundleResult>> RollbackBundleAsync(CancellationToken cancellationToken);

	/// <summary>
	/// Validates the bundle in manager-apps, and optionally checks whether applying it would restart the peers
	/// (<c>POST cluster/manager/control/default/validate_bundle</c>).
	/// </summary>
	/// <param name="request">Validation options.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with the validated bundle.</returns>
	[Post("services/cluster/manager/control/default/validate_bundle")]
	Task<SplunkFeed<ClusterBundleResult>> ValidateBundleAsync([Body] ClusterValidateBundleRequest request, CancellationToken cancellationToken);
}
