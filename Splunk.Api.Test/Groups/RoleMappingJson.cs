namespace Splunk.Api.Test.Groups;

/// <summary>Role mapping responses shared by the ProxySSO and SAML mapping tests.</summary>
internal static class RoleMappingJson
{
	// Captured from Splunk Enterprise 10.6.0.5 (GET admin/ProxySSO-groups/{name}), trimmed; host replaced.
	public const string Feed = """
		{
			"links": { "create": "/services/admin/ProxySSO-groups/_new" },
			"origin": "https://splunk.test:8089/services/admin/ProxySSO-groups",
			"generator": { "build": "86587d4e3b27", "version": "10.6.0.5" },
			"entry": [
				{
					"name": "splunk_api_it_group",
					"id": "https://splunk.test:8089/services/admin/ProxySSO-groups/splunk_api_it_group",
					"links": { "alternate": "/services/admin/ProxySSO-groups/splunk_api_it_group", "remove": "/services/admin/ProxySSO-groups/splunk_api_it_group" },
					"author": "system",
					"acl": { "app": "", "owner": "system", "sharing": "system", "perms": { "read": ["admin"], "write": ["admin"] } },
					"fields": { "required": ["roles"], "optional": [], "wildcard": [] },
					"content": { "eai:acl": null, "roles": ["user", "power"] }
				}
			],
			"paging": { "total": 1, "perPage": 30, "offset": 0 },
			"messages": []
		}
		""";

	public static void ShouldBeTheCapturedMapping(this Models.SplunkFeed<Models.Access.RoleMapping> feed)
	{
		var entry = feed.Entries.Should().ContainSingle().Subject;
		entry.Name.Should().Be("splunk_api_it_group");
		entry.Content!.Roles.Should().Equal("user", "power");
	}
}
