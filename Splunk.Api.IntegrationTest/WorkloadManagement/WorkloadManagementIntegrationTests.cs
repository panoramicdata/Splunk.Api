using Splunk.Api.Models;
using Splunk.Api.Models.WorkloadManagement;
using System.Net;

namespace Splunk.Api.IntegrationTest.WorkloadManagement;

/// <summary>
/// Workload management reads. Enabling or disabling workload management, creating pools (the API has no pool delete), and
/// changing weights or policies would change the shared instance, so they are covered by unit tests only.
/// </summary>
[Collection(SplunkTestGroup.Name)]
public class WorkloadManagementIntegrationTests(SplunkFixture fixture)
{
	private static CancellationToken Token => TestContext.Current.CancellationToken;

	[Fact]
	public async Task Categories_AreSearchIngestAndMisc()
	{
		var feed = await fixture.Client.WorkloadCategories.ListAsync(new ListOptions { Count = 0 }, Token);

		feed.Entries.Select(e => e.Name).Should().BeEquivalentTo("ingest", "misc", "search");
		feed.Entries.Should().OnlyContain(e => e.Content!.CpuWeightSum > 0);
	}

	[Fact]
	public async Task PoolsAndRules_AreListed()
	{
		var pools = await fixture.Client.WorkloadPools.ListAsync(null, Token);
		var rules = await fixture.Client.WorkloadRules.ListAsync(null, Token);
		var admissionRules = await fixture.Client.WorkloadRules.ListAsync(new WorkloadRuleListOptions { WorkloadRuleType = "search_filter" }, Token);

		pools.Links.Should().ContainKey("create");
		rules.Links.Should().ContainKey("create");
		admissionRules.Links.Should().ContainKey("create");
	}

	[Fact]
	public async Task DeletingAMissingRule_IsNotFound()
	{
		var act = () => fixture.Client.WorkloadRules.DeleteAsync(SplunkFixture.UniqueName("rule"), null, Token);

		(await act.Should().ThrowAsync<SplunkApiException>()).Which.StatusCode.Should().Be(HttpStatusCode.NotFound);
	}

	[Fact]
	public async Task Config_ReportsTheBaseDirectoryAndPreflightChecks()
	{
		var directory = await fixture.Client.WorkloadConfig.GetBaseDirectoryAsync(Token);
		var preflight = await fixture.Client.WorkloadConfig.GetPreflightChecksAsync(Token);

		directory.Entries.Should().ContainSingle().Which.Content!.Name.Should().NotBeNullOrWhiteSpace();
		var checks = preflight.Entries.Should().ContainSingle().Subject.Content!;
		checks.General.Should().NotBeNull();
		checks.Checks.Should().ContainKey("platform_type").WhoseValue.Title.Should().NotBeNullOrWhiteSpace();
	}

	[Fact]
	public async Task PolicyAndStatus_AreRead()
	{
		var policy = await fixture.Client.WorkloadPolicy.GetSearchAdmissionControlAsync(Token);
		var status = await fixture.Client.WorkloadStatus.GetAsync(new WorkloadStatusOptions { Advanced = true }, Token);

		policy.Entries.Should().ContainSingle().Which.Content!.AdmissionRulesEnabled.Should().NotBeNull();
		status.Entries.Select(e => e.Name).Should().BeEquivalentTo("admission-control-status", "workload-management-status");
		status.Entries.Single(e => e.Name == "workload-management-status").Content!.General!.IsSupported.Should().NotBeNull();
	}
}
