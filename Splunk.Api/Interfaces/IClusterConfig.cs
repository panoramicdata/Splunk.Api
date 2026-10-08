using Refit;
using Splunk.Api.Models;
using Splunk.Api.Models.Cluster;

namespace Splunk.Api.Interfaces;

/// <summary>A node's indexer clustering configuration (<c>cluster/config</c>), on any node.</summary>
/// <remarks>Cluster endpoints are not available on Splunk Cloud Platform.</remarks>
public interface IClusterConfig
{
	/// <summary>Lists the node's clustering configuration (<c>GET cluster/config</c>).</summary>
	/// <param name="options">Paging, filtering and sorting, or <see langword="null"/>.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with one entry, <c>config</c>.</returns>
	[Get("services/cluster/config")]
	Task<SplunkFeed<ClusterConfig>> ListAsync([Query] ListOptions? options, CancellationToken cancellationToken);

	/// <summary>Gets the node's clustering configuration (<c>GET cluster/config/config</c>); the same as <see cref="ListAsync"/>.</summary>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with one entry, <c>config</c>.</returns>
	[Get("services/cluster/config/config")]
	Task<SplunkFeed<ClusterConfig>> GetAsync(CancellationToken cancellationToken);

	/// <summary>Changes the node's clustering configuration (<c>POST cluster/config/config</c>).</summary>
	/// <remarks>
	/// This turns clustering on, off or into another mode, writing the <c>[clustering]</c> stanza of server.conf; a mode
	/// change takes effect after a restart.
	/// </remarks>
	/// <param name="request">The configuration.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>The feed Splunk answers with.</returns>
	[Post("services/cluster/config/config")]
	Task<SplunkFeed<ClusterConfig>> UpdateAsync([Body] ClusterConfigUpdateRequest request, CancellationToken cancellationToken);
}
