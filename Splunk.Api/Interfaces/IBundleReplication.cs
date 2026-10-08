using Refit;
using Splunk.Api.Models;
using Splunk.Api.Models.Deployment;

namespace Splunk.Api.Interfaces;

/// <summary>
/// Knowledge bundle replication from a search head to its search peers (<c>search/distributed/bundle/replication</c> and
/// <c>search/distributed/bundle-replication-files</c>).
/// </summary>
/// <remarks>The replication configuration and cycles need the <c>search</c> capability.</remarks>
public interface IBundleReplication
{
	/// <summary>Gets the bundle replication settings (<c>GET search/distributed/bundle/replication/config</c>).</summary>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with one entry, <c>bundleReplicationConfig</c>.</returns>
	[Get("services/search/distributed/bundle/replication/config")]
	Task<SplunkFeed<BundleReplicationConfig>> GetConfigAsync(CancellationToken cancellationToken);

	/// <summary>Lists the bundle replication cycles (<c>GET search/distributed/bundle/replication/cycles</c>).</summary>
	/// <param name="latest">
	/// <see langword="true"/> for only the latest cycle (<c>latest</c>), or <see langword="null"/> for every cycle kept.
	/// </param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with one entry per cycle, named by cycle ID; empty on a search head with no peers.</returns>
	[Get("services/search/distributed/bundle/replication/cycles")]
	Task<SplunkFeed<BundleReplicationCycle>> ListCyclesAsync([AliasAs("latest")] bool? latest, CancellationToken cancellationToken);

	/// <summary>Lists the most recent knowledge bundles (<c>GET search/distributed/bundle-replication-files</c>).</summary>
	/// <param name="options">Paging, filtering and sorting, or <see langword="null"/>.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with one entry per bundle, named by checksum.</returns>
	[Get("services/search/distributed/bundle-replication-files")]
	Task<SplunkFeed<BundleReplicationFile>> ListFilesAsync([Query] ListOptions? options, CancellationToken cancellationToken);

	/// <summary>Gets a knowledge bundle (<c>GET search/distributed/bundle-replication-files/{name}</c>).</summary>
	/// <remarks>
	/// A name that is not a number raises HTTP 400 "Entity name must be a valid checksum number"; an unknown checksum raises
	/// HTTP 404 "Could not find search head bundle with checksum=...".
	/// </remarks>
	/// <param name="checksum">The bundle checksum.</param>
	/// <param name="forceListAll"><see langword="true"/> to force a listing of the file (<c>force_list_all</c>), or <see langword="null"/>.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with the bundle.</returns>
	[Get("services/search/distributed/bundle-replication-files/{checksum}")]
	Task<SplunkFeed<BundleReplicationFile>> GetFileAsync(string checksum, [AliasAs("force_list_all")] bool? forceListAll, CancellationToken cancellationToken);
}
