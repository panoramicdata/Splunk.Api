using Splunk.Api.Models.Access;
using System.Net;

namespace Splunk.Api.IntegrationTest.Access;

[Collection(SplunkTestGroup.Name)]
public class AuthenticationTokensIntegrationTests(SplunkFixture fixture)
{
	[Fact]
	public async Task CreateUseDisableDelete_RoundTrips()
	{
		var ct = TestContext.Current.CancellationToken;
		var userName = fixture.CreateOptions().Username!;
		var audience = SplunkFixture.UniqueName("token");

		var created = await fixture.Client.AuthenticationTokens.CreateAsync(new AuthenticationTokenCreateRequest { Name = userName, Audience = audience, ExpiresOn = "+1h" }, ct);
		var token = created.Entries.Should().ContainSingle().Subject.Content!;
		token.TokenId.Should().NotBeNullOrWhiteSpace();
		token.Token.Should().NotBeNullOrWhiteSpace();
		try
		{
			using (var bearer = new SplunkClient(fixture.CreateOptions(o =>
			{
				o.Username = null;
				o.Password = null;
				o.Token = token.Token;
			})))
			{
				(await bearer.ServerInfo.GetAsync(ct)).Entries.Should().ContainSingle();
			}

			var listed = await fixture.Client.AuthenticationTokens.ListAsync(new AuthenticationTokenListOptions { TokenId = token.TokenId }, ct);
			var entry = listed.Entries.Should().ContainSingle().Subject;
			entry.Name.Should().Be(token.TokenId);
			entry.Content!.Status.Should().Be(TokenStatus.Enabled);
			entry.Content.Claims!.Audience.Should().Be(audience);
			entry.Content.Claims.Subject.Should().Be(userName);
			entry.Content.Claims.ExpiresAt.Should().BeAfter(DateTimeOffset.UtcNow);
			entry.Content.Headers!.Algorithm.Should().NotBeNullOrWhiteSpace();

			var disabled = await fixture.Client.AuthenticationTokens.UpdateStatusAsync(userName, new AuthenticationTokenStatusRequest { TokenId = token.TokenId, Status = TokenStatus.Disabled }, ct);
			disabled.Messages.Should().ContainSingle().Which.Text.Should().Contain("disabled");

			var filtered = await fixture.Client.AuthenticationTokens.ListAsync(new AuthenticationTokenListOptions { TokenId = token.TokenId, Status = TokenStatus.Disabled }, ct);
			filtered.Entries.Should().ContainSingle().Which.Content!.Status.Should().Be(TokenStatus.Disabled);
		}
		finally
		{
			await fixture.Client.AuthenticationTokens.DeleteAsync(userName, token.TokenId, ct);
		}

		var after = await fixture.Client.AuthenticationTokens.ListAsync(new AuthenticationTokenListOptions { TokenId = token.TokenId }, ct);
		after.Entries.Should().BeEmpty();
	}

	[Fact]
	public async Task UpdateStatusAsync_WithoutStatus_IsRejected()
	{
		// The reference documents audience/expires_on/not_before for this POST; Splunk 10.6 refuses them.
		var request = new AuthenticationTokenStatusRequest { Status = TokenStatus.Enabled, TokenId = "splunk_api_it_none" };
		request.AdditionalParameters["audience"] = "x";

		await SplunkAssert.FailsAsync(() => fixture.Client.AuthenticationTokens.UpdateStatusAsync(fixture.CreateOptions().Username!, request, TestContext.Current.CancellationToken), HttpStatusCode.BadRequest, "Argument \"audience\" is not supported by this handler.");
	}
}
