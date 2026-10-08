using Splunk.Api.Models.Access;

namespace Splunk.Api.Test.Groups;

/// <summary>User responses shared by the user and current-context tests.</summary>
internal static class UserJson
{
	// Captured from Splunk Enterprise 10.6.0.5 (GET authentication/current-context), trimmed; host and email replaced.
	public const string Context = """
		{
			"links": {},
			"origin": "https://splunk.test:8089/services/authentication/current-context",
			"generator": { "build": "86587d4e3b27", "version": "10.6.0.5" },
			"entry": [
				{
					"name": "context",
					"author": "system",
					"acl": { "app": "", "owner": "system", "sharing": "system", "perms": { "read": ["*"], "write": ["*"] } },
					"content": {
						"capabilities": ["accelerate_datamodel", "admin_all_objects", "search"],
						"defaultApp": "launcher",
						"defaultAppIsUserOverride": false,
						"defaultAppSourceRole": "system",
						"display_new_search_banner": true,
						"eai:acl": null,
						"email": "admin@example.com",
						"lang": "",
						"last_successful_login": 1791467392,
						"locked-out": false,
						"olly_org": "",
						"password": "********",
						"realname": "Administrator",
						"restart_background_jobs": null,
						"roles": ["admin"],
						"search_assistant": "compact",
						"theme": "default_system_theme",
						"type": "Splunk",
						"tz": "",
						"username": "admin"
					}
				}
			],
			"paging": { "total": 1, "perPage": 30, "offset": 0 },
			"messages": []
		}
		""";

	/// <summary>The same content as a user entry (<c>authentication/users</c>).</summary>
	public static string User => Context.Replace("\"name\": \"context\"", "\"name\": \"admin\"", StringComparison.Ordinal);

	public static void ShouldBeTheCapturedAdmin(this UserProperties user)
	{
		user.Capabilities.Should().Equal("accelerate_datamodel", "admin_all_objects", "search");
		user.Roles.Should().Equal("admin");
		user.RealName.Should().Be("Administrator");
		user.Email.Should().Be("admin@example.com");
		user.DefaultApp.Should().Be("launcher");
		user.DefaultAppIsUserOverride.Should().BeFalse();
		user.DefaultAppSourceRole.Should().Be("system");
		user.Type.Should().Be("Splunk");
		user.TimeZone.Should().BeEmpty();
		user.Language.Should().BeEmpty();
		user.LockedOut.Should().BeFalse();
		user.RestartBackgroundJobs.Should().BeNull();
		user.LastSuccessfulLogin.Should().Be(DateTimeOffset.FromUnixTimeSeconds(1791467392));
		user.Password.Should().Be("********");
		user.Theme.Should().Be("default_system_theme");
		user.AdditionalProperties["search_assistant"].GetString().Should().Be("compact");
	}
}
