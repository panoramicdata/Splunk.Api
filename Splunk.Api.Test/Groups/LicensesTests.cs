using Splunk.Api.Models.Licensing;
using Splunk.Api.Test.Support;

namespace Splunk.Api.Test.Groups;

public class LicensesTests
{
	// Captured from Splunk Enterprise 10.6.0.5 (GET licenser/licenses), trimmed; host replaced, features shortened.
	private const string LicensesJson = """
		{
			"links": { "create": "/services/licenser/licenses/_new" },
			"origin": "https://splunk.test:8089/services/licenser/licenses",
			"entry": [
				{
					"name": "5C52DA5145AD67B8188604C49962D12F2C3B2CF1B82A6878E46F68CA2812807B",
					"author": "system",
					"content": {
						"add_ons": { "hadoop": { "erp_type": "report", "guid": "6F416E61-B40E-461C-A782-CBC186E98133", "maxNodes": "200" } },
						"allowedRoles": [],
						"assignableRoles": [],
						"creation_time": 1571036400,
						"disabled_features": [],
						"eai:acl": null,
						"expiration_time": 1796649098,
						"features": ["Acceleration", "AdvancedSearchCommands"],
						"group_id": "Trial",
						"guid": "6F416E61-B40E-461C-A782-CBC186E98133",
						"is_unlimited": false,
						"label": "Splunk Enterprise   Splunk Analytics for Hadoop Download Trial",
						"license_hash": "5C52DA5145AD67B8188604C49962D12F2C3B2CF1B82A6878E46F68CA2812807B",
						"max_retention_size": 0,
						"max_stack_quota": 18446744073709552000,
						"max_users": 4294967295,
						"max_violations": 5,
						"notes": "",
						"quota": 524288000,
						"relative_expiration_interval": 5184000,
						"relative_expiration_start": 1791465098,
						"sourcetypes": [],
						"stack_id": "download-trial",
						"status": "VALID",
						"subgroup_id": "Production",
						"type": "download-trial",
						"window_period": 30
					}
				},
				{
					"name": "FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFD",
					"content": { "add_ons": null, "disabled_features": ["Acceleration"], "group_id": "Forwarder", "quota": 1048576 }
				}
			],
			"paging": { "total": 2, "perPage": 30, "offset": 0 },
			"messages": []
		}
		""";

	[Fact]
	public async Task ListAsync_SendsGet()
		=> (await EndpointRequests.SendAsync((c, ct) => c.Licenses.ListAsync(null, ct)))
			.ShouldBeEndpointRequest(HttpMethod.Get, "/services/licenser/licenses");

	[Fact]
	public async Task AddAsync_PostsThePayload()
		=> (await EndpointRequests.SendAsync((c, ct) => c.Licenses.AddAsync(new LicenseAddRequest { Name = "/tmp/x.lic", Payload = "<license/>" }, ct)))
			.ShouldBeEndpointRequest(HttpMethod.Post, "/services/licenser/licenses", "name=%2Ftmp%2Fx.lic&payload=%3Clicense%2F%3E");

	[Fact]
	public async Task GetAsync_SendsGetForTheHash()
		=> (await EndpointRequests.SendAsync((c, ct) => c.Licenses.GetAsync("ABC", ct)))
			.ShouldBeEndpointRequest(HttpMethod.Get, "/services/licenser/licenses/ABC");

	[Fact]
	public async Task DeleteAsync_SendsDelete()
		=> (await EndpointRequests.SendAsync((c, ct) => c.Licenses.DeleteAsync("ABC", ct)))
			.ShouldBeEndpointRequest(HttpMethod.Delete, "/services/licenser/licenses/ABC");

	[Fact]
	public async Task Content_MapsEveryModelledField()
	{
		var feed = await EndpointRequests.ReadAsync((c, ct) => c.Licenses.ListAsync(null, ct), LicensesJson);

		var license = feed.Entries[0].Content!;
		license.AddOns!["hadoop"].GetProperty("maxNodes").GetString().Should().Be("200");
		license.AllowedRoles.Should().BeEmpty();
		license.AssignableRoles.Should().BeEmpty();
		license.CreationTime.Should().Be(DateTimeOffset.FromUnixTimeSeconds(1571036400));
		license.DisabledFeatures.Should().BeEmpty();
		license.ExpirationTime.Should().Be(DateTimeOffset.FromUnixTimeSeconds(1796649098));
		license.Features.Should().Equal("Acceleration", "AdvancedSearchCommands");
		license.GroupId.Should().Be("Trial");
		license.LicenseGuid.Should().Be("6F416E61-B40E-461C-A782-CBC186E98133");
		license.IsUnlimited.Should().BeFalse();
		license.Label.Should().Be("Splunk Enterprise   Splunk Analytics for Hadoop Download Trial");
		license.LicenseHash.Should().Be("5C52DA5145AD67B8188604C49962D12F2C3B2CF1B82A6878E46F68CA2812807B");
		license.MaxRetentionSize.Should().Be(0);
		license.MaxStackQuota.Should().Be(18446744073709552000d);
		license.MaxUsers.Should().Be(4294967295);
		license.MaxViolations.Should().Be(5);
		license.Notes.Should().BeEmpty();
		license.Quota.Should().Be(524288000);
		license.RelativeExpirationInterval.Should().Be(5184000);
		license.RelativeExpirationStart.Should().Be(DateTimeOffset.FromUnixTimeSeconds(1791465098));
		license.SourceTypes.Should().BeEmpty();
		license.StackId.Should().Be("download-trial");
		license.Status.Should().Be("VALID");
		license.SubgroupId.Should().Be("Production");
		license.Type.Should().Be("download-trial");
		license.WindowPeriod.Should().Be(30);
		feed.Entries[1].Content!.AddOns.Should().BeNull();
		feed.Entries[1].Content!.DisabledFeatures.Should().Equal("Acceleration");
	}

	[Fact]
	public Task Error_RaisesSplunkApiException()
		=> EndpointRequests.ShouldRaiseSplunkErrorAsync((c, ct) => c.Licenses.GetAsync("missing", ct));
}
