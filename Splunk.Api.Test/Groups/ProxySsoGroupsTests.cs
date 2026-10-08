using Splunk.Api.Models.Access;
using Splunk.Api.Test.Support;

namespace Splunk.Api.Test.Groups;

public class ProxySsoGroupsTests
{
	[Fact]
	public async Task ListAsync_SendsGet()
		=> (await EndpointRequests.SendAsync((c, ct) => c.ProxySsoGroups.ListAsync(null, ct)))
			.ShouldBeEndpointRequest(HttpMethod.Get, "/services/admin/ProxySSO-groups");

	[Fact]
	public async Task CreateAsync_PostsTheMapping()
		=> (await EndpointRequests.SendAsync((c, ct) => c.ProxySsoGroups.CreateAsync(new RoleMappingCreateRequest { Name = "ops", Roles = ["user", "power"] }, ct)))
			.ShouldBeEndpointRequest(HttpMethod.Post, "/services/admin/ProxySSO-groups", "name=ops&roles=user&roles=power");

	[Fact]
	public async Task GetAsync_SendsGetForTheName()
		=> (await EndpointRequests.SendAsync((c, ct) => c.ProxySsoGroups.GetAsync("ops", ct)))
			.ShouldBeEndpointRequest(HttpMethod.Get, "/services/admin/ProxySSO-groups/ops");

	[Fact]
	public async Task UpdateAsync_PostsTheRoles()
		=> (await EndpointRequests.SendAsync((c, ct) => c.ProxySsoGroups.UpdateAsync("ops", new RoleMappingUpdateRequest { Roles = ["admin"] }, ct)))
			.ShouldBeEndpointRequest(HttpMethod.Post, "/services/admin/ProxySSO-groups/ops", "roles=admin");

	[Fact]
	public async Task DeleteAsync_SendsDelete()
		=> (await EndpointRequests.SendAsync((c, ct) => c.ProxySsoGroups.DeleteAsync("ops", ct)))
			.ShouldBeEndpointRequest(HttpMethod.Delete, "/services/admin/ProxySSO-groups/ops");

	[Fact]
	public async Task Content_MapsEveryModelledField()
		=> (await EndpointRequests.ReadAsync((c, ct) => c.ProxySsoGroups.GetAsync("splunk_api_it_group", ct), RoleMappingJson.Feed)).ShouldBeTheCapturedMapping();

	[Fact]
	public Task Error_RaisesSplunkApiException()
		=> EndpointRequests.ShouldRaiseSplunkErrorAsync((c, ct) => c.ProxySsoGroups.GetAsync("missing", ct));
}
