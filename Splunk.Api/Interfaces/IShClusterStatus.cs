using Refit;
using Splunk.Api.Models;
using Splunk.Api.Models.Cluster;

namespace Splunk.Api.Interfaces;

/// <summary>Search head cluster health (<c>shcluster/status</c>).</summary>
/// <remarks>
/// Needs the admin role or <c>list_search_head_clustering</c>; a node without search head clustering answers HTTP 503.
/// </remarks>
public interface IShClusterStatus
{
	/// <summary>
	/// Runs the health checks made before a rolling upgrade or restart of the search head cluster (<c>GET shcluster/status</c>).
	/// </summary>
	/// <param name="advanced"><see langword="true"/> for verbose status (<c>advanced</c>), or <see langword="null"/>.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with one entry, <c>status</c>.</returns>
	[Get("services/shcluster/status")]
	Task<SplunkFeed<ShClusterStatus>> GetAsync([AliasAs("advanced")] bool? advanced, CancellationToken cancellationToken);
}
