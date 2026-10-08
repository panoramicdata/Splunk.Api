using System.Net;

namespace Splunk.Api.IntegrationTest.Topology;

/// <summary>The standalone test instance is its own license manager, so every topology endpoint answers.</summary>
[Collection(SplunkTestGroup.Name)]
public class TopologyIntegrationTests(SplunkFixture fixture)
{
	[Fact]
	public async Task GetAsync_ListsThisInstanceAsLicenseManagerAndIndexer()
	{
		var topology = await fixture.Client.Topology.GetAsync(TestContext.Current.CancellationToken);

		var manager = topology.LicenseManager!;
		manager.NodeGuid.Should().NotBeNullOrWhiteSpace();
		manager.Roles.Should().Contain("license_manager");
		manager.HostInfo!.ManagementPort.Should().BePositive();
		manager.VersionInfo!.Version.Should().NotBeNullOrWhiteSpace();
		manager.OsInfo!.Name.Should().NotBeNullOrWhiteSpace();
		topology.Indexers.Should().Contain(n => n.NodeGuid == manager.NodeGuid);
		topology.Header!.Timestamp.Should().NotBeNull();
	}

	[Fact]
	public async Task GetWithUnmanagedActorsAsync_Succeeds()
	{
		var topology = await fixture.Client.Topology.GetWithUnmanagedActorsAsync(TestContext.Current.CancellationToken);

		topology.LicenseManager.Should().NotBeNull();
		topology.UnmanagedActors.Should().NotBeNull();
	}

	[Fact]
	public async Task GetNodeIdentityAsync_DescribesThisNode()
	{
		var identity = await fixture.Client.Topology.GetNodeIdentityAsync(TestContext.Current.CancellationToken);

		identity.HostInfo!.ManagementPort.Should().BePositive();
		identity.Roles.Should().NotBeEmpty();
		identity.VersionInfo!.Version.Should().NotBeNullOrWhiteSpace();
	}

	[Fact]
	public async Task RemoteEndpoints_AcceptThisNodesOwnGuid()
	{
		var local = await fixture.Client.Topology.GetTrustedConnectionsAsync(TestContext.Current.CancellationToken);
		local.NodeGuid.Should().NotBeNullOrWhiteSpace();
		local.Hec.Should().NotBeNull();

		var remote = await fixture.Client.Topology.GetRemoteTrustedConnectionsAsync(local.NodeGuid!, TestContext.Current.CancellationToken);
		var identity = await fixture.Client.Topology.GetRemoteNodeIdentityAsync(local.NodeGuid!, TestContext.Current.CancellationToken);

		remote.NodeGuid.Should().Be(local.NodeGuid);
		identity.Roles.Should().NotBeEmpty();
	}

	[Fact]
	public async Task RemoteEndpoints_UnknownGuid_Raise404WithTheSidecarsError()
	{
		const string Unknown = "00000000-0000-0000-0000-000000000000";

		var identity = () => fixture.Client.Topology.GetRemoteNodeIdentityAsync(Unknown, TestContext.Current.CancellationToken);
		var connections = () => fixture.Client.Topology.GetRemoteTrustedConnectionsAsync(Unknown, TestContext.Current.CancellationToken);

		var identityError = (await identity.Should().ThrowAsync<SplunkApiException>()).Which;
		identityError.StatusCode.Should().Be(HttpStatusCode.NotFound);
		identityError.Message.Should().StartWith("failed to get node identity").And.Contain($"node with guid: {Unknown} not found");
		var connectionsError = (await connections.Should().ThrowAsync<SplunkApiException>()).Which;
		connectionsError.StatusCode.Should().Be(HttpStatusCode.NotFound);
		connectionsError.Message.Should().StartWith("failed to get trusted connections");
	}
}
