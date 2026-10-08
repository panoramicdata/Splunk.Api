using Refit;
using Splunk.Api.Models;
using Splunk.Api.Models.Cluster;

namespace Splunk.Api.Interfaces;

/// <summary>Configuration replication in a search head cluster (<c>replication/configuration</c>).</summary>
/// <remarks>
/// On a node that is not a search head cluster member these answer HTTP 400 "No local ConfRepo registered".
/// </remarks>
public interface IConfigurationReplication
{
	/// <summary>Runs a configuration replication health check (<c>GET replication/configuration/health</c>).</summary>
	/// <param name="options">The check to run, or <see langword="null"/>.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed whose entries depend on the check; see <see cref="ConfigurationReplicationHealth"/>.</returns>
	[Get("services/replication/configuration/health")]
	Task<SplunkFeed<ConfigurationReplicationHealth>> GetHealthAsync([Query] ConfigurationReplicationHealthOptions? options, CancellationToken cancellationToken);

	/// <summary>Lists the quarantined lookups (<c>GET replication/configuration/quarantined-assets</c>).</summary>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with one entry per quarantined lookup.</returns>
	[Get("services/replication/configuration/quarantined-assets")]
	Task<SplunkFeed<QuarantinedAsset>> ListQuarantinedAssetsAsync(CancellationToken cancellationToken);
}
