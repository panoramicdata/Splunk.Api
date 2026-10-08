using Splunk.Api.Models.Cluster;
using System.Net;

namespace Splunk.Api.IntegrationTest.Cluster;

/// <summary>
/// The shared test instance is a standalone: indexer and search head clustering are off. These tests read the clustering
/// configuration and pin the errors a standalone gives for role-specific endpoints. They never change clustering
/// configuration, push bundles, restart, upgrade or enter maintenance mode; the few POST and DELETE calls made are ones
/// Splunk rejects outright because the role is not enabled, against made-up prefixed names.
/// </summary>
[Collection(SplunkTestGroup.Name)]
public class ClusterIntegrationTests(SplunkFixture fixture)
{
	private const string ManagerNotEnabled = "Cluster manager is not enabled on this node";
	private const string PeerNotEnabled = "Cluster peer is not enabled on this node, check clustering stanza in server.conf";
	private const string ShcNotEnabled = "Search Head Clustering is not enabled on this node. REST endpoint is not available";

	private static readonly string FakeBucket = $"{SplunkFixture.Prefix}idx~0~00000000-0000-0000-0000-000000000000";

	private static CancellationToken Ct => TestContext.Current.CancellationToken;

	[Fact]
	public async Task ClusterConfig_ShowsClusteringDisabled()
	{
		var listed = (await fixture.Client.ClusterConfig.ListAsync(null, Ct)).Entries.Should().ContainSingle().Subject;
		var config = (await fixture.Client.ClusterConfig.GetAsync(Ct)).Entries.Should().ContainSingle().Subject;

		listed.Name.Should().Be("config");
		config.Content!.Mode.Should().Be(ClusterMode.Disabled);
		config.Content.Disabled.Should().BeTrue();
		config.Content.ReplicationFactor.Should().BePositive();
		config.Content.ClusterGuid.Should().NotBeNullOrWhiteSpace();
	}

	[Fact]
	public async Task ShClusterConfig_ShowsSearchHeadClusteringDisabled()
	{
		var config = (await fixture.Client.ShClusterConfig.ListAsync(null, Ct)).Entries.Should().ContainSingle().Subject.Content!;

		config.Mode.Should().Be("disabled");
		config.Disabled.Should().BeTrue();
		config.ManualDetention.Should().Be(ManualDetentionMode.Off);
	}

	[Fact]
	public async Task ManagerEndpoints_OnAStandalone_Raise503()
	{
		var c = fixture.Client;

		await SplunkAssert.FailsAsync(() => c.ClusterManager.GetInfoAsync(Ct), HttpStatusCode.ServiceUnavailable, ManagerNotEnabled);
		await SplunkAssert.FailsAsync(() => c.ClusterManager.GetHealthAsync(Ct), HttpStatusCode.ServiceUnavailable, ManagerNotEnabled);
		await SplunkAssert.FailsAsync(() => c.ClusterManager.GetStatusAsync(Ct), HttpStatusCode.ServiceUnavailable, ManagerNotEnabled);
		await SplunkAssert.FailsAsync(() => c.ClusterManager.GetHaActiveStatusAsync(Ct), HttpStatusCode.ServiceUnavailable, "Cluster manager is not enabled.");
		await SplunkAssert.FailsAsync(() => c.ClusterManager.ListFixupsAsync(ClusterFixupLevel.Generation, null, Ct), HttpStatusCode.ServiceUnavailable, ManagerNotEnabled);
		await SplunkAssert.FailsAsync(() => c.ClusterManager.ListRedundancyAsync(Ct), HttpStatusCode.ServiceUnavailable, ManagerNotEnabled + ".");
		await SplunkAssert.FailsAsync(() => c.ClusterManagerBuckets.ListAsync(null, Ct), HttpStatusCode.ServiceUnavailable, ManagerNotEnabled);
		await SplunkAssert.FailsAsync(() => c.ClusterManagerBuckets.GetAsync(FakeBucket, Ct), HttpStatusCode.ServiceUnavailable, ManagerNotEnabled);
		await SplunkAssert.FailsAsync(() => c.ClusterManagerGenerations.ListAsync(null, Ct), HttpStatusCode.ServiceUnavailable, ManagerNotEnabled);
		await SplunkAssert.FailsAsync(() => c.ClusterManagerGenerations.GetAsync("manager", Ct), HttpStatusCode.ServiceUnavailable, ManagerNotEnabled);
		await SplunkAssert.FailsAsync(() => c.ClusterManagerIndexes.ListAsync(null, Ct), HttpStatusCode.ServiceUnavailable, ManagerNotEnabled);
		await SplunkAssert.FailsAsync(() => c.ClusterManagerIndexes.GetAsync("main", Ct), HttpStatusCode.ServiceUnavailable, ManagerNotEnabled);
		await SplunkAssert.FailsAsync(() => c.ClusterManagerPeers.ListAsync(null, Ct), HttpStatusCode.ServiceUnavailable, ManagerNotEnabled);
		await SplunkAssert.FailsAsync(() => c.ClusterManagerPeers.GetAsync("x", null, Ct), HttpStatusCode.ServiceUnavailable, ManagerNotEnabled);
		await SplunkAssert.FailsAsync(() => c.ClusterManagerSites.ListAsync(null, Ct), HttpStatusCode.ServiceUnavailable, ManagerNotEnabled);
		await SplunkAssert.FailsAsync(() => c.ClusterManagerSites.GetAsync("site1", Ct), HttpStatusCode.ServiceUnavailable, ManagerNotEnabled);
	}

