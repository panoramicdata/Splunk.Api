using Splunk.Api.Models.Access;
using Splunk.Api.Test.Support;

namespace Splunk.Api.Test.Groups;

public class LdapStrategiesTests
{
	// Shaped after authentication.conf (a live standalone instance has no LDAP strategy).
	private const string StrategyJson = """
		{
			"entry": [
				{
					"name": "corp_ldap",
					"content": {
						"host": "ldap.example.com",
						"port": "636",
						"SSLEnabled": "1",
						"bindDN": "cn=splunk,dc=example,dc=com",
						"bindDNpassword": "********",
						"userBaseDN": "ou=people,dc=example,dc=com",
						"userBaseFilter": "(objectclass=person)",
						"userNameAttribute": "uid",
						"realNameAttribute": "cn",
						"emailAttribute": "mail",
						"groupBaseDN": "ou=groups,dc=example,dc=com",
						"groupBaseFilter": "(objectclass=group)",
						"groupNameAttribute": "cn",
						"groupMemberAttribute": "member",
						"groupMappingAttribute": "dn",
						"dynamicGroupFilter": "(objectclass=groupOfURLs)",
						"dynamicMemberAttribute": "memberURL",
						"nestedGroups": "0",
						"anonymous_referrals": "1",
						"sizelimit": "1000",
						"timelimit": "15",
						"network_timeout": "20",
						"disabled": "0"
					}
				}
			]
		}
		""";

	[Fact]
	public async Task ListAsync_SendsTheStrategyFilter()
		=> (await EndpointRequests.SendAsync((c, ct) => c.LdapStrategies.ListAsync(new LdapStrategyListOptions { Strategy = "corp_ldap" }, ct)))
			.ShouldBeEndpointRequest(HttpMethod.Get, "/services/authentication/providers/LDAP", query: "?strategy=corp_ldap&output_mode=json");

	[Fact]
	public async Task CreateAsync_PostsTheStrategy()
		=> (await EndpointRequests.SendAsync((c, ct) => c.LdapStrategies.CreateAsync(
			new LdapStrategyCreateRequest
			{
				Name = "corp",
				Host = "ldap",
				UserBaseDN = "ou=p",
				UserNameAttribute = "uid",
				RealNameAttribute = "cn",
				GroupBaseDN = "ou=g",
				GroupNameAttribute = "cn",
				GroupMemberAttribute = "member",
				Port = 636,
				SslEnabled = true,
				BindDN = "cn=s",
				BindDNPassword = "pw",
				UserBaseFilter = "uf",
				EmailAttribute = "mail",
				GroupBaseFilter = "gf",
				GroupMappingAttribute = "dn",
				DynamicGroupFilter = "dg",
				DynamicMemberAttribute = "dm",
				NestedGroups = false,
				AnonymousReferrals = true,
				SizeLimit = 100,
				TimeLimit = 10,
				NetworkTimeout = 20
			},
			ct)))
			.ShouldBeEndpointRequest(
				HttpMethod.Post,
				"/services/authentication/providers/LDAP",
				"name=corp&host=ldap&userBaseDN=ou%3Dp&userNameAttribute=uid&realNameAttribute=cn&groupBaseDN=ou%3Dg&groupNameAttribute=cn&groupMemberAttribute=member&port=636&SSLEnabled=true&bindDN=cn%3Ds&bindDNpassword=pw&userBaseFilter=uf&emailAttribute=mail&groupBaseFilter=gf&groupMappingAttribute=dn&dynamicGroupFilter=dg&dynamicMemberAttribute=dm&nestedGroups=false&anonymous_referrals=true&sizelimit=100&timelimit=10&network_timeout=20");

	[Fact]
	public async Task UpdateAsync_PostsTheChanges()
		=> (await EndpointRequests.SendAsync((c, ct) => c.LdapStrategies.UpdateAsync(
			"corp",
			new LdapStrategyUpdateRequest
			{
				Host = "ldap2",
				UserBaseDN = "u",
				UserNameAttribute = "n",
				RealNameAttribute = "r",
				GroupBaseDN = "g",
				GroupNameAttribute = "gn",
				GroupMemberAttribute = "gm",
				Port = 389
			},
			ct)))
			.ShouldBeEndpointRequest(
				HttpMethod.Post,
				"/services/authentication/providers/LDAP/corp",
				"port=389&host=ldap2&userBaseDN=u&userNameAttribute=n&realNameAttribute=r&groupBaseDN=g&groupNameAttribute=gn&groupMemberAttribute=gm");

	[Fact]
	public async Task DeleteAsync_SendsDelete()
		=> (await EndpointRequests.SendAsync((c, ct) => c.LdapStrategies.DeleteAsync("corp", ct)))
			.ShouldBeEndpointRequest(HttpMethod.Delete, "/services/authentication/providers/LDAP/corp");

	[Fact]
	public async Task EnableAsync_PostsToEnable()
		=> (await EndpointRequests.SendAsync((c, ct) => c.LdapStrategies.EnableAsync("corp", ct)))
			.ShouldBeEndpointRequest(HttpMethod.Post, "/services/authentication/providers/LDAP/corp/enable");

	[Fact]
	public async Task DisableAsync_PostsToDisable()
		=> (await EndpointRequests.SendAsync((c, ct) => c.LdapStrategies.DisableAsync("corp", ct)))
			.ShouldBeEndpointRequest(HttpMethod.Post, "/services/authentication/providers/LDAP/corp/disable");

	[Fact]
	public async Task Content_MapsEveryModelledField()
	{
		var feed = await EndpointRequests.ReadAsync((c, ct) => c.LdapStrategies.ListAsync(null, ct), StrategyJson);

		var ldap = feed.Entries.Should().ContainSingle().Subject.Content!;
		ldap.Host.Should().Be("ldap.example.com");
		ldap.Port.Should().Be(636);
		ldap.SslEnabled.Should().BeTrue();
		ldap.BindDN.Should().Be("cn=splunk,dc=example,dc=com");
		ldap.UserBaseDN.Should().Be("ou=people,dc=example,dc=com");
		ldap.UserBaseFilter.Should().Be("(objectclass=person)");
		ldap.UserNameAttribute.Should().Be("uid");
		ldap.RealNameAttribute.Should().Be("cn");
		ldap.EmailAttribute.Should().Be("mail");
		ldap.GroupBaseDN.Should().Be("ou=groups,dc=example,dc=com");
		ldap.GroupBaseFilter.Should().Be("(objectclass=group)");
		ldap.GroupNameAttribute.Should().Be("cn");
		ldap.GroupMemberAttribute.Should().Be("member");
		ldap.GroupMappingAttribute.Should().Be("dn");
		ldap.DynamicGroupFilter.Should().Be("(objectclass=groupOfURLs)");
		ldap.DynamicMemberAttribute.Should().Be("memberURL");
		ldap.NestedGroups.Should().BeFalse();
		ldap.AnonymousReferrals.Should().BeTrue();
		ldap.SizeLimit.Should().Be(1000);
		ldap.TimeLimit.Should().Be(15);
		ldap.NetworkTimeout.Should().Be(20);
		ldap.Disabled.Should().BeFalse();
		ldap.AdditionalProperties.Should().ContainKey("bindDNpassword");
	}

	[Fact]
	public Task Error_RaisesSplunkApiException()
		=> EndpointRequests.ShouldRaiseSplunkErrorAsync((c, ct) => c.LdapStrategies.DeleteAsync("missing", ct));
}
