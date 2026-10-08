using Splunk.Api.Models.Licensing;
using Splunk.Api.Test.Support;

namespace Splunk.Api.Test.Groups;

public class LicenseGroupsTests
{
	// Captured from Splunk Enterprise 10.6.0.5 (GET licenser/groups), trimmed; host replaced.
	private const string GroupsJson = """
		{
			"links": {},
			"origin": "https://splunk.test:8089/services/licenser/groups",
			"entry": [
				{ "name": "Enterprise", "author": "system", "content": { "eai:acl": null, "is_active": false, "stack_ids": [] } },
				{ "name": "Trial", "author": "system", "content": { "eai:acl": null, "is_active": true, "stack_ids": ["download-trial"] } }
			],
			"paging": { "total": 2, "perPage": 30, "offset": 0 },
			"messages": []
		}
		""";

	[Fact]
	public async Task ListAsync_SendsGet()
		=> (await EndpointRequests.SendAsync((c, ct) => c.LicenseGroups.ListAsync(null, ct)))
			.ShouldBeEndpointRequest(HttpMethod.Get, "/services/licenser/groups");

	[Fact]
	public async Task GetAsync_SendsGetForTheName()
		=> (await EndpointRequests.SendAsync((c, ct) => c.LicenseGroups.GetAsync("Trial", ct)))
			.ShouldBeEndpointRequest(HttpMethod.Get, "/services/licenser/groups/Trial");

	[Fact]
	public async Task UpdateAsync_PostsIsActive()
		=> (await EndpointRequests.SendAsync((c, ct) => c.LicenseGroups.UpdateAsync("Enterprise", new LicenseGroupUpdateRequest { IsActive = true }, ct)))
			.ShouldBeEndpointRequest(HttpMethod.Post, "/services/licenser/groups/Enterprise", "is_active=true");

	[Fact]
	public async Task Content_MapsEveryModelledField()
	{
		var feed = await EndpointRequests.ReadAsync((c, ct) => c.LicenseGroups.ListAsync(null, ct), GroupsJson);

		feed.Entries.Should().HaveCount(2);
		feed.Entries[0].Content!.IsActive.Should().BeFalse();
		feed.Entries[0].Content!.StackIds.Should().BeEmpty();
		feed.Entries[1].Name.Should().Be("Trial");
		feed.Entries[1].Content!.IsActive.Should().BeTrue();
		feed.Entries[1].Content!.StackIds.Should().Equal("download-trial");
	}

	[Fact]
	public Task Error_RaisesSplunkApiException()
		=> EndpointRequests.ShouldRaiseSplunkErrorAsync((c, ct) => c.LicenseGroups.GetAsync("missing", ct));
}
