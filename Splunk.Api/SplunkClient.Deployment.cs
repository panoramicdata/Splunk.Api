using Splunk.Api.Interfaces;

namespace Splunk.Api;

public sealed partial class SplunkClient
{
	/// <summary>This instance's deployment client configuration (<c>deployment/client</c>).</summary>
	public IDeploymentClientConfig DeploymentClientConfig => field ??= For<IDeploymentClientConfig>();

	/// <summary>Apps distributed by the deployment server (<c>deployment/server/applications</c>).</summary>
	public IDeploymentServerApplications DeploymentServerApplications => field ??= For<IDeploymentServerApplications>();

	/// <summary>Clients of the deployment server (<c>deployment/server/clients</c>).</summary>
	public IDeploymentServerClients DeploymentServerClients => field ??= For<IDeploymentServerClients>();

	/// <summary>The deployment server configuration (<c>deployment/server/config</c>).</summary>
	public IDeploymentServerConfig DeploymentServerConfig => field ??= For<IDeploymentServerConfig>();

	/// <summary>Server classes of the deployment server (<c>deployment/server/serverclasses</c>).</summary>
	public IDeploymentServerClasses DeploymentServerClasses => field ??= For<IDeploymentServerClasses>();

	/// <summary>Knowledge bundle replication (<c>search/distributed/bundle/replication</c>, <c>search/distributed/bundle-replication-files</c>).</summary>
	public IBundleReplication BundleReplication => field ??= For<IBundleReplication>();

	/// <summary>Distributed search settings and search peers (<c>search/distributed/config</c>, <c>search/distributed/peers</c>).</summary>
	public IDistributedSearch DistributedSearch => field ??= For<IDistributedSearch>();
}
