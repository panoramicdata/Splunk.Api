using Splunk.Api.Models.Access;
using System.Net;

namespace Splunk.Api.IntegrationTest.Access;

/// <summary>
/// The external authentication configurations (MFA, LDAP, SAML, ProxySSO, OAuth). Creating any of them would change how
/// everyone logs in to the shared instance, so these tests read and assert the errors a standalone instance returns;
/// unit tests pin the writes.
/// </summary>
[Collection(SplunkTestGroup.Name)]
public class AuthenticationProvidersIntegrationTests(SplunkFixture fixture)
{
	private const string Missing = "splunk_api_it_missing";

	[Fact]
	public async Task DuoMfa_ListIsEmptyWithAWarning_AndAnUnknownNameIsNotFound()
	{
		var ct = TestContext.Current.CancellationToken;

		var feed = await fixture.Client.DuoMfa.ListAsync(null, ct);

		feed.Entries.Should().BeEmpty();
		feed.Messages.Should().ContainSingle().Which.Text.Should().Be("No active Duo MFA configuration to list.");
		await ShouldBeNotFoundAsync(() => fixture.Client.DuoMfa.GetAsync(Missing, ct));
	}

	[Fact]
	public async Task RsaMfa_ListIsEmpty_AndVerifyReportsNoConfiguration()
	{
		var ct = TestContext.Current.CancellationToken;

		(await fixture.Client.RsaMfa.ListAsync(null, ct)).Entries.Should().BeEmpty();

		await SplunkAssert.FailsAsync(() => fixture.Client.RsaMfa.VerifyAsync(Missing, new RsaMfaVerifyRequest { Username = "splunk_api_it", Passcode = "000000" }, ct), HttpStatusCode.NotFound, "No Rsa MFA configuration was found.");
	}

	[Fact]
	public async Task Ldap_StrategiesAndGroupsAreEmpty()
	{
		var ct = TestContext.Current.CancellationToken;

		(await fixture.Client.LdapStrategies.ListAsync(null, ct)).Entries.Should().BeEmpty();
		var groups = await fixture.Client.LdapGroups.ListAsync(new LdapGroupListOptions { Strategy = Missing }, ct);

		groups.Entries.Should().BeEmpty();
		groups.Messages.Should().ContainSingle().Which.Text.Should().Be("Attempted to get LDAP groups when no LDAP strategies were enabled");
	}

	[Fact]
	public async Task Saml_ConfigurationsAreEmpty_AndAnUnknownNameIsNotFound()
	{
		var ct = TestContext.Current.CancellationToken;

		(await fixture.Client.SamlProviders.ListAsync(null, ct)).Entries.Should().BeEmpty();
		await ShouldBeNotFoundAsync(() => fixture.Client.SamlProviders.GetAsync(Missing, ct));
	}

	[Fact]
	public async Task ProxySso_ConfigurationsAreEmpty_AndAnUnknownNameIsNotFound()
	{
		var ct = TestContext.Current.CancellationToken;

		(await fixture.Client.ProxySsoConfigurations.ListAsync(null, ct)).Entries.Should().BeEmpty();
		await ShouldBeNotFoundAsync(() => fixture.Client.ProxySsoConfigurations.GetAsync(Missing, ct));
	}

	[Fact]
	public async Task OAuth2_ProvidersAndGroupsAreEmpty_AndAnUnknownNameIsNotFound()
	{
		var ct = TestContext.Current.CancellationToken;

		(await fixture.Client.OAuth2Providers.ListAsync(null, ct)).Entries.Should().BeEmpty();
		(await fixture.Client.OAuth2Groups.ListAsync(null, ct)).Entries.Should().BeEmpty();
		var expected = $"Config with name={Missing} was not found. Error=OAuth2 Config with friendlyName={Missing} was not found.";
		await ShouldBeNotFoundAsync(() => fixture.Client.OAuth2Providers.GetAsync(Missing, ct));
		var filter = () => fixture.Client.OAuth2Groups.ListAsync(new OAuth2GroupListOptions { Config = Missing }, ct);
		(await filter.Should().ThrowAsync<SplunkApiException>()).Which.Message.Should().Be(expected);
	}

	[Fact]
	public async Task OAuth2Token_AnInvalidAssertion_IsRejectedAtTheRootPath()
	{
		var act = () => fixture.Client.OAuth2Tokens.ExchangeAsync(new OAuth2TokenExchangeRequest { ClientId = "splunk_api_it_client", ClientAssertion = "not.a.jwt" }, TestContext.Current.CancellationToken);

		(await act.Should().ThrowAsync<SplunkApiException>()).Which.StatusCode.Should().Be(HttpStatusCode.BadRequest);
	}

	[Fact]
	public async Task FieldFilters_ListAndAnUnknownNameIsNotFound()
	{
		var ct = TestContext.Current.CancellationToken;

		var feed = await fixture.Client.FieldFilters.ListAsync(null, ct);

		feed.Entries.Should().NotContain(e => e.Name.StartsWith(SplunkFixture.Prefix, StringComparison.Ordinal));
		await ShouldBeNotFoundAsync(() => fixture.Client.FieldFilters.GetAsync(Missing, ct));
	}

	[Fact]
	public async Task MetricsProcessor_Reloads()
		=> await fixture.Client.MetricsProcessor.ReloadAsync(TestContext.Current.CancellationToken);

	private static async Task ShouldBeNotFoundAsync(Func<Task> act)
	{
		await SplunkAssert.FailsAsync(act, HttpStatusCode.NotFound, $"Could not find object id={Missing}");
	}
}
