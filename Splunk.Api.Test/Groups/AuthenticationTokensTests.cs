using Splunk.Api.Models.Access;
using Splunk.Api.Test.Support;
using System.Net;

namespace Splunk.Api.Test.Groups;

public class AuthenticationTokensTests
{
	// Captured from Splunk Enterprise 10.6.0.5 (GET authorization/tokens), trimmed; host and identifier replaced.
	private const string TokensJson = """
		{
			"links": { "create": "/services/authorization/tokens/_new" },
			"origin": "https://splunk.test:8089/services/authorization/tokens",
			"entry": [
				{
					"name": "00000000000000000000000000000000000000000000000000000000000000aa",
					"author": "system",
					"acl": { "app": "", "owner": "system", "sharing": "system", "perms": { "read": ["admin"], "write": ["admin"] } },
					"content": {
						"claims": {
							"aud": "splunk_api_it_probe",
							"exp": 1791471279,
							"iat": 1791467679,
							"idp": "Splunk",
							"iss": "admin from splunk01",
							"nbr": 1791467679,
							"roles": ["*"],
							"sub": "admin"
						},
						"eai:acl": null,
						"headers": { "alg": "HS512", "kid": "splunk.secret", "ttyp": "static", "ver": "v2" },
						"lastUsed": 0,
						"lastUsedIp": "",
						"status": "enabled"
					}
				}
			],
			"paging": { "total": 1, "perPage": 30, "offset": 0 },
			"messages": []
		}
		""";

	// Captured from Splunk Enterprise 10.6.0.5 (POST authorization/tokens), trimmed; identifier and token replaced.
	private const string CreatedJson = """
		{
			"entry": [
				{
					"name": "tokens",
					"content": { "eai:acl": null, "id": "00000000000000000000000000000000000000000000000000000000000000aa", "token": "eyJraWQiOiJmYWtlIn0.e30.fake" }
				}
			],
			"messages": []
		}
		""";

	// Captured from Splunk Enterprise 10.6.0.5 (POST authorization/tokens/admin with status=disabled).
	private const string UpdatedJson = """
		{ "entry": [], "paging": { "total": 0, "perPage": 30, "offset": 0 }, "messages": [ { "type": "INFO", "text": "Token(s) updated to status: disabled." } ] }
		""";

	[Fact]
	public async Task ListAsync_SendsTheFilters()
		=> (await RequestAssert.SendAsync((c, ct) => c.AuthenticationTokens.ListAsync(new AuthenticationTokenListOptions { Username = "admin", TokenId = "aa", Status = TokenStatus.Disabled }, ct)))
			.ShouldBe(HttpMethod.Get, "/services/authorization/tokens", query: "?username=admin&id=aa&status=disabled&output_mode=json");

	[Fact]
	public async Task CreateAsync_PostsTheToken()
		=> (await RequestAssert.SendAsync(
			(c, ct) => c.AuthenticationTokens.CreateAsync(new AuthenticationTokenCreateRequest { Name = "admin", Audience = "ci", ExpiresOn = "+90d", NotBefore = "+1m", Status = TokenStatus.Enabled }, ct),
			CreatedJson))
			.ShouldBe(HttpMethod.Post, "/services/authorization/tokens", "name=admin&audience=ci&expires_on=%2B90d&not_before=%2B1m&status=enabled");

	[Fact]
	public async Task UpdateStatusAsync_PostsTheStatus()
		=> (await RequestAssert.SendAsync((c, ct) => c.AuthenticationTokens.UpdateStatusAsync("admin", new AuthenticationTokenStatusRequest { Status = TokenStatus.Disabled, TokenId = "aa" }, ct), UpdatedJson))
			.ShouldBe(HttpMethod.Post, "/services/authorization/tokens/admin", "status=disabled&id=aa");

	[Fact]
	public async Task DeleteAsync_SendsTheTokenId()
		=> (await RequestAssert.SendAsync((c, ct) => c.AuthenticationTokens.DeleteAsync("admin", "aa", ct)))
			.ShouldBe(HttpMethod.Delete, "/services/authorization/tokens/admin", query: "?id=aa&output_mode=json");

	[Fact]
	public async Task DeleteAsync_WithoutATokenId_SendsNoId()
		=> (await RequestAssert.SendAsync((c, ct) => c.AuthenticationTokens.DeleteAsync("admin", null, ct)))
			.ShouldBe(HttpMethod.Delete, "/services/authorization/tokens/admin");

	[Fact]
	public async Task Content_MapsEveryModelledField()
	{
		var feed = await RequestAssert.ReadAsync((c, ct) => c.AuthenticationTokens.ListAsync(null, ct), TokensJson);

		var token = feed.Entries.Should().ContainSingle().Subject.Content!;
		token.Status.Should().Be(TokenStatus.Enabled);
		token.LastUsed.Should().BeNull();
		token.LastUsedIp.Should().BeEmpty();
		token.Claims!.Audience.Should().Be("splunk_api_it_probe");
		token.Claims.ExpiresAt.Should().Be(DateTimeOffset.FromUnixTimeSeconds(1791471279));
		token.Claims.IssuedAt.Should().Be(DateTimeOffset.FromUnixTimeSeconds(1791467679));
		token.Claims.NotBefore.Should().Be(DateTimeOffset.FromUnixTimeSeconds(1791467679));
		token.Claims.IdentityProvider.Should().Be("Splunk");
		token.Claims.Issuer.Should().Be("admin from splunk01");
		token.Claims.Roles.Should().Equal("*");
		token.Claims.Subject.Should().Be("admin");
		token.Headers!.Algorithm.Should().Be("HS512");
		token.Headers.KeyId.Should().Be("splunk.secret");
		token.Headers.TokenType.Should().Be("static");
		token.Headers.Version.Should().Be("v2");
	}

	[Fact]
	public async Task Created_MapsTheIdAndToken()
	{
		var feed = await RequestAssert.ReadAsync((c, ct) => c.AuthenticationTokens.CreateAsync(new AuthenticationTokenCreateRequest { Name = "admin", Audience = "ci" }, ct), CreatedJson);

		var created = feed.Entries.Should().ContainSingle().Subject.Content!;
		created.TokenId.Should().Be("00000000000000000000000000000000000000000000000000000000000000aa");
		created.Token.Should().Be("eyJraWQiOiJmYWtlIn0.e30.fake");
	}

	[Fact]
	public async Task UpdateStatus_ReturnsSplunksMessage()
	{
		var feed = await RequestAssert.ReadAsync((c, ct) => c.AuthenticationTokens.UpdateStatusAsync("admin", new AuthenticationTokenStatusRequest { Status = TokenStatus.Disabled }, ct), UpdatedJson);

		feed.Entries.Should().BeEmpty();
		feed.Messages.Should().ContainSingle().Which.Text.Should().Be("Token(s) updated to status: disabled.");
	}

	[Fact]
	public Task Error_RaisesSplunkApiException()
		=> RequestAssert.ShouldRaiseSplunkErrorAsync((c, ct) => c.AuthenticationTokens.UpdateStatusAsync("admin", new AuthenticationTokenStatusRequest { Status = TokenStatus.Enabled }, ct), HttpStatusCode.BadRequest);
}
