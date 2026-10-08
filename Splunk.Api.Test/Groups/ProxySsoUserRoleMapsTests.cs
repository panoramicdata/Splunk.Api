using Splunk.Api.Models.Access;
using Splunk.Api.Test.Support;
using System.Net;

namespace Splunk.Api.Test.Groups;

public class ProxySsoUserRoleMapsTests
{
	[Fact]
	public async Task ListAsync_SendsGet()
		=> (await EndpointRequests.SendAsync((c, ct) => c.ProxySsoUserRoleMaps.ListAsync(null, ct)))
			.ShouldBeEndpointRequest(HttpMethod.Get, "/services/admin/ProxySSO-user-role-map");

	[Fact]
	public async Task CreateAsync_PostsTheMapping()
		=> (await EndpointRequests.SendAsync((c, ct) => c.ProxySsoUserRoleMaps.CreateAsync(new RoleMappingCreateRequest { Name = "jo", Roles = ["user"] }, ct)))
			.ShouldBeEndpointRequest(HttpMethod.Post, "/services/admin/ProxySSO-user-role-map", "name=jo&roles=user");

	[Fact]
	public async Task GetAsync_SendsGetForTheName()
		=> (await EndpointRequests.SendAsync((c, ct) => c.ProxySsoUserRoleMaps.GetAsync("jo", ct)))
			.ShouldBeEndpointRequest(HttpMethod.Get, "/services/admin/ProxySSO-user-role-map/jo");

	[Fact]
	public async Task DeleteAsync_SendsDelete()
		=> (await EndpointRequests.SendAsync((c, ct) => c.ProxySsoUserRoleMaps.DeleteAsync("jo", ct)))
			.ShouldBeEndpointRequest(HttpMethod.Delete, "/services/admin/ProxySSO-user-role-map/jo");

	[Fact]
	public async Task Content_MapsEveryModelledField()
		=> (await EndpointRequests.ReadAsync((c, ct) => c.ProxySsoUserRoleMaps.ListAsync(null, ct), RoleMappingJson.Feed)).ShouldBeTheCapturedMapping();

	[Fact]
	public Task Error_RaisesSplunkApiException()
		=> EndpointRequests.ShouldRaiseSplunkErrorAsync((c, ct) => c.ProxySsoUserRoleMaps.DeleteAsync("jo", ct), HttpStatusCode.BadRequest);
}
