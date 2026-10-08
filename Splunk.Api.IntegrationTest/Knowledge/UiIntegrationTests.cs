using Splunk.Api.Models;
using Splunk.Api.Models.Knowledge;

namespace Splunk.Api.IntegrationTest.Knowledge;

/// <summary>
/// Exercises the <c>data/ui/*</c> endpoints. The global banner is a shared singleton, so it is only read: writing it
/// would change what every user of the shared instance sees.
/// </summary>
[Collection(SplunkTestGroup.Name)]
public sealed class UiIntegrationTests(SplunkFixture fixture) : IDisposable
{
	private readonly SplunkClient _app = fixture.Client.InNamespace("nobody", "search");

	private static CancellationToken Token => TestContext.Current.CancellationToken;

	public void Dispose() => _app.Dispose();

	[Fact]
	public async Task GlobalBanner_IsTheSingleton()
	{
		var banner = (await fixture.Client.GlobalBanners.ListAsync(Token)).Entries.Should().ContainSingle().Subject;

		banner.Name.Should().Be(GlobalBannerCreateRequest.SingletonName);
		banner.Content!.BackgroundColor.Should().NotBeNullOrEmpty();
		banner.Content.Visible.Should().NotBeNull();
	}

	[Fact]
	public async Task Panel_CreateAndList()
	{
		var name = SplunkFixture.UniqueName("pn");
		try
		{
			await _app.Panels.CreateAsync(new PanelCreateRequest { Name = name, Data = "<panel><label>IT panel</label><title>IT</title></panel>" }, Token);

			var panel = (await _app.Panels.ListAsync(new ListOptions { Search = name }, Token)).Entries.Should().ContainSingle().Subject.Content!;
			panel.Label.Should().Be("IT panel");
			panel.Title.Should().Be("IT");
			panel.RootNode.Should().Be("panel");
		}
		finally
		{
			await Cleanup.DeleteUndocumentedAsync(fixture, "data/ui/panels", name);
		}
	}

	[Fact]
	public async Task View_CreateUpdateHistoryRevisionDisableEnableDelete()
	{
		var name = SplunkFixture.UniqueName("v");
		try
		{
			await _app.Views.CreateAsync(new ViewCreateRequest { Name = name, Data = Dashboard("v1") }, Token);
			await _app.Views.UpdateAsync(name, new ViewUpdateRequest { Data = Dashboard("v2"), ChangeLog = "second" }, Token);

			var view = (await _app.Views.GetAsync(name, Token)).Entries.Should().ContainSingle().Subject.Content!;
			view.Label.Should().Be("v2");
			view.IsDashboard.Should().BeTrue();
			view.DashboardType.Should().Be(0);

			var history = (await _app.Views.ListHistoryAsync(name, Token)).Entries;
			history.Should().HaveCount(2);
			history[0].Content!.Message.Should().Be("second");
			var withMessages = (await _app.Views.ListHistoryWithMessagesAsync(name, Token)).Entries;
			withMessages.Should().ContainSingle().Which.Content!.Sha.Should().Be(history[0].Content!.Sha);

			var first = (await _app.Views.GetRevisionAsync(name, history[1].Content!.Sha!, Token)).Entries.Should().ContainSingle().Subject.Content!;
			first.Data.Should().Be(Dashboard("v1"));

			(await _app.Views.DisableAsync(name, Token)).Entries.Should().ContainSingle().Which.Content!.Disabled.Should().BeTrue();
			(await _app.Views.EnableAsync(name, Token)).Entries.Should().ContainSingle().Which.Content!.Disabled.Should().BeFalse();
			(await _app.Views.ListAsync(new ListOptions { Search = name }, Token)).Entries.Should().ContainSingle();

			await _app.Views.DeleteAsync(name, new ViewDeleteRequest { ChangeLog = "integration test cleanup" }, Token);
		}
		finally
		{
			await Cleanup.IgnoreMissingAsync(() => _app.Views.DeleteAsync(name, CancellationToken.None));
		}

		await Cleanup.AssertGoneAsync(() => _app.Views.GetAsync(name, Token));
	}

	[Fact]
	public async Task View_DeleteWithoutChangeLog()
	{
		var name = SplunkFixture.UniqueName("v");
		try
		{
			await _app.Views.CreateAsync(new ViewCreateRequest { Name = name, Data = Dashboard("v1") }, Token);

			await _app.Views.DeleteAsync(name, Token);

			await Cleanup.AssertGoneAsync(() => _app.Views.GetAsync(name, Token));
		}
		finally
		{
			await Cleanup.IgnoreMissingAsync(() => _app.Views.DeleteAsync(name, CancellationToken.None));
		}
	}

	private static string Dashboard(string label) => $"<dashboard version=\"1.1\"><label>{label}</label></dashboard>";
}
