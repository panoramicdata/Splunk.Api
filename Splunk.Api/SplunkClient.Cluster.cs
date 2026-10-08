using Splunk.Api.Interfaces;

namespace Splunk.Api;

public sealed partial class SplunkClient
{
	/// <summary>A node's indexer clustering configuration (<c>cluster/config</c>).</summary>
	public IClusterConfig ClusterConfig => field ??= For<IClusterConfig>();

	/// <summary>The cluster manager's state, health, status, fixups and redundancy (<c>cluster/manager</c>).</summary>
	public IClusterManager ClusterManager => field ??= For<IClusterManager>();

	/// <summary>Clustered buckets, on the cluster manager (<c>cluster/manager/buckets</c>).</summary>
	public IClusterManagerBuckets ClusterManagerBuckets => field ??= For<IClusterManagerBuckets>();

	/// <summary>Bucket and upgrade actions of the cluster manager (<c>cluster/manager/control/control</c>).</summary>
	public IClusterManagerControl ClusterManagerControl => field ??= For<IClusterManagerControl>();

	/// <summary>Bundle, maintenance mode and restart operations of the cluster manager (<c>cluster/manager/control/default</c>).</summary>
	public IClusterManagerOperations ClusterManagerOperations => field ??= For<IClusterManagerOperations>();

	/// <summary>Cluster generations, on the cluster manager (<c>cluster/manager/generation</c>).</summary>
	public IClusterManagerGenerations ClusterManagerGenerations => field ??= For<IClusterManagerGenerations>();

	/// <summary>Clustered indexes, on the cluster manager (<c>cluster/manager/indexes</c>).</summary>
	public IClusterManagerIndexes ClusterManagerIndexes => field ??= For<IClusterManagerIndexes>();

	/// <summary>Cluster peers, on the cluster manager (<c>cluster/manager/peers</c>).</summary>
	public IClusterManagerPeers ClusterManagerPeers => field ??= For<IClusterManagerPeers>();

	/// <summary>Cluster sites, on the cluster manager (<c>cluster/manager/sites</c>).</summary>
	public IClusterManagerSites ClusterManagerSites => field ??= For<IClusterManagerSites>();

	/// <summary>Cluster generations, on an indexer cluster search head (<c>cluster/searchhead/generation</c>).</summary>
	public IClusterSearchHeadGenerations ClusterSearchHeadGenerations => field ??= For<IClusterSearchHeadGenerations>();

	/// <summary>The clusters a search head belongs to (<c>cluster/searchhead/searchheadconfig</c>).</summary>
	public IClusterSearchHeadConfigs ClusterSearchHeadConfigs => field ??= For<IClusterSearchHeadConfigs>();

	/// <summary>This peer node's state and control actions (<c>cluster/peer</c>).</summary>
	public IClusterPeer ClusterPeer => field ??= For<IClusterPeer>();

	/// <summary>This peer node's buckets (<c>cluster/peer/buckets</c>).</summary>
	public IClusterPeerBuckets ClusterPeerBuckets => field ??= For<IClusterPeerBuckets>();

	/// <summary>Search head cluster configuration replication (<c>replication/configuration</c>).</summary>
	public IConfigurationReplication ConfigurationReplication => field ??= For<IConfigurationReplication>();

	/// <summary>The search head cluster captain (<c>shcluster/captain</c>).</summary>
	public IShClusterCaptain ShClusterCaptain => field ??= For<IShClusterCaptain>();

	/// <summary>Search artifacts managed by the captain (<c>shcluster/captain/artifacts</c>).</summary>
	public IShClusterCaptainArtifacts ShClusterCaptainArtifacts => field ??= For<IShClusterCaptainArtifacts>();

	/// <summary>Scheduled jobs dispatched by the captain (<c>shcluster/captain/jobs</c>).</summary>
	public IShClusterCaptainJobs ShClusterCaptainJobs => field ??= For<IShClusterCaptainJobs>();

	/// <summary>Search head cluster members, as the captain sees them (<c>shcluster/captain/members</c>).</summary>
	public IShClusterCaptainMembers ShClusterCaptainMembers => field ??= For<IShClusterCaptainMembers>();

	/// <summary>A node's search head clustering configuration (<c>shcluster/config</c>).</summary>
	public IShClusterConfig ShClusterConfig => field ??= For<IShClusterConfig>();

	/// <summary>This search head cluster member (<c>shcluster/member</c>).</summary>
	public IShClusterMember ShClusterMember => field ??= For<IShClusterMember>();

	/// <summary>Search head cluster health (<c>shcluster/status</c>).</summary>
	public IShClusterStatus ShClusterStatus => field ??= For<IShClusterStatus>();

	/// <summary>Automated rolling upgrades of a search head cluster (<c>upgrade/shc</c>).</summary>
	public IShClusterUpgrades ShClusterUpgrades => field ??= For<IShClusterUpgrades>();
}
