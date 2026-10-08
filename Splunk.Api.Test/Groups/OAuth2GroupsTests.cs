using Splunk.Api.Models.Access;
using Splunk.Api.Test.Support;

namespace Splunk.Api.Test.Groups;

public class OAuth2GroupsTests
{
	// Shaped after the reference (the shared test instance has no OAuth configuration to map groups for).
	private const string MappingJson = """
		{ "entry": [ { "name": "splunk-admins", "content": { "config": "okta", "roles": ["admin", "power"] } } ] }
		""";

	[Fact]
	public async Task ListAsync_SendsTheConfigFilter()
		=> (await EndpointRequests.SendAsync((c, ct) => c.OAuth2Groups.ListAsync(new OAuth2GroupListOptions { Config = "okta" }, ct)))
			.ShouldBeEndpointRequest(HttpMethod.Get, "/services/admin/oauth2-groups", query: "?config=okta&output_mode=json");

	[Fact]
	public async Task CreateAsync_PostsTheMapping()
		=> (await EndpointRequests.SendAsync((c, ct) => c.OAuth2Groups.CreateAsync(new OAuth2GroupCreateRequest { Name = "admins", Config = "okta", Roles = ["admin", "power"] }, ct)))
			.ShouldBeEndpointRequest(HttpMethod.Post, "/services/admin/oauth2-groups", "name=admins&config=okta&roles=admin&roles=power");

	[Fact]
	public async Task Content_MapsEveryModelledField()
	{
		var feed = await EndpointRequests.ReadAsync((c, ct) => c.OAuth2Groups.ListAsync(null, ct), MappingJson);

		var entry = feed.Entries.Should().ContainSingle().Subject;
		entry.Name.Should().Be("splunk-admins");
		entry.Content!.Config.Should().Be("okta");
		entry.Content.Roles.Should().Equal("admin", "power");
	}

	[Fact]
	public Task Error_RaisesSplunkApiException()
		=> EndpointRequests.ShouldRaiseSplunkErrorAsync((c, ct) => c.OAuth2Groups.ListAsync(null, ct));
}
