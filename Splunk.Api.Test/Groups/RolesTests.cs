using Splunk.Api.Models.Access;
using Splunk.Api.Test.Support;

namespace Splunk.Api.Test.Groups;

public partial class RolesTests
{
	[Fact]
	public async Task ListAsync_SendsGet()
		=> (await RequestAssert.SendAsync((c, ct) => c.Roles.ListAsync(null, ct)))
			.ShouldBe(HttpMethod.Get, "/services/authorization/roles");

	[Fact]
	public async Task CreateAsync_PostsTheRole()
		=> (await RequestAssert.SendAsync((c, ct) => c.Roles.CreateAsync(
			new RoleCreateRequest
			{
				Name = "analyst",
				Capabilities = ["search", "rtsearch"],
				ImportedRoles = ["user"],
				GrantableRoles = ["user"],
				DefaultApp = "search",
				SearchIndexesAllowed = ["main", "web*"],
				SearchIndexesDefault = ["main"],
				SearchIndexesDisallowed = ["_audit"],
				DeleteIndexesAllowed = ["scratch"],
				SearchFilter = "host=web*",
				SearchJobsQuota = 5,
				RealTimeSearchJobsQuota = 2,
				CumulativeSearchJobsQuota = 50,
				CumulativeRealTimeSearchJobsQuota = 20,
				QueuedSearchQuota = 3,
				SearchDiskQuota = 200,
				SearchTimeWindow = 86400,
				SearchTimeEarliest = 604800,
				SearchFederatedProvidersAllowed = "*",
				SearchFederatedProvidersDefault = "fsh1;fsh2",
				FieldFilterExemption = ["mask_ssn"]
			},
			ct)))
			.ShouldBe(
				HttpMethod.Post,
				"/services/authorization/roles",
				"name=analyst&capabilities=search&capabilities=rtsearch&imported_roles=user&grantable_roles=user&defaultApp=search"
				+ "&srchIndexesAllowed=main&srchIndexesAllowed=web%2A&srchIndexesDefault=main&srchIndexesDisallowed=_audit&deleteIndexesAllowed=scratch"
				+ "&srchFilter=host%3Dweb%2A&srchJobsQuota=5&rtSrchJobsQuota=2&cumulativeSrchJobsQuota=50&cumulativeRTSrchJobsQuota=20&queuedSearchQuota=3"
				+ "&srchDiskQuota=200&srchTimeWin=86400&srchTimeEarliest=604800&srchFederatedProvidersAllowed=%2A&srchFederatedProvidersDefault=fsh1%3Bfsh2"
				+ "&fieldFilterExemption=mask_ssn");

	[Fact]
	public async Task GetAsync_SendsGetForTheName()
		=> (await RequestAssert.SendAsync((c, ct) => c.Roles.GetAsync("user", ct)))
			.ShouldBe(HttpMethod.Get, "/services/authorization/roles/user");

	[Fact]
	public async Task UpdateAsync_PostsTheChanges()
		=> (await RequestAssert.SendAsync((c, ct) => c.Roles.UpdateAsync("analyst", new RoleUpdateRequest { SearchJobsQuota = 7, Capabilities = ["search"] }, ct)))
			.ShouldBe(HttpMethod.Post, "/services/authorization/roles/analyst", "capabilities=search&srchJobsQuota=7");

	[Fact]
	public async Task DeleteAsync_SendsDelete()
		=> (await RequestAssert.SendAsync((c, ct) => c.Roles.DeleteAsync("analyst", ct)))
			.ShouldBe(HttpMethod.Delete, "/services/authorization/roles/analyst");

	[Fact]
	public Task Error_RaisesSplunkApiException()
		=> RequestAssert.ShouldRaiseSplunkErrorAsync((c, ct) => c.Roles.GetAsync("missing", ct));
}
