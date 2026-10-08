using Refit;
using Splunk.Api.Models;
using Splunk.Api.Models.Cluster;

namespace Splunk.Api.Interfaces;

/// <summary>
/// The state of the cluster manager: info, health, rolling restart status, fixup levels and manager redundancy
/// (<c>cluster/manager/...</c>).
/// </summary>
/// <remarks>Call these on the cluster manager; other nodes answer HTTP 503 "Cluster manager is not enabled on this node".</remarks>
public interface IClusterManager
{
	/// <summary>Gets the cluster manager's state and bundles (<c>GET cluster/manager/info</c>).</summary>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with one entry, <c>manager</c>.</returns>
	[Get("services/cluster/manager/info")]
	Task<SplunkFeed<ClusterManagerInfo>> GetInfoAsync(CancellationToken cancellationToken);

	/// <summary>Runs the health checks made before a rolling upgrade (<c>GET cluster/manager/health</c>).</summary>
	/// <remarks>Needs the admin role or <c>list_indexer_cluster</c>.</remarks>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with one entry, <c>manager</c>.</returns>
	[Get("services/cluster/manager/health")]
	Task<SplunkFeed<ClusterManagerHealth>> GetHealthAsync(CancellationToken cancellationToken);

	/// <summary>Gets the rolling restart status (<c>GET cluster/manager/status</c>).</summary>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with one entry, <c>manager</c>.</returns>
	[Get("services/cluster/manager/status")]
	Task<SplunkFeed<ClusterManagerStatus>> GetStatusAsync(CancellationToken cancellationToken);

	/// <summary>
	/// Checks whether this cluster manager is the active one, for load balancers
	/// (<c>GET cluster/manager/ha_active_status</c>).
	/// </summary>
	/// <remarks>
	/// The endpoint needs no authentication. The active manager answers HTTP 200 with an empty feed; a standby or starting
	/// manager raises HTTP 503 "Cluster manager is in inactive mode.", and a node that is not a manager HTTP 503 "Cluster
	/// manager is not enabled.".
	/// </remarks>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>An empty feed.</returns>
	[Get("services/cluster/manager/ha_active_status")]
	Task<SplunkFeed<SplunkDynamicContent>> GetHaActiveStatusAsync(CancellationToken cancellationToken);

	/// <summary>Lists the buckets on a fixup level (<c>GET cluster/manager/fixup</c>).</summary>
	/// <remarks>A bucket that needs fixup starts on every level and leaves each as it is processed there.</remarks>
	/// <param name="level">The fixup level (<c>level</c>).</param>
	/// <param name="options">An index filter, paging and sorting, or <see langword="null"/>.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with one entry per bucket.</returns>
	[Get("services/cluster/manager/fixup")]
	Task<SplunkFeed<ClusterFixupBucket>> ListFixupsAsync([AliasAs("level")] ClusterFixupLevel level, [Query] ClusterFixupListOptions? options, CancellationToken cancellationToken);

	/// <summary>
	/// Lists the cluster managers taking part in cluster manager redundancy (<c>GET cluster/manager/redundancy</c>).
	/// </summary>
	/// <remarks>Needs <c>list_indexer_cluster</c>.</remarks>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with one entry per manager, named by GUID.</returns>
	[Get("services/cluster/manager/redundancy")]
	Task<SplunkFeed<ClusterManagerNode>> ListRedundancyAsync(CancellationToken cancellationToken);

	/// <summary>Switches this cluster manager to active or standby (<c>POST cluster/manager/redundancy</c>).</summary>
	/// <remarks>Needs <c>edit_indexer_cluster</c>.</remarks>
	/// <param name="request">The mode to switch to.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with the manager's resulting mode.</returns>
	[Post("services/cluster/manager/redundancy")]
	Task<SplunkFeed<ClusterManagerNode>> SwitchHaModeAsync([Body] ClusterHaModeSwitchRequest request, CancellationToken cancellationToken);
}