	[Fact]
	public async Task ManagerBucketActions_OnAStandalone_AreRejected()
	{
		// Rejected before anything happens: this node is not a cluster manager, and the bucket does not exist.
		await SplunkAssert.FailsAsync(() => fixture.Client.ClusterManagerBuckets.FixAsync(FakeBucket, Ct), HttpStatusCode.ServiceUnavailable, ManagerNotEnabled);
		await SplunkAssert.FailsAsync(
			() => fixture.Client.ClusterManagerControl.RollHotBucketAsync(new ClusterRollHotBucketRequest { BucketId = FakeBucket }, Ct),
			HttpStatusCode.ServiceUnavailable,
			ManagerNotEnabled);
	}

	[Fact]
	public async Task PeerAndSearchHeadEndpoints_OnAStandalone_Raise503()
	{
		var c = fixture.Client;

		await SplunkAssert.FailsAsync(() => c.ClusterPeer.GetInfoAsync(Ct), HttpStatusCode.ServiceUnavailable, PeerNotEnabled);
		await SplunkAssert.FailsAsync(() => c.ClusterPeerBuckets.ListAsync(null, Ct), HttpStatusCode.ServiceUnavailable, PeerNotEnabled);
		await SplunkAssert.FailsAsync(() => c.ClusterPeerBuckets.GetAsync(FakeBucket, null, Ct), HttpStatusCode.ServiceUnavailable, PeerNotEnabled);
		await SplunkAssert.FailsAsync(
			() => c.ClusterPeerBuckets.DeleteAsync(FakeBucket, new ClusterPeerBucketRemoveRequest { BucketId = FakeBucket }, Ct),
			HttpStatusCode.ServiceUnavailable,
			PeerNotEnabled);
		await SplunkAssert.FailsAsync(() => c.ClusterSearchHeadGenerations.ListAsync(null, Ct), HttpStatusCode.ServiceUnavailable, "Search head or cluster manager is not enabled on this node.");
		await SplunkAssert.FailsAsync(() => c.ClusterSearchHeadGenerations.GetAsync("x", Ct), HttpStatusCode.ServiceUnavailable, "Search head or cluster manager is not enabled on this node.");
		await SplunkAssert.FailsAsync(() => c.ClusterSearchHeadConfigs.ListAsync(null, Ct), HttpStatusCode.ServiceUnavailable, "Searchhead is not enabled on this node");
		await SplunkAssert.FailsAsync(() => c.ClusterSearchHeadConfigs.GetAsync("x", Ct), HttpStatusCode.ServiceUnavailable, "Searchhead is not enabled on this node");
		await SplunkAssert.FailsAsync(
			() => c.ClusterSearchHeadConfigs.DeleteAsync(SplunkFixture.UniqueName("sh"), Ct),
			HttpStatusCode.ServiceUnavailable,
			"Searchhead is not enabled on this node");
	}

