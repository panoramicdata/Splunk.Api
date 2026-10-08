using Splunk.Api.Models;
using Splunk.Api.Models.Introspection;
using System.Net;

namespace Splunk.Api.IntegrationTest.Introspection;

/// <summary>Health, introspection and status reads. Health report settings are not changed (unit tests cover the writes).</summary>
[Collection(SplunkTestGroup.Name)]
public class IntrospectionIntegrationTests(SplunkFixture fixture)
{
	private static CancellationToken Token => TestContext.Current.CancellationToken;

	[Fact]
	public async Task Health_IsReported()
	{
		var splunkd = await fixture.Client.Health.GetSplunkdAsync(Token);
		var details = await fixture.Client.Health.GetSplunkdDetailsAsync(Token);
		var deployment = await fixture.Client.Health.GetDeploymentAsync(Token);
		var deploymentDetails = await fixture.Client.Health.GetDeploymentDetailsAsync(Token);
		var config = await fixture.Client.HealthConfig.ListAsync(new ListOptions { Count = 0 }, Token);

		splunkd.Entries.Single().Content!.Health.Should().NotBe(HealthColor.Unknown);
		details.Entries.Single().Content!.Features.Should().NotBeEmpty();
		deployment.Entries.Single().Content!.Health.Should().NotBe(HealthColor.Unknown);
		deploymentDetails.Entries.Should().ContainSingle();
		config.Entries.Select(e => e.Name).Should().Contain("alert_action:email").And.Contain("feature:iowait");
	}

	[Fact]
	public async Task Introspection_IsRead()
	{
		var list = await fixture.Client.Introspection.ListAsync(Token);
		var indexer = await fixture.Client.Introspection.GetIndexerAsync(Token);
		var dispatch = await fixture.Client.Introspection.ListSearchDispatchAsync(Token);
		var timings = new[]
		{
			await fixture.Client.Introspection.GetBundleDirectoryReaperAsync(Token),
			await fixture.Client.Introspection.GetComputeUserSearchQuotaAsync(Token),
			await fixture.Client.Introspection.GetDispatchDirectoryReaperAsync(Token),
			await fixture.Client.Introspection.GetSearchStartUpTimeAsync(Token)
		};
		var distributed = await fixture.Client.Introspection.GetSearchDistributedAsync(null, Token);
		var saved = await fixture.Client.Introspection.GetSearchSavedAsync(Token);

		list.Entries.Select(e => e.Name).Should().Contain("indexer");
		indexer.Entries.Single().Content!.Status.Should().NotBeNullOrWhiteSpace();
		dispatch.Entries.Should().HaveCount(4);
		timings.Should().OnlyContain(t => t.Entries.Single().Content!.MaxTimeMs != null);
		distributed.Entries.Select(e => e.Name).Should().Contain("window_metrics");
		saved.Paging.Should().NotBeNull();
	}

	[Fact]
	public async Task KvStoreIntrospection_IsRead()
	{
		var list = await fixture.Client.KvStoreIntrospection.ListAsync(Token);
		var collections = await fixture.Client.KvStoreIntrospection.GetCollectionStatsAsync(Token);
		var server = await fixture.Client.KvStoreIntrospection.GetServerStatusAsync(Token);

		list.Entries.Select(e => e.Name).Should().Contain("serverstatus");
		collections.Entries.Single().Content!.Collections.Should().OnlyContain(c => c.Collection != null);
		server.Entries.Single().Content!.ParseData().Should().NotBeNull();
	}

	[Fact]
	public async Task ReplicaSetStats_WithoutMongo_IsUnavailable()
	{
		var act = () => fixture.Client.KvStoreIntrospection.GetReplicaSetStatsAsync(Token);

		(await act.Should().ThrowAsync<SplunkApiException>()).Which.StatusCode.Should().Be(HttpStatusCode.ServiceUnavailable);
	}

	[Fact]
	public async Task ServerStatus_IsRead()
	{
		var list = await fixture.Client.ServerStatus.ListAsync(Token);
		var artifacts = await fixture.Client.ServerStatus.GetDispatchArtifactsAsync(Token);
		var fishbucket = await fixture.Client.ServerStatus.GetFishbucketAsync(Token);
		var integrity = await fixture.Client.ServerStatus.GetInstalledFileIntegrityAsync(new FileIntegrityOptions { RegexFilter = "splunk_api_it_nothing" }, Token);
		var limits = await fixture.Client.ServerStatus.GetSearchConcurrencyLimitsAsync(Token);
		var partitions = await fixture.Client.ServerStatus.ListPartitionsSpaceAsync(null, Token);

		list.Entries.Select(e => e.Name).Should().Contain("dispatch-artifacts");
		artifacts.Entries.Should().ContainSingle();
		fishbucket.Entries.Single().Content!.KeyCount.Should().BePositive();
		integrity.Entries.Should().ContainSingle();
		limits.Entries.Single().Content!.MaxHistoricalSearches.Should().BePositive();
		partitions.Entries.Should().OnlyContain(p => p.Content!.CapacityMB > 0);
	}

	[Fact]
	public async Task ResourceUsageAndSysInfo_AreRead()
	{
		var list = await fixture.Client.ResourceUsage.ListAsync(null, Token);
		var host = await fixture.Client.ResourceUsage.GetHostwideAsync(Token);
		var io = await fixture.Client.ResourceUsage.ListIoStatsAsync(new ListOptions { Count = 2 }, Token);
		var processes = await fixture.Client.ResourceUsage.ListSplunkProcessesAsync(new ListOptions { Count = 2 }, Token);
		var sysinfo = await fixture.Client.SystemInfo.GetAsync(Token);

		list.Entries.Select(e => e.Name).Should().Contain("hostwide");
		host.Entries.Single().Content!.CpuCount.Should().BePositive();
		io.Entries.Should().NotBeEmpty();
		processes.Entries.Should().NotBeEmpty();
		sysinfo.Entries.Single().Content!.NumberOfCores.Should().BePositive();
	}
}
