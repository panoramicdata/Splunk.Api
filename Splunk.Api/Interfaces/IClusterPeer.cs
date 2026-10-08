using Refit;
using Splunk.Api.Models;
using Splunk.Api.Models.Cluster;

namespace Splunk.Api.Interfaces;

/// <summary>This indexer cluster peer node: its state and control actions (<c>cluster/peer/info</c>, <c>cluster/peer/control/control</c>).</summary>
/// <remarks>Call these on an indexer cluster peer; other nodes answer HTTP 503.</remarks>
public interface IClusterPeer
{
	/// <summary>Gets the peer's state (<c>GET cluster/peer/info</c>).</summary>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with one entry, <c>peer</c>.</returns>
	[Get("services/cluster/peer/info")]
	Task<SplunkFeed<ClusterPeerInfo>> GetInfoAsync(CancellationToken cancellationToken);

	/// <summary>Decommissions the peer (<c>POST cluster/peer/control/control/decommission</c>).</summary>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>The feed Splunk answers with.</returns>
	[Post("services/cluster/peer/control/control/decommission")]
	Task<SplunkFeed<SplunkDynamicContent>> DecommissionAsync(CancellationToken cancellationToken);

	/// <summary>
	/// Makes the peer re-add itself to the manager, syncing its bucket state (<c>POST cluster/peer/control/control/re-add-peer</c>).
	/// </summary>
	/// <param name="request">Whether the manager reassigns all primary copies.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>The feed Splunk answers with.</returns>
	[Post("services/cluster/peer/control/control/re-add-peer")]
	Task<SplunkFeed<SplunkDynamicContent>> ReAddAsync([Body] ClusterPeerReAddRequest request, CancellationToken cancellationToken);

	/// <summary>
	/// Puts the peer into manual detention, where it is no replication target, or takes it out
	/// (<c>POST cluster/peer/control/control/set_manual_detention</c>).
	/// </summary>
	/// <remarks>Manual detention survives restarts. It replaces the deprecated <c>set_detention_override</c>.</remarks>
	/// <param name="request">The detention state.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>The feed Splunk answers with.</returns>
	[Post("services/cluster/peer/control/control/set_manual_detention")]
	Task<SplunkFeed<SplunkDynamicContent>> SetManualDetentionAsync([Body] ManualDetentionRequest request, CancellationToken cancellationToken);
}