	[Fact]
	public async Task SearchHeadClusterEndpoints_OnAStandalone_AreRejected()
	{
		var c = fixture.Client;

		await SplunkAssert.FailsAsync(() => c.ShClusterCaptain.GetInfoAsync(Ct), HttpStatusCode.ServiceUnavailable, ShcNotEnabled);
		await SplunkAssert.FailsAsync(() => c.ShClusterCaptainArtifacts.ListAsync(null, Ct), HttpStatusCode.ServiceUnavailable, ShcNotEnabled);
		await SplunkAssert.FailsAsync(() => c.ShClusterCaptainArtifacts.GetAsync("x", Ct), HttpStatusCode.ServiceUnavailable, ShcNotEnabled);
		await SplunkAssert.FailsAsync(() => c.ShClusterCaptainJobs.ListAsync(null, Ct), HttpStatusCode.ServiceUnavailable, ShcNotEnabled);
		await SplunkAssert.FailsAsync(() => c.ShClusterCaptainJobs.GetAsync("x", Ct), HttpStatusCode.ServiceUnavailable, ShcNotEnabled);
		await SplunkAssert.FailsAsync(() => c.ShClusterCaptainMembers.ListAsync(null, Ct), HttpStatusCode.ServiceUnavailable, ShcNotEnabled);
		await SplunkAssert.FailsAsync(() => c.ShClusterCaptainMembers.GetAsync("x", Ct), HttpStatusCode.ServiceUnavailable, ShcNotEnabled);
		await SplunkAssert.FailsAsync(() => c.ShClusterMember.GetInfoAsync(Ct), HttpStatusCode.ServiceUnavailable, ShcNotEnabled);
		await SplunkAssert.FailsAsync(() => c.ShClusterMember.ListArtifactsAsync(null, Ct), HttpStatusCode.ServiceUnavailable, ShcNotEnabled);
		await SplunkAssert.FailsAsync(() => c.ShClusterMember.GetArtifactAsync("x", Ct), HttpStatusCode.ServiceUnavailable, ShcNotEnabled);
		await SplunkAssert.FailsAsync(
			() => c.ShClusterMember.GetConsensusAsync(Ct),
			HttpStatusCode.BadRequest,
			"Search Head Clustering is not enabled on this node. Raft REST endpoints are not available!");
		await SplunkAssert.FailsAsync(() => c.ShClusterStatus.GetAsync(null, Ct), HttpStatusCode.ServiceUnavailable, ShcNotEnabled);
		await SplunkAssert.FailsAsync(() => c.ShClusterUpgrades.GetStatusAsync(Ct), HttpStatusCode.BadRequest, "Configuration error: 'passAuth' does not exist");
	}

	[Fact]
	public async Task ConfigurationReplication_OnAStandalone_Raises400()
	{
		await SplunkAssert.FailsAsync(() => fixture.Client.ConfigurationReplication.GetHealthAsync(null, Ct), HttpStatusCode.BadRequest, "No local ConfRepo registered");
		await SplunkAssert.FailsAsync(
			() => fixture.Client.ConfigurationReplication.GetHealthAsync(new ConfigurationReplicationHealthOptions { CheckShareBaseline = true }, Ct),
			HttpStatusCode.BadRequest,
			"No local ConfRepo registered");
		await SplunkAssert.FailsAsync(() => fixture.Client.ConfigurationReplication.ListQuarantinedAssetsAsync(Ct), HttpStatusCode.BadRequest, "No local ConfRepo registered");
	}
}
