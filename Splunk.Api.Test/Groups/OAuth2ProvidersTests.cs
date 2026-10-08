using Splunk.Api.Models.Access;
using Splunk.Api.Test.Support;

namespace Splunk.Api.Test.Groups;

public class OAuth2ProvidersTests
{
	// Shaped after the reference (the shared test instance has no OAuth provider, and creating one changes authentication).
	private const string ProviderJson = """
		{
			"entry": [
				{
					"name": "okta",
					"content": {
						"jwks_uri": "https://idp.example.com/oauth2/v1/keys",
						"audience": "api://splunk",
						"groupsClaim": "groups",
						"issuer": "https://idp.example.com",
						"clientIdClaim": "cid",
						"clientFullNameClaim": "name",
						"disabled": "0"
					}
				}
			]
		}
		""";

	[Fact]
	public async Task ListAsync_SendsGet()
		=> (await RequestAssert.SendAsync((c, ct) => c.OAuth2Providers.ListAsync(null, ct)))
			.ShouldBe(HttpMethod.Get, "/services/authentication/providers/oauth2");

	[Fact]
	public async Task CreateAsync_PostsTheConfiguration()
		=> (await RequestAssert.SendAsync((c, ct) => c.OAuth2Providers.CreateAsync(
			new OAuth2ProviderCreateRequest
			{
				Name = "okta",
				JwksUri = "https://idp/keys",
				Audience = "aud",
				GroupsClaim = "groups",
				Issuer = "https://idp",
				ClientIdClaim = "cid",
				ClientFullNameClaim = "name"
			},
			ct)))
			.ShouldBe(
				HttpMethod.Post,
				"/services/authentication/providers/oauth2",
				"name=okta&jwks_uri=https%3A%2F%2Fidp%2Fkeys&audience=aud&groupsClaim=groups&issuer=https%3A%2F%2Fidp&clientIdClaim=cid&clientFullNameClaim=name");

	[Fact]
	public async Task GetAsync_SendsGetForTheName()
		=> (await RequestAssert.SendAsync((c, ct) => c.OAuth2Providers.GetAsync("okta", ct)))
			.ShouldBe(HttpMethod.Get, "/services/authentication/providers/oauth2/okta");

	[Fact]
	public async Task UpdateAsync_PostsTheChanges()
		=> (await RequestAssert.SendAsync((c, ct) => c.OAuth2Providers.UpdateAsync(
			"okta",
			new OAuth2ProviderUpdateRequest { JwksUri = "k", Audience = "a", GroupsClaim = "g", ClientIdClaim = "c", ClientFullNameClaim = "n", Disabled = true },
			ct)))
			.ShouldBe(HttpMethod.Post, "/services/authentication/providers/oauth2/okta", "jwks_uri=k&audience=a&groupsClaim=g&clientIdClaim=c&clientFullNameClaim=n&disabled=true");

	[Fact]
	public async Task DeleteAsync_SendsDelete()
		=> (await RequestAssert.SendAsync((c, ct) => c.OAuth2Providers.DeleteAsync("okta", ct)))
			.ShouldBe(HttpMethod.Delete, "/services/authentication/providers/oauth2/okta");

	[Fact]
	public async Task Content_MapsEveryModelledField()
	{
		var feed = await RequestAssert.ReadAsync((c, ct) => c.OAuth2Providers.GetAsync("okta", ct), ProviderJson);

		var provider = feed.Entries.Should().ContainSingle().Subject.Content!;
		provider.JwksUri.Should().Be("https://idp.example.com/oauth2/v1/keys");
		provider.Audience.Should().Be("api://splunk");
		provider.GroupsClaim.Should().Be("groups");
		provider.Issuer.Should().Be("https://idp.example.com");
		provider.ClientIdClaim.Should().Be("cid");
		provider.ClientFullNameClaim.Should().Be("name");
		provider.Disabled.Should().BeFalse();
	}

	[Fact]
	public Task Error_RaisesSplunkApiException()
		=> RequestAssert.ShouldRaiseSplunkErrorAsync((c, ct) => c.OAuth2Providers.GetAsync("missing", ct));
}
