using Splunk.Api.Models.Access;
using Splunk.Api.Test.Support;

namespace Splunk.Api.Test.Groups;

public class LdapGroupsTests
{
	// Shaped after the reference (a live standalone instance has no LDAP strategy).
	private const string GroupsJson = """
		{
			"entry": [
				{
					"name": "Splunk Admins",
					"content": { "roles": ["admin"], "strategy": "corp_ldap", "type": "static", "users": ["alice", "bob"] }
				}
			],
			"messages": []
		}
		""";

	[Fact]
	public async Task ListAsync_SendsTheFilters()
		=> (await RequestAssert.SendAsync((c, ct) => c.LdapGroups.ListAsync(new LdapGroupListOptions { Strategy = "corp_ldap", LdapGroup = "admins", Count = 0 }, ct)))
			.ShouldBe(HttpMethod.Get, "/services/admin/LDAP-groups", query: "?strategy=corp_ldap&LDAPgroup=admins&count=0&output_mode=json");

	[Fact]
	public async Task UpdateAsync_PostsTheMapping()
		=> (await RequestAssert.SendAsync((c, ct) => c.LdapGroups.UpdateAsync(new LdapGroupUpdateRequest { Strategy = "corp_ldap", LdapGroup = "admins", Roles = ["admin", "power"] }, ct)))
			.ShouldBe(HttpMethod.Post, "/services/admin/LDAP-groups", "strategy=corp_ldap&LDAPgroup=admins&roles=admin&roles=power");

	[Fact]
	public async Task Content_MapsEveryModelledField()
	{
		var feed = await RequestAssert.ReadAsync((c, ct) => c.LdapGroups.ListAsync(null, ct), GroupsJson);

		var entry = feed.Entries.Should().ContainSingle().Subject;
		entry.Name.Should().Be("Splunk Admins");
		entry.Content!.Roles.Should().Equal("admin");
		entry.Content.Strategy.Should().Be("corp_ldap");
		entry.Content.Type.Should().Be("static");
		entry.Content.Users.Should().Equal("alice", "bob");
	}

	[Fact]
	public Task Error_RaisesSplunkApiException()
		=> RequestAssert.ShouldRaiseSplunkErrorAsync((c, ct) => c.LdapGroups.ListAsync(null, ct));
}
