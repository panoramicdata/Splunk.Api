using Splunk.Api.Models.Access;
using Splunk.Api.Test.Support;
using System.Net;

namespace Splunk.Api.Test.Groups;

public class OAuth2TokensTests
{
	// The standard OAuth 2.0 token response (the shared test instance has no identity provider to issue a real assertion).
	private const string TokenJson = """{"access_token":"fake-access-token","token_type":"Bearer","expires_in":3600,"scope":"api","issued_token_type":"urn:ietf:params:oauth:token-type:access_token"}""";

	[Fact]
	public async Task ExchangeAsync_PostsTheAssertionToTheRootPath()
		=> (await RequestAssert.SendAsync((c, ct) => c.OAuth2Tokens.ExchangeAsync(new OAuth2TokenExchangeRequest { ClientId = "app", ClientAssertion = "a.b.c" }, ct), TokenJson))
			.ShouldBe(
				HttpMethod.Post,
				"/oauth2/v1/token",
				"grant_type=client_credentials&client_id=app&client_assertion_type=urn%3Aietf%3Aparams%3Aoauth%3Aclient-assertion-type%3Ajwt-bearer&client_assertion=a.b.c");

	[Fact]
	public async Task ExchangeAsync_InANamespace_IsNotRewritten()
	{
		var stub = TestClient.Stub(TokenJson);
		using var client = TestClient.Create(stub);

		await client.InNamespace("nobody", "search").OAuth2Tokens.ExchangeAsync(
			new OAuth2TokenExchangeRequest { ClientId = "app", ClientAssertion = "a.b.c", GrantType = "client_credentials", ClientAssertionType = "custom" },
			TestContext.Current.CancellationToken);

		stub.Calls.Should().ContainSingle().Which.Uri.AbsolutePath.Should().Be("/oauth2/v1/token");
	}

	[Fact]
	public async Task Response_MapsEveryModelledField()
	{
		var token = await RequestAssert.ReadAsync((c, ct) => c.OAuth2Tokens.ExchangeAsync(new OAuth2TokenExchangeRequest { ClientId = "app", ClientAssertion = "a.b.c" }, ct), TokenJson);

		token.AccessToken.Should().Be("fake-access-token");
		token.TokenType.Should().Be("Bearer");
		token.ExpiresIn.Should().Be(3600);
		token.Scope.Should().Be("api");
		token.AdditionalProperties["issued_token_type"].GetString().Should().Be("urn:ietf:params:oauth:token-type:access_token");
	}

	[Fact]
	public Task Error_RaisesSplunkApiException()
		=> RequestAssert.ShouldRaiseSplunkErrorAsync((c, ct) => c.OAuth2Tokens.ExchangeAsync(new OAuth2TokenExchangeRequest { ClientId = "app", ClientAssertion = "x" }, ct), HttpStatusCode.BadRequest);
}
