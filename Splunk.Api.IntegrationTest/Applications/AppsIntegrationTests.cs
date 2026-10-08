using Splunk.Api.Models;
using Splunk.Api.Models.Applications;
using System.Net;

namespace Splunk.Api.IntegrationTest.Applications;

[Collection(SplunkTestGroup.Name)]
public class AppsIntegrationTests(SplunkFixture fixture)
{
	[Fact]
	public async Task ListAsync_IncludesTheSearchApp()
	{
		var feed = await fixture.Client.Apps.ListAsync(new ListOptions { Count = 0 }, TestContext.Current.CancellationToken);

		var search = feed.Entries.Should().ContainSingle(e => e.Name == "search").Subject.Content!;
		search.Label.Should().Be("Search & Reporting");
		search.Core.Should().BeTrue();
		search.Visible.Should().BeTrue();
		search.Version.Should().NotBeNullOrWhiteSpace();
	}

	[Fact]
	public async Task GetSetupAndCheckForUpdate_ReadTheSearchApp()
	{
		var ct = TestContext.Current.CancellationToken;

		var app = await fixture.Client.Apps.GetAsync("search", new AppGetOptions { Refresh = false }, ct);
		var setup = await fixture.Client.Apps.GetSetupAsync("search", ct);
		var update = await fixture.Client.Apps.CheckForUpdateAsync("search", ct);

		app.Entries.Should().ContainSingle().Which.Content!.Author.Should().Be("Splunk");
		setup.Entries.Should().ContainSingle().Which.Content!.Setup.Should().Contain("<SetupInfo>");
		update.Entries.Should().ContainSingle().Which.Content!.UpdateVersion.Should().BeNull("a core app has no Splunkbase update");
	}

	[Fact]
	public async Task AppTemplates_ListAndGet()
	{
		var ct = TestContext.Current.CancellationToken;

		var templates = await fixture.Client.AppTemplates.ListAsync(null, ct);
		var barebones = await fixture.Client.AppTemplates.GetAsync("barebones", ct);

		templates.Entries.Select(e => e.Name).Should().Contain(["barebones", "sample_app"]);
		barebones.Entries.Should().ContainSingle().Which.Name.Should().Be("barebones");
	}

	[Fact]
	public async Task CreateFromTemplateUpdateDelete_RoundTrips()
	{
		var ct = TestContext.Current.CancellationToken;
		var name = SplunkFixture.UniqueName("app");
		try
		{
			var created = await fixture.Client.Apps.CreateAsync(
				new AppCreateRequest
				{
					Name = name,
					Template = "barebones",
					Label = "Splunk.Api integration test",
					Description = "Created and deleted by Splunk.Api integration tests",
					Author = "splunk_api_it",
					Version = "1.0.0",
					Visible = false
				},
				ct);
			var app = created.Entries.Should().ContainSingle().Subject.Content!;
			app.Name.Should().Be(name);
			app.Label.Should().Be("Splunk.Api integration test");
			app.Visible.Should().BeFalse();
			app.Core.Should().BeFalse();

			var updated = await fixture.Client.Apps.UpdateAsync(name, new AppUpdateRequest { Description = "Updated", CheckForUpdates = false, Version = "1.0.1" }, ct);
			var after = updated.Entries.Should().ContainSingle().Subject.Content!;
			after.Description.Should().Be("Updated");
			after.CheckForUpdates.Should().BeFalse();
			after.Version.Should().Be("1.0.1");

			(await fixture.Client.Apps.GetSetupAsync(name, ct)).Entries.Should().ContainSingle();
			(await fixture.Client.Apps.CheckForUpdateAsync(name, ct)).Entries.Should().ContainSingle().Which.Name.Should().Be(name);
		}
		finally
		{
			await fixture.Client.Apps.DeleteAsync(name, ct);
		}

		var act = () => fixture.Client.Apps.GetAsync(name, null, ct);
		(await act.Should().ThrowAsync<SplunkApiException>()).Which.StatusCode.Should().Be(HttpStatusCode.NotFound);
	}
}
