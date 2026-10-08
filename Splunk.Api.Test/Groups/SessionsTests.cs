using Splunk.Api.Models;
using Splunk.Api.Models.Access;
using Splunk.Api.Test.Support;
using System.Net;

namespace Splunk.Api.Test.Groups;

public class SessionsTests
{
	// Captured from Splunk Enterprise 10.6.0.5 (POST auth/login?output_mode=json); key replaced.
	private const string LoginJson = """{"sessionKey":"fake-session-key","message":"","code":""}""";

	// Captured from Splunk Enterprise 10.6.0.5 (GET authentication/httpauth-tokens), trimmed; host and names replaced.
	private const string SessionsJson = """
		{
			"links": {},
			"origin": "https://splunk.test:8089/services/authentication/httpauth-tokens",
			"generator": { "build": "86587d4e3b27", "version": "10.6.0.5" },
			"entry": [
				{
					"name": "00000000000000000000000000000001",
					"id": "https://splunk.test:8089/services/authentication/httpauth-tokens/00000000000000000000000000000001",
					"links": { "alternate": "/services/authentication/httpauth-tokens/00000000000000000000000000000001", "remove": "/services/authentication/httpauth-tokens/00000000000000000000000000000001" },
					"author": "system",
					"acl": { "app": "", "owner": "system", "sharing": "system", "perms": { "read": ["admin", "splunk-system-role"], "write": ["admin", "splunk-system-role"] } },
					"content": { "authString": "******", "eai:acl": null, "searchId": "1791467314.6", "timeAccessed": "Thu Oct  8 13:48:34 2026", "userName": "admin" }
				}
			],
			"paging": { "total": 1, "perPage": 30, "offset": 0 },
			"messages": []
		}
		""";

	[Fact]
	public async Task LoginAsync_PostsTheCredentials()
		=> (await RequestAssert.SendAsync(
			(c, ct) => c.Sessions.LoginAsync(new LoginRequest { Username = "jo", Password = "p&ss", Passcode = "123456", Cookie = true }, ct),
			LoginJson))
			.ShouldBe(HttpMethod.Post, "/services/auth/login", "username=jo&password=p%26ss&passcode=123456&cookie=true");

	[Fact]
	public async Task LoginAsync_ReturnsTheSessionKey()
	{
		var login = await RequestAssert.ReadAsync((c, ct) => c.Sessions.LoginAsync(new LoginRequest { Username = "jo", Password = "pw" }, ct), LoginJson);

		login.SessionKey.Should().Be("fake-session-key");
		login.Message.Should().BeEmpty();
		login.Code.Should().BeEmpty();
	}

	[Fact]
	public async Task LoginAsync_WithASessionClient_IsSentWithoutLoggingInFirst()
	{
		var stub = TestClient.Stub(LoginJson);
		using var client = TestClient.Create(stub, o =>
		{
			o.Token = null;
			o.Username = "admin";
			o.Password = "client-password";
			o.ReadOnly = true;
		});

		var login = await client.Sessions.LoginAsync(new LoginRequest { Username = "other", Password = "pw" }, TestContext.Current.CancellationToken);

		login.SessionKey.Should().Be("fake-session-key");
		var call = stub.Calls.Should().ContainSingle().Subject;
		call.Body.Should().Be("username=other&password=pw");
		call.Headers.Authorization.Should().BeNull();
	}

	[Fact]
	public async Task ListAsync_SendsGet()
		=> (await RequestAssert.SendAsync((c, ct) => c.Sessions.ListAsync(new ListOptions { Search = "userName=admin" }, ct)))
			.ShouldBe(HttpMethod.Get, "/services/authentication/httpauth-tokens", query: "?search=userName%3Dadmin&output_mode=json");

	[Fact]
	public async Task GetAsync_SendsGetForTheName()
		=> (await RequestAssert.SendAsync((c, ct) => c.Sessions.GetAsync("abc", ct)))
			.ShouldBe(HttpMethod.Get, "/services/authentication/httpauth-tokens/abc");

	[Fact]
	public async Task DeleteAsync_SendsDelete()
		=> (await RequestAssert.SendAsync((c, ct) => c.Sessions.DeleteAsync("abc", ct)))
			.ShouldBe(HttpMethod.Delete, "/services/authentication/httpauth-tokens/abc");

	[Fact]
	public async Task Content_MapsEveryModelledField()
	{
		var feed = await RequestAssert.ReadAsync((c, ct) => c.Sessions.ListAsync(null, ct), SessionsJson);

		var entry = feed.Entries.Should().ContainSingle().Subject;
		entry.Name.Should().Be("00000000000000000000000000000001");
		entry.Content!.AuthString.Should().Be("******");
		entry.Content.SearchId.Should().Be("1791467314.6");
		entry.Content.TimeAccessed.Should().Be("Thu Oct  8 13:48:34 2026");
		entry.Content.UserName.Should().Be("admin");
	}

	[Fact]
	public Task Error_RaisesSplunkApiException()
		=> RequestAssert.ShouldRaiseSplunkErrorAsync((c, ct) => c.Sessions.LoginAsync(new LoginRequest { Username = "jo", Password = "wrong" }, ct), HttpStatusCode.Unauthorized);
}
