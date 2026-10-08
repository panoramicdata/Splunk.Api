using Refit;
using Splunk.Api.Models.Cluster;

namespace Splunk.Api.Interfaces;

/// <summary>Automated rolling upgrades of a search head cluster (<c>upgrade/shc</c>).</summary>
/// <remarks>
/// Needs the admin role, or <c>upgrade_splunk_shc</c>, <c>list_search_head_clustering</c>, <c>list_settings</c> and
/// <c>use_remote_proxy</c>. These answer a <c>props</c> layout (<see cref="ShClusterUpgradeResponse{T}"/>), not the usual
/// feed. On a node not set up for automated upgrades Splunk 10.6 answers HTTP 400 "Configuration error: 'passAuth' does
/// not exist", in XML whatever the output mode.
/// </remarks>
public interface IShClusterUpgrades
{
	/// <summary>Starts an automated rolling upgrade of the search head cluster (<c>POST upgrade/shc/upgrade</c>).</summary>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>The result.</returns>
	[Post("services/upgrade/shc/upgrade")]
	Task<ShClusterUpgradeResponse<ShClusterUpgradeResult>> StartAsync(CancellationToken cancellationToken);

	/// <summary>Gets the progress of an automated rolling upgrade (<c>GET upgrade/shc/status</c>).</summary>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>The progress.</returns>
	[Get("services/upgrade/shc/status")]
	Task<ShClusterUpgradeResponse<ShClusterUpgradeStatus>> GetStatusAsync(CancellationToken cancellationToken);

	/// <summary>
	/// Returns the search head cluster to a ready state after a failed automated rolling upgrade (<c>POST upgrade/shc/recovery</c>).
	/// </summary>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>The result.</returns>
	[Post("services/upgrade/shc/recovery")]
	Task<ShClusterUpgradeResponse<ShClusterUpgradeResult>> RecoverAsync(CancellationToken cancellationToken);
}
