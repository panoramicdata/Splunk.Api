namespace Splunk.Api.IntegrationTest;

[Collection(SplunkTestGroup.Name)]
public class ServerInfoIntegrationTests(SplunkFixture fixture)
{
	[Fact]
	public async Task GetAsync_ReturnsTheServersVersion()
	{
		var feed = await fixture.Client.ServerInfo.GetAsync(TestContext.Current.CancellationToken);

		var info = feed.Entries.Should().ContainSingle().Subject.Content!;
		info.Version.Should().NotBeNullOrWhiteSpace();
		info.ServerName.Should().NotBeNullOrWhiteSpace();
		info.ServerRoles.Should().NotBeEmpty();
		feed.Generator!.Version.Should().Be(info.Version);
	}

	[Fact]
	public async Task WrongPassword_RaisesUnauthorized()
	{
		using var client = new SplunkClient(fixture.CreateOptions(o =>
		{
			o.Token = null;
			o.Username = "admin";
			o.Password = "definitely-not-the-password";
		}));

		var act = () => client.ServerInfo.GetAsync(TestContext.Current.CancellationToken);

		(await act.Should().ThrowAsync<SplunkApiException>()).Which.StatusCode.Should().Be(System.Net.HttpStatusCode.Unauthorized);
	}

	[Fact]
	public async Task BasicAuthentication_Works()
	{
		using var client = new SplunkClient(fixture.CreateOptions(o => o.UseBasicAuthentication = true));

		var feed = await client.ServerInfo.GetAsync(TestContext.Current.CancellationToken);

		feed.Entries.Should().ContainSingle();
	}
}
