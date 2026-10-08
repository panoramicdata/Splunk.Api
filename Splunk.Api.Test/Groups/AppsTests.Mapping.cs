using Splunk.Api.Test.Support;

namespace Splunk.Api.Test.Groups;

public partial class AppsTests
{
	// Captured from Splunk Enterprise 10.6.0.5 (GET apps/local/search, and POST apps/local for a throwaway app), trimmed; host
	// replaced and a details URL added (no installed app has one).
	private const string AppsJson = """
		{
			"links": { "create": "/services/apps/local/_new", "_reload": "/services/apps/local/_reload" },
			"origin": "https://splunk.test:8089/services/apps/local",
			"entry": [
				{
					"name": "search",
					"links": { "alternate": "/servicesNS/nobody/system/apps/local/search", "package": "/servicesNS/nobody/system/apps/local/search/package" },
					"author": "nobody",
					"acl": { "app": "system", "owner": "nobody", "sharing": "app", "perms": { "read": ["*"], "write": ["admin", "power"] } },
					"content": {
						"author": "Splunk",
						"check_for_updates": true,
						"configured": true,
						"core": true,
						"description": "The Search app is Splunk's default interface for searching and analyzing IT data.",
						"disabled": false,
						"eai:acl": null,
						"label": "Search & Reporting",
						"managed_by_deployment_client": false,
						"show_in_nav": true,
						"state_change_requires_restart": false,
						"supported_themes": "light,dark",
						"version": "10.6.0.5",
						"visible": true
					}
				},
				{
					"name": "splunk_api_it_app",
					"author": "nobody",
					"content": {
						"author": "splunk_api_it",
						"check_for_updates": true,
						"configured": false,
						"core": false,
						"description": "probe",
						"details": "https://splunkbase.splunk.com/app/0",
						"disabled": false,
						"label": "Splunk API probe",
						"managed_by_deployment_client": false,
						"name": "splunk_api_it_app",
						"show_in_nav": true,
						"state_change_requires_restart": false,
						"version": "1.0.0",
						"visible": false
					}
				}
			],
			"paging": { "total": 2, "perPage": 30, "offset": 0 },
			"messages": []
		}
		""";

	// Captured from Splunk Enterprise 10.6.0.5 (GET apps/local/search/setup), trimmed.
	private const string SetupJson = """
		{ "entry": [ { "name": "search", "content": { "eai:acl": null, "eai:setup": "<?xml version=\"1.0\" encoding=\"UTF-8\"?>\n<SetupInfo>\n  <block>\n    <text>setup_stub</text>\n  </block>\n</SetupInfo>\n" } } ] }
		""";

	// The update keys as Splunk returns them when Splunkbase has a newer version (a live check returns only eai:acl).
	private const string UpdateJson = """
		{
			"entry": [
				{
					"name": "my_app",
					"content": {
						"eai:acl": null,
						"update.name": "My App",
						"update.version": "2.0.0",
						"update.homepage": "https://splunkbase.splunk.com/app/1234",
						"update.appurl": "https://splunkbase.splunk.com/app/1234/release/2.0.0/download",
						"update.size": "102400",
						"update.checksum": "d41d8cd98f00b204e9800998ecf8427e",
						"update.checksum.type": "md5",
						"update.implicit_id_required": "0"
					}
				}
			]
		}
		""";

	[Fact]
	public async Task Content_MapsEveryModelledField()
	{
		var feed = await RequestAssert.ReadAsync((c, ct) => c.Apps.ListAsync(null, ct), AppsJson);

		feed.Entries.Should().HaveCount(2);
		var search = feed.Entries[0].Content!;
		search.Author.Should().Be("Splunk");
		search.CheckForUpdates.Should().BeTrue();
		search.Configured.Should().BeTrue();
		search.Core.Should().BeTrue();
		search.Description.Should().StartWith("The Search app");
		search.Disabled.Should().BeFalse();
		search.Label.Should().Be("Search & Reporting");
		search.ManagedByDeploymentClient.Should().BeFalse();
		search.ShowInNav.Should().BeTrue();
		search.StateChangeRequiresRestart.Should().BeFalse();
		search.SupportedThemes.Should().Be("light,dark");
		search.Version.Should().Be("10.6.0.5");
		search.Visible.Should().BeTrue();
		var created = feed.Entries[1].Content!;
		created.Name.Should().Be("splunk_api_it_app");
		created.Details.Should().Be("https://splunkbase.splunk.com/app/0");
		created.Visible.Should().BeFalse();
	}

	[Fact]
	public async Task Setup_MapsTheSetupXml()
	{
		var feed = await RequestAssert.ReadAsync((c, ct) => c.Apps.GetSetupAsync("search", ct), SetupJson);

		feed.Entries.Should().ContainSingle().Which.Content!.Setup.Should().Contain("<text>setup_stub</text>");
	}

	[Fact]
	public async Task UpdateCheck_MapsEveryModelledField()
	{
		var feed = await RequestAssert.ReadAsync((c, ct) => c.Apps.CheckForUpdateAsync("my_app", ct), UpdateJson);

		var update = feed.Entries.Should().ContainSingle().Subject.Content!;
		update.UpdateName.Should().Be("My App");
		update.UpdateVersion.Should().Be("2.0.0");
		update.UpdateHomepage.Should().Be("https://splunkbase.splunk.com/app/1234");
		update.UpdateAppUrl.Should().Be("https://splunkbase.splunk.com/app/1234/release/2.0.0/download");
		update.UpdateSize.Should().Be(102400);
		update.UpdateChecksum.Should().Be("d41d8cd98f00b204e9800998ecf8427e");
		update.UpdateChecksumType.Should().Be("md5");
		update.UpdateImplicitIdRequired.Should().BeFalse();
	}
}
