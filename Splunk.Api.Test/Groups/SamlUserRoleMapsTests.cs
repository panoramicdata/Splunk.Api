using Splunk.Api.Models.Access;
using Splunk.Api.Test.Support;
using System.Net;

namespace Splunk.Api.Test.Groups;

public class SamlUserRoleMapsTests
{
	[Fact]
	public async Task ListAsync_SendsGet()
		=> (await EndpointRequests.SendAsync((c, ct) => c.SamlUserRoleMaps.ListAsync(null, ct)))
			.ShouldBeEndpointRequest(HttpMethod.Get, "/services/admin/SAML-user-role-map");

	[Fact]
	public async Task CreateAsync_PostsTheUser()
		=> (await EndpointRequests.SendAsync((c, ct) => c.SamlUserRoleMaps.CreateAsync(new RoleMappingCreateRequest { Name = "jo@example.com", Roles = ["user", "power"] }, ct)))
			.ShouldBeEndpointRequest(HttpMethod.Post, "/services/admin/SAML-user-role-map", "name=jo%40example.com&roles=user&roles=power");

	[Fact]
	public async Task DeleteAllAsync_SendsDeleteToTheCollection()
		=> (await EndpointRequests.SendAsync((c, ct) => c.SamlUserRoleMaps.DeleteAllAsync(ct)))
			.ShouldBeEndpointRequest(HttpMethod.Delete, "/services/admin/SAML-user-role-map");

	[Fact]
	public async Task DeleteAsync_SendsDeleteForTheName()
		=> (await EndpointRequests.SendAsync((c, ct) => c.SamlUserRoleMaps.DeleteAsync("jo@example.com", ct)))
			.ShouldBeEndpointRequest(HttpMethod.Delete, "/services/admin/SAML-user-role-map/jo%40example.com");

	[Fact]
	public async Task Content_MapsEveryModelledField()
		=> (await EndpointRequests.ReadAsync((c, ct) => c.SamlUserRoleMaps.ListAsync(null, ct), RoleMappingJson.Feed)).ShouldBeTheCapturedMapping();

	[Fact]
	public Task Error_RaisesSplunkApiException()
		=> EndpointRequests.ShouldRaiseSplunkErrorAsync((c, ct) => c.SamlUserRoleMaps.DeleteAllAsync(ct), HttpStatusCode.BadRequest);
}
