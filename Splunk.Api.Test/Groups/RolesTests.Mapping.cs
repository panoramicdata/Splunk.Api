using Splunk.Api.Test.Support;

namespace Splunk.Api.Test.Groups;

public partial class RolesTests
{
	// Captured from Splunk Enterprise 10.6.0.5 (GET authorization/roles/admin), trimmed; host replaced.
	private const string RoleJson = """
		{
			"links": { "create": "/services/authorization/roles/_new" },
			"origin": "https://splunk.test:8089/services/authorization/roles",
			"entry": [
				{
					"name": "admin",
					"author": "system",
					"acl": { "app": "", "owner": "system", "sharing": "system", "removable": false, "perms": { "read": ["*"], "write": ["*"] } },
					"content": {
						"capabilities": ["accelerate_datamodel", "admin_all_objects"],
						"cumulativeRTSrchJobsQuota": 400,
						"cumulativeSrchJobsQuota": 200,
						"defaultApp": "",
						"deleteIndexesAllowed": [],
						"eai:acl": null,
						"federatedProviders": [],
						"fieldFilterExemption": ["mask_ssn"],
						"grantable_roles": [],
						"imported_capabilities": ["accelerate_search", "change_own_password"],
						"imported_queuedSearchQuota": 0,
						"imported_roles": ["power", "user"],
						"imported_rtSrchJobsQuota": 20,
						"imported_srchDiskQuota": 500,
						"imported_srchFilter": "",
						"imported_srchIndexesAllowed": ["*"],
						"imported_srchIndexesDefault": ["main"],
						"imported_srchIndexesDisallowed": [],
						"imported_srchJobsQuota": 10,
						"imported_srchTimeEarliest": -1,
						"imported_srchTimeWin": -1,
						"kvstore_create.deny_list": [],
						"queuedSearchQuota": 0,
						"rtSrchJobsQuota": 100,
						"srchAllowAdvancedCron": true,
						"srchDiskQuota": 10000,
						"srchFederatedProvidersAllowed": ["*"],
						"srchFederatedProvidersDefault": ["*"],
						"srchFilter": "*",
						"srchIndexesAllowed": ["*", "_*"],
						"srchIndexesDefault": ["main", "os"],
						"srchIndexesDisallowed": [],
						"srchJobsQuota": 50,
						"srchMinScheduleInterval": 0,
						"srchTimeEarliest": 0,
						"srchTimeWin": 0
					}
				}
			],
			"paging": { "total": 1, "perPage": 30, "offset": 0 },
			"messages": []
		}
		""";

	[Fact]
	public async Task Content_MapsEveryModelledField()
	{
		var feed = await RequestAssert.ReadAsync((c, ct) => c.Roles.GetAsync("admin", ct), RoleJson);

		var role = feed.Entries.Should().ContainSingle().Subject.Content!;
		role.Capabilities.Should().Equal("accelerate_datamodel", "admin_all_objects");
		role.ImportedCapabilities.Should().Equal("accelerate_search", "change_own_password");
		role.ImportedRoles.Should().Equal("power", "user");
		role.GrantableRoles.Should().BeEmpty();
		role.DefaultApp.Should().BeEmpty();
		role.SearchIndexesAllowed.Should().Equal("*", "_*");
		role.SearchIndexesDefault.Should().Equal("main", "os");
		role.SearchIndexesDisallowed.Should().BeEmpty();
		role.DeleteIndexesAllowed.Should().BeEmpty();
		role.SearchFilter.Should().Be("*");
		role.SearchJobsQuota.Should().Be(50);
		role.RealTimeSearchJobsQuota.Should().Be(100);
		role.CumulativeSearchJobsQuota.Should().Be(200);
		role.CumulativeRealTimeSearchJobsQuota.Should().Be(400);
		role.QueuedSearchQuota.Should().Be(0);
		role.SearchDiskQuota.Should().Be(10000);
		role.SearchTimeWindow.Should().Be(0);
		role.SearchTimeEarliest.Should().Be(0);
		role.SearchMinScheduleInterval.Should().Be(0);
		role.SearchAllowAdvancedCron.Should().BeTrue();
		role.SearchFederatedProvidersAllowed.Should().Equal("*");
		role.SearchFederatedProvidersDefault.Should().Equal("*");
		role.FieldFilterExemption.Should().Equal("mask_ssn");
		role.ImportedSearchIndexesAllowed.Should().Equal("*");
		role.ImportedSearchIndexesDefault.Should().Equal("main");
		role.ImportedSearchIndexesDisallowed.Should().BeEmpty();
		role.ImportedSearchFilter.Should().BeEmpty();
		role.ImportedSearchJobsQuota.Should().Be(10);
		role.ImportedRealTimeSearchJobsQuota.Should().Be(20);
		role.ImportedSearchDiskQuota.Should().Be(500);
		role.ImportedSearchTimeWindow.Should().Be(-1);
		role.ImportedSearchTimeEarliest.Should().Be(-1);
		role.AdditionalProperties.Should().ContainKey("kvstore_create.deny_list");
	}
}
