using Refit;
using Splunk.Api.Models;
using Splunk.Api.Models.Cluster;

namespace Splunk.Api.Interfaces;

/// <summary>Bucket and upgrade actions of the cluster manager (<c>cluster/manager/control/control</c>).</summary>
/// <remarks>
/// Call these on the cluster manager; other nodes answer HTTP 503. Each answers an empty feed whose
/// <see cref="SplunkFeed{T}.Messages"/> may carry Splunk's report.
/// </remarks>
public interface IClusterManagerControl
{
	/// <summary>Removes excess bucket copies from one index or all (<c>POST cluster/manager/control/control/prune_index</c>).</summary>
	/// <param name="request">The index, or none for all.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>The feed Splunk answers with.</returns>
	[Post("services/cluster/manager/control/control/prune_index")]
	Task<SplunkFeed<SplunkDynamicContent>> PruneIndexAsync([Body] ClusterPruneIndexRequest request, CancellationToken cancellationToken);

	/// <summary>Rebalances primary bucket copies across the peers (<c>POST cluster/manager/control/control/rebalance_primaries</c>).</summary>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>The feed Splunk answers with.</returns>
	[Post("services/cluster/manager/control/control/rebalance_primaries")]
	Task<SplunkFeed<SplunkDynamicContent>> RebalancePrimariesAsync(CancellationToken cancellationToken);

	/// <summary>Removes peers from the cluster (<c>POST cluster/manager/control/control/remove_peers</c>).</summary>
	/// <param name="request">The peers; each must be <c>Down</c> or <c>GracefulShutdown</c>.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>The feed Splunk answers with.</returns>
	[Post("services/cluster/manager/control/control/remove_peers")]
	Task<SplunkFeed<SplunkDynamicContent>> RemovePeersAsync([Body] ClusterRemovePeersRequest request, CancellationToken cancellationToken);

	/// <summary>
	/// Resets a bucket's state from its current state on one peer
	/// (<c>POST cluster/manager/control/control/resync_bucket_from_peer</c>).
	/// </summary>
	/// <param name="request">The bucket and peer.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>The feed Splunk answers with.</returns>
	[Post("services/cluster/manager/control/control/resync_bucket_from_peer")]
	Task<SplunkFeed<SplunkDynamicContent>> ResyncBucketFromPeerAsync([Body] ClusterResyncBucketRequest request, CancellationToken cancellationToken);

	/// <summary>Rolls a hot bucket to warm on every peer (<c>POST cluster/manager/control/control/roll-hot-buckets</c>).</summary>
	/// <remarks>Needs the admin role.</remarks>
	/// <param name="request">The bucket.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>The feed Splunk answers with.</returns>
	[Post("services/cluster/manager/control/control/roll-hot-buckets")]
	Task<SplunkFeed<SplunkDynamicContent>> RollHotBucketAsync([Body] ClusterRollHotBucketRequest request, CancellationToken cancellationToken);

	/// <summary>Finishes a rolling upgrade of the indexer cluster (<c>POST cluster/manager/control/control/rolling_upgrade_finalize</c>).</summary>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>The feed Splunk answers with.</returns>
	[Post("services/cluster/manager/control/control/rolling_upgrade_finalize")]
	Task<SplunkFeed<SplunkDynamicContent>> FinalizeRollingUpgradeAsync(CancellationToken cancellationToken);

	/// <summary>Starts a rolling upgrade of the indexer cluster (<c>POST cluster/manager/control/control/rolling_upgrade_init</c>).</summary>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>The feed Splunk answers with.</returns>
	[Post("services/cluster/manager/control/control/rolling_upgrade_init")]
	Task<SplunkFeed<SplunkDynamicContent>> InitializeRollingUpgradeAsync(CancellationToken cancellationToken);
}
