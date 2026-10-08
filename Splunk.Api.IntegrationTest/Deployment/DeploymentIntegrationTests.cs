using Splunk.Api.Models.Deployment;
using System.Net;

namespace Splunk.Api.IntegrationTest.Deployment;

/// <summary>
/// The shared test instance is a standalone: its deployment client is disabled and it has no server classes, apps or
/// clients. Nothing here creates a server class or changes deployment configuration (that would make the instance act as
/// a deployment server); writes are limited to calls Splunk rejects.
/// </summary>
[Collection(SplunkTestGroup.Name)]
public class DeploymentIntegrationTests(SplunkFixture fixture)
{
	private static CancellationToken Ct => TestContext.Current.CancellationToken;

	[Fact]
	public async Task ClientConfig_IsDisabledOnAStandalone()
	{
		var client = fixture.Client.DeploymentClientConfig;

		(await client.ListAsync(null, Ct)).Entries.Should().ContainSingle().Which.Content!.Disabled.Should().BeTrue();
		(await client.GetAsync(Ct)).Entries.Should().ContainSingle().Which.Name.Should().Be("config");
		var status = (await client.GetDisabledStatusAsync(Ct)).Entries.Should().ContainSingle().Subject;
		status.Name.Should().Be("default");
		status.Content!.Disabled.Should().BeTrue();
	}

	[Fact]
	public async Task ClientConfig_Reload_AnswersWithTheConfigEntry()
	{
		// Reloading a disabled deployment client leaves it disabled.
		var client = fixture.Client.DeploymentClientConfig;

		(await client.ReloadConfigAsync(Ct)).Entries.Should().ContainSingle().Which.Content!.Disabled.Should().BeTrue();
		(await client.ReloadAsync(SplunkFixture.UniqueName("client"), Ct)).Entries.Should().ContainSingle().Which.Name.Should().Be("config");
	}

	[Fact]
	public async Task ServerApplications_ListIsEmpty_AndAnUnknownAppIsNotFound()
	{
		var apps = fixture.Client.DeploymentServerApplications;
		var name = SplunkFixture.UniqueName("app");

		(await apps.ListAsync(null, Ct)).Paging!.Total.Should().Be(0);
		await SplunkAssert.FailsAsync(() => apps.GetAsync(name, Ct), HttpStatusCode.NotFound, $"Could not find object id={name}");
		await SplunkAssert.FailsAsync(
			() => apps.ListAsync(new DeploymentApplicationListOptions { ClientId = name }, Ct),
			HttpStatusCode.BadRequest,
			$"No client id={name}");
	}

	[Fact]
	public async Task ServerClients_ReadsWork_AndDeleteIsDeprecated()
	{
		var clients = fixture.Client.DeploymentServerClients;
		var name = SplunkFixture.UniqueName("client");

		(await clients.ListAsync(new DeploymentServerClientListOptions { Action = "phonehome", HasDeploymentError = false }, Ct)).Entries.Should().BeEmpty();
		(await clients.CountByMachineTypeAsync(Ct)).Entries.Should().ContainSingle().Which.Content!.Counts.Should().BeNull();
		(await clients.CountRecentDownloadsAsync(3600, Ct)).Entries.Should().ContainSingle().Which.Content!.Count.Should().Be(0);
		await SplunkAssert.FailsAsync(() => clients.GetAsync(name, null, Ct), HttpStatusCode.NotFound, $"Could not find object id={name}");
		await SplunkAssert.FailsAsync(
			() => clients.GetAsync(name, new DeploymentServerClientFilter { Application = name }, Ct),
			HttpStatusCode.InternalServerError,
			$"Bad client selector application='{name}': no such application associated with any serverclass.");
		await SplunkAssert.FailsAsync(() => clients.DeleteAsync(name, Ct), HttpStatusCode.NotFound, "This functionality has been deprecated");
	}

	[Fact]
	public async Task ServerConfig_ReadsWork_AndThePostIsRejected()
	{
		var config = fixture.Client.DeploymentServerConfig;

		(await config.GetDisabledStatusAsync(Ct)).Entries.Should().ContainSingle().Which.Name.Should().Be("default");
		(await config.ListUnsupportedAttributesAsync(Ct)).Entries.Should().BeEmpty();
		await SplunkAssert.FailsAsync(
			() => config.PostAsync(new Dictionary<string, string?>(), Ct),
			HttpStatusCode.BadRequest,
			"Cannot perform action \"POST\" without a target name to act on.");
	}

	[Fact]
	public async Task ServerClasses_ListIsEmpty_AndUnknownNamesAreRejected()
	{
		var classes = fixture.Client.DeploymentServerClasses;
		var name = SplunkFixture.UniqueName("sc");

		(await classes.ListAsync(null, Ct)).Entries.Should().BeEmpty();
		await SplunkAssert.FailsAsync(() => classes.GetAsync(name, null, Ct), HttpStatusCode.NotFound, $"Could not find object id={name}");
		await SplunkAssert.FailsAsync(() => classes.DeleteAsync(name, Ct), HttpStatusCode.InternalServerError, $"No config found: sc={name}");
		await SplunkAssert.FailsAsync(
			() => classes.RenameAsync(new DeploymentServerClassRenameRequest { OldName = name, NewName = name + "_2" }, Ct),
			HttpStatusCode.InternalServerError,
			$"serverclass={name} (\"from\") does not exist");
		(await classes.ListAsync(null, Ct)).Entries.Should().BeEmpty();
	}

	[Fact]
	public async Task BundleReplication_ReadsWork()
	{
		var bundles = fixture.Client.BundleReplication;

		var config = (await bundles.GetConfigAsync(Ct)).Entries.Should().ContainSingle().Subject;
		config.Name.Should().Be("bundleReplicationConfig");
		config.Content!.ReplicationPolicy.Should().NotBeNullOrWhiteSpace();
		config.Content.MaxBundleSize.Should().BePositive();
		(await bundles.ListCyclesAsync(true, Ct)).Entries.Should().BeEmpty();
		(await bundles.ListCyclesAsync(null, Ct)).Entries.Should().BeEmpty();
		(await bundles.ListFilesAsync(null, Ct)).Paging.Should().NotBeNull();
		await SplunkAssert.FailsAsync(() => bundles.GetFileAsync("x", null, Ct), HttpStatusCode.BadRequest, "Entity name must be a valid checksum number: x");
		await SplunkAssert.FailsAsync(() => bundles.GetFileAsync("123", true, Ct), HttpStatusCode.NotFound, "Could not find search head bundle with checksum=123");
	}

	[Fact]
	public async Task DistributedSearch_ReadsWork()
	{
		var config = (await fixture.Client.DistributedSearch.GetConfigAsync(Ct)).Entries.Should().ContainSingle().Subject;
		var peers = await fixture.Client.DistributedSearch.ListPeersAsync(null, Ct);

		config.Name.Should().Be("distributedSearch");
		config.Content!.DistributedSearchEnabled.Should().BeTrue();
		config.Content.StatusTimeout.Should().BePositive();
		peers.Entries.Should().BeEmpty();
	}
}
