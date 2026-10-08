using Refit;
using Splunk.Api.Models;
using Splunk.Api.Models.Cluster;

namespace Splunk.Api.Interfaces;

/// <summary>A node's search head clustering configuration (<c>shcluster/config</c>), on any node.</summary>
public interface IShClusterConfig
{
	/// <summary>Lists the node's search head clustering configuration (<c>GET shcluster/config</c>).</summary>
	/// <param name="options">Paging, filtering and sorting, or <see langword="null"/>.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with one entry, <c>config</c>.</returns>
	[Get("services/shcluster/config")]
	Task<SplunkFeed<ShClusterConfig>> ListAsync([Query] ListOptions? options, CancellationToken cancellationToken);

	/// <summary>
	/// Sets the rolling restart mode, or puts a member into or out of manual detention (<c>POST shcluster/config/config</c>).
	/// </summary>
	/// <param name="request">The settings.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>The feed Splunk answers with.</returns>
	[Post("services/shcluster/config/config")]
	Task<SplunkFeed<ShClusterConfig>> UpdateAsync([Body] ShClusterConfigUpdateRequest request, CancellationToken cancellationToken);
}
