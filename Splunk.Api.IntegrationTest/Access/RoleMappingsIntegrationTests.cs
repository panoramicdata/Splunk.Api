using Splunk.Api.Models.Access;
using System.Net;

namespace Splunk.Api.IntegrationTest.Access;

/// <summary>
/// SAML and ProxySSO role mappings. Group mappings take effect only when SAML or ProxySSO is enabled, which it is not
/// on the shared instance, so prefixed mappings are created and deleted; user mappings need a configured identity
/// provider, so their errors are asserted.
/// </summary>
[Collection(SplunkTestGroup.Name)]
public class RoleMappingsIntegrationTests(SplunkFixture fixture)
{
	[Fact]
	public async Task SamlGroups_CreateListDelete_RoundTrips()
	{
		var ct = TestContext.Current.CancellationToken;
		var name = SplunkFixture.UniqueName("saml_group");
		await fixture.Client.SamlGroups.CreateAsync(new RoleMappingCreateRequest { Name = name, Roles = ["user"] }, ct);
		try
		{
			var listed = await fixture.Client.SamlGroups.ListAsync(new() { Count = 0 }, ct);

			listed.Entries.Should().ContainSingle(e => e.Name == name).Which.Content!.Roles.Should().Equal("user");
		}
		finally
		{
			await fixture.Client.SamlGroups.DeleteAsync(name, ct);
		}

		var act = () => fixture.Client.SamlGroups.DeleteAsync(name, ct);
		(await act.Should().ThrowAsync<SplunkApiException>()).Which.StatusCode.Should().Be(HttpStatusCode.NotFound);
	}

	[Fact]
	public async Task ProxySsoGroups_CreateGetUpdateDelete_RoundTrips()
	{
		var ct = TestContext.Current.CancellationToken;
		var name = SplunkFixture.UniqueName("proxy_group");
		await fixture.Client.ProxySsoGroups.CreateAsync(new RoleMappingCreateRequest { Name = name, Roles = ["user"] }, ct);
		try
		{
			(await fixture.Client.ProxySsoGroups.GetAsync(name, ct)).Entries.Should().ContainSingle().Which.Content!.Roles.Should().Equal("user");

			await fixture.Client.ProxySsoGroups.UpdateAsync(name, new RoleMappingUpdateRequest { Roles = ["user", "power"] }, ct);

			var listed = await fixture.Client.ProxySsoGroups.ListAsync(new() { Count = 0 }, ct);
			listed.Entries.Should().ContainSingle(e => e.Name == name).Which.Content!.Roles.Order().Should().Equal("power", "user");
		}
		finally
		{
			await fixture.Client.ProxySsoGroups.DeleteAsync(name, ct);
		}

		var act = () => fixture.Client.ProxySsoGroups.GetAsync(name, ct);
		(await act.Should().ThrowAsync<SplunkApiException>()).Which.Message.Should().Be($"Unable to find a role mapping for {name}");
	}

	[Fact]
	public async Task ProxySsoUserRoleMaps_NeedAProxySsoManager()
	{
		var ct = TestContext.Current.CancellationToken;
		var name = SplunkFixture.UniqueName("proxy_user");

		(await fixture.Client.ProxySsoUserRoleMaps.ListAsync(null, ct)).Entries.Should().BeEmpty();
		await ShouldFailAsync(() => fixture.Client.ProxySsoUserRoleMaps.CreateAsync(new RoleMappingCreateRequest { Name = name, Roles = ["user"] }, ct), HttpStatusCode.BadRequest, "Proxy SSO Manager not configured.");
		await ShouldFailAsync(() => fixture.Client.ProxySsoUserRoleMaps.GetAsync(name, ct), HttpStatusCode.BadRequest, $"Unable to find a role mapping for user={name}");
		await ShouldFailAsync(() => fixture.Client.ProxySsoUserRoleMaps.DeleteAsync(name, ct), HttpStatusCode.BadRequest, "Proxy SSO Manager not configured.");
	}

	[Fact]
	public async Task SamlUserRoleMaps_NeedAKnownUser_AndTheCollectionCannotBeDeleted()
	{
		var ct = TestContext.Current.CancellationToken;
		var name = SplunkFixture.UniqueName("saml_user");

		(await fixture.Client.SamlUserRoleMaps.ListAsync(null, ct)).Entries.Should().BeEmpty();
		await ShouldFailAsync(() => fixture.Client.SamlUserRoleMaps.CreateAsync(new RoleMappingCreateRequest { Name = name, Roles = ["user"] }, ct), HttpStatusCode.BadRequest, $"Failed to get cachedUserInfo for user={name}");
		await ShouldFailAsync(() => fixture.Client.SamlUserRoleMaps.DeleteAsync(name, ct), HttpStatusCode.NotFound, $"Delete failed, User={name} does not exist in SAML userToRoleMap.");
		await ShouldFailAsync(() => fixture.Client.SamlUserRoleMaps.DeleteAllAsync(ct), HttpStatusCode.BadRequest, "Cannot perform action \"DELETE\" without a target name to act on.");
	}

	[Fact]
	public async Task SamlMetadata_ServiceProviderMetadataIsReturned_AndABadIdpFileIsRejected()
	{
		var ct = TestContext.Current.CancellationToken;

		var sp = await fixture.Client.SamlMetadata.GetServiceProviderMetadataAsync(ct);
		var idp = await fixture.Client.SamlMetadata.GetIdentityProviderMetadataAsync(null, ct);

		sp.Entries.Should().ContainSingle().Which.Content!.SpMetadata.Should().Contain("EntityDescriptor");
		idp.Entries.Should().BeEmpty();
		var act = () => fixture.Client.SamlMetadata.GetIdentityProviderMetadataAsync(new SamlIdpMetadataOptions { IdpMetadataFile = "/splunk_api_it_missing.xml" }, ct);
		(await act.Should().ThrowAsync<SplunkApiException>()).Which.StatusCode.Should().Be(HttpStatusCode.BadRequest);
	}

	[Fact]
	public async Task SamlMetadata_ReplicatingCertificates_NeedsASearchHeadCluster()
	{
		var act = () => fixture.Client.SamlMetadata.ReplicateCertificatesAsync(TestContext.Current.CancellationToken);

		(await act.Should().ThrowAsync<SplunkApiException>()).Which.StatusCode.Should().NotBe(HttpStatusCode.OK);
	}

	private static async Task ShouldFailAsync(Func<Task> act, HttpStatusCode status, string message)
	{
		var thrown = await act.Should().ThrowAsync<SplunkApiException>();
		thrown.Which.StatusCode.Should().Be(status);
		thrown.Which.Message.Should().Be(message);
	}
}
