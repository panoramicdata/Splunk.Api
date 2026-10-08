using Splunk.Api.Models;
using Splunk.Api.Models.Access;
using System.Net;

namespace Splunk.Api.IntegrationTest.Access;

[Collection(SplunkTestGroup.Name)]
public class SessionsIntegrationTests(SplunkFixture fixture)
{
	[Fact]
	public async Task LoginAsync_ThroughTheClientPipeline_ReturnsAUsableSessionKey()
	{
		var ct = TestContext.Current.CancellationToken;
		var credentials = fixture.CreateOptions();

		var login = await fixture.Client.Sessions.LoginAsync(new LoginRequest { Username = credentials.Username!, Password = credentials.Password! }, ct);

		login.SessionKey.Should().NotBeNullOrWhiteSpace();
		using var sessionClient = SessionKeyHandler.CreateClient(fixture, login.SessionKey!);
		var context = await sessionClient.CurrentContext.GetAsync(ct);
		context.Entries.Should().ContainSingle().Which.Content!.Username.Should().Be(credentials.Username);
	}

	[Fact]
	public async Task LoginAsync_IsAllowedByAReadOnlyClient()
	{
		var credentials = fixture.CreateOptions();
		using var client = new SplunkClient(fixture.CreateOptions(o => o.ReadOnly = true));

		var login = await client.Sessions.LoginAsync(new LoginRequest { Username = credentials.Username!, Password = credentials.Password!, Cookie = true }, TestContext.Current.CancellationToken);

		login.SessionKey.Should().NotBeNullOrWhiteSpace();
	}

	[Fact]
	public async Task LoginAsync_WrongPassword_RaisesUnauthorized()
	{
		var act = () => fixture.Client.Sessions.LoginAsync(new LoginRequest { Username = "admin", Password = "definitely-not-the-password" }, TestContext.Current.CancellationToken);

		(await act.Should().ThrowAsync<SplunkApiException>()).Which.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
	}

	[Fact]
	public async Task ListGetAndDelete_EndAUsersSession()
	{
		var ct = TestContext.Current.CancellationToken;
		var userName = SplunkFixture.UniqueName("session");
		var password = "It-" + Guid.NewGuid().ToString("N");
		await fixture.Client.Users.CreateAsync(new UserCreateRequest { Name = userName, Password = password, Roles = ["user"] }, ct);
		try
		{
			var login = await fixture.Client.Sessions.LoginAsync(new LoginRequest { Username = userName, Password = password }, ct);

			var sessions = await fixture.Client.Sessions.ListAsync(new ListOptions { Search = "userName=" + userName, Count = 0 }, ct);
			var session = sessions.Entries.Should().ContainSingle().Subject;
			session.Content!.UserName.Should().Be(userName);
			session.Content.AuthString.Should().MatchRegex("^[*]+$");
			session.Content.TimeAccessed.Should().NotBeNullOrWhiteSpace();

			var one = await fixture.Client.Sessions.GetAsync(session.Name, ct);
			one.Entries.Should().ContainSingle().Which.Content!.UserName.Should().Be(userName);

			await fixture.Client.Sessions.DeleteAsync(session.Name, ct);

			using var ended = SessionKeyHandler.CreateClient(fixture, login.SessionKey!);
			var act = () => ended.CurrentContext.GetAsync(ct);
			(await act.Should().ThrowAsync<SplunkApiException>()).Which.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
		}
		finally
		{
			await fixture.Client.Users.DeleteAsync(userName, ct);
		}
	}

	[Fact]
	public async Task GetAsync_UnknownSession_RaisesSplunkError()
	{
		var act = () => fixture.Client.Sessions.GetAsync("splunk_api_it_no_such_session", TestContext.Current.CancellationToken);

		(await act.Should().ThrowAsync<SplunkApiException>()).Which.Message.Should().Be("Not a valid session: splunk_api_it_no_such_session");
	}
}
