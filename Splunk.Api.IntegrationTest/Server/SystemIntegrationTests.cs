using Splunk.Api.Models;
using Splunk.Api.Models.Server;
using System.Net;

namespace Splunk.Api.IntegrationTest.Server;

/// <summary>
/// The system endpoints. Restart, restart_webui, rotate-splunk-secret, proxy changes and logger level changes would affect
/// everyone sharing the instance, so they are covered by unit tests only.
/// </summary>
[Collection(SplunkTestGroup.Name)]
public class SystemIntegrationTests(SplunkFixture fixture)
{
	private static CancellationToken Token => TestContext.Current.CancellationToken;

	[Fact]
	public async Task Message_RoundTrip()
	{
		var name = SplunkFixture.UniqueName("msg");
		var created = await fixture.Client.Messages.CreateAsync(
			new MessageCreateRequest { Name = name, Value = "Splunk.Api integration test", Severity = MessageSeverity.Info, Roles = ["admin"] },
			Token);
		try
		{
			created.Entries.Should().ContainSingle().Which.Content!.Severity.Should().Be(MessageSeverity.Info);

			var read = await fixture.Client.Messages.GetAsync(name, Token);
			var message = read.Entries.Should().ContainSingle().Subject.Content!;
			message.Message.Should().Be("Splunk.Api integration test");
			message.Roles.Should().Equal("admin");
			message.TimeCreated.Should().NotBeNull();

			var list = await fixture.Client.Messages.ListAsync(new ListOptions { Count = 0 }, Token);
			list.Entries.Select(e => e.Name).Should().Contain(name);
		}
		finally
		{
			await fixture.Client.Messages.DeleteAsync(name, CancellationToken.None);
		}

		var act = () => fixture.Client.Messages.DeleteAsync(name, Token);
		(await act.Should().ThrowAsync<SplunkApiException>()).Which.StatusCode.Should().Be(HttpStatusCode.NotFound);
	}

	[Fact]
	public async Task Loggers_ListAndGet()
	{
		var list = await fixture.Client.Loggers.ListAsync(new ListOptions { Count = 5 }, Token);
		var one = await fixture.Client.Loggers.GetAsync("TcpOutputProc", Token);

		list.Entries.Should().HaveCount(5);
		list.Paging!.Total.Should().BeGreaterThan(5);
		one.Entries.Should().ContainSingle().Which.Content!.Level.Should().NotBe(SplunkLogLevel.Unknown);
	}

	[Fact]
	public async Task ServerControl_ListsTheRestartAction()
	{
		var feed = await fixture.Client.ServerControl.ListAsync(Token);

		feed.Links.Should().ContainKeys("restart", "restart_webui");
	}

	[Fact]
	public async Task ServerRolesAndSettings_AreRead()
	{
		var roles = await fixture.Client.ServerRoles.GetAsync(Token);
		var settings = await fixture.Client.ServerSettings.GetAsync(Token);

		roles.Entries.Should().ContainSingle().Which.Content!.Roles.Should().NotBeEmpty();
		var content = settings.Entries.Should().ContainSingle().Subject.Content!;
		content.SplunkHome.Should().NotBeNullOrWhiteSpace();
		content.ManagementPort.Should().BePositive();
	}

	[Fact]
	public async Task ProxySettings_AreRead()
	{
		var feed = await fixture.Client.ProxySettings.GetAsync(Token);

		feed.Entries.Should().ContainSingle().Which.Name.Should().Be("proxyConfig");
	}
}
