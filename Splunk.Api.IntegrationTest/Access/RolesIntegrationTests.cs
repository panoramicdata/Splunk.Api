using Splunk.Api.Models;
using Splunk.Api.Models.Access;
using System.Net;

namespace Splunk.Api.IntegrationTest.Access;

[Collection(SplunkTestGroup.Name)]
public class RolesIntegrationTests(SplunkFixture fixture)
{
	[Fact]
	public async Task ListAsync_IncludesTheBuiltInRoles()
	{
		var feed = await fixture.Client.Roles.ListAsync(new ListOptions { Count = 0 }, TestContext.Current.CancellationToken);

		feed.Entries.Select(e => e.Name).Should().Contain(["admin", "power", "user"]);
		var admin = feed.Entries.Single(e => e.Name == "admin").Content!;
		admin.ImportedRoles.Should().Contain("user");
		admin.Capabilities.Should().Contain("admin_all_objects");
		admin.SearchIndexesAllowed.Should().NotBeEmpty();
	}

	[Fact]
	public async Task Capabilities_ListAllAndGrantable()
	{
		var ct = TestContext.Current.CancellationToken;

		var all = (await fixture.Client.Capabilities.ListAsync(ct)).Entries.Should().ContainSingle().Subject;
		var grantable = (await fixture.Client.Capabilities.ListGrantableAsync(ct)).Entries.Should().ContainSingle().Subject;

		all.Name.Should().Be("capabilities");
		all.Content!.Capabilities.Should().Contain(["search", "admin_all_objects", "edit_user"]);
		grantable.Content!.Capabilities.Should().Contain("search");
	}

	[Fact]
	public async Task CreateGetUpdateDelete_RoundTrips()
	{
		var ct = TestContext.Current.CancellationToken;
		var name = SplunkFixture.UniqueName("role");
		try
		{
			await fixture.Client.Roles.CreateAsync(
				new RoleCreateRequest
				{
					Name = name,
					Capabilities = ["list_settings", "search"],
					ImportedRoles = ["user"],
					SearchIndexesAllowed = ["main", "_internal"],
					SearchIndexesDefault = ["main"],
					SearchFilter = "sourcetype=splunkd",
					SearchJobsQuota = 4,
					RealTimeSearchJobsQuota = 1,
					SearchDiskQuota = 50,
					SearchTimeWindow = 86400
				},
				ct);

			var role = (await fixture.Client.Roles.GetAsync(name, ct)).Entries.Should().ContainSingle().Subject.Content!;
			role.Capabilities.Should().Equal(["list_settings"], "Splunk drops capabilities the role already imports");
			role.ImportedRoles.Should().Equal("user");
			role.ImportedCapabilities.Should().NotBeEmpty();
			role.SearchIndexesAllowed.Order().Should().Equal("_internal", "main");
			role.SearchIndexesDefault.Should().Equal("main");
			role.SearchFilter.Should().Be("sourcetype=splunkd");
			role.SearchJobsQuota.Should().Be(4);
			role.RealTimeSearchJobsQuota.Should().Be(1);
			role.SearchDiskQuota.Should().Be(50);
			role.SearchTimeWindow.Should().Be(86400);

			var updated = await fixture.Client.Roles.UpdateAsync(name, new RoleUpdateRequest { SearchJobsQuota = 6, Capabilities = ["list_settings", "edit_tcp"] }, ct);
			updated.Entries.Should().ContainSingle().Which.Content!.SearchJobsQuota.Should().Be(6);
			updated.Entries[0].Content!.Capabilities.Order().Should().Equal("edit_tcp", "list_settings");

			// The reference lists imported_* parameters for this POST; Splunk 10.6 computes them and refuses them.
			var imported = new RoleUpdateRequest();
			imported.AdditionalParameters["imported_srchJobsQuota"] = "5";
			var refuse = () => fixture.Client.Roles.UpdateAsync(name, imported, ct);
			(await refuse.Should().ThrowAsync<SplunkApiException>()).Which.Message.Should().Be("Argument \"imported_srchJobsQuota\" is not supported by this handler.");
		}
		finally
		{
			await fixture.Client.Roles.DeleteAsync(name, ct);
		}

		var act = () => fixture.Client.Roles.GetAsync(name, ct);
		(await act.Should().ThrowAsync<SplunkApiException>()).Which.StatusCode.Should().Be(HttpStatusCode.NotFound);
	}
}
