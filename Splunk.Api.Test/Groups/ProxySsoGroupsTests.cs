using Splunk.Api.Models.Access;
using Splunk.Api.Test.Support;

namespace Splunk.Api.Test.Groups;

public class ProxySsoGroupsTests
{
	[Fact]
	public async Task ListAsync_SendsGet()
		=> (await RequestAssert.SendAsync((c, ct) => c.ProxySsoGroups.ListAsync(null, ct)))
			.ShouldBe(HttpMethod.Get, "/services/admin/ProxySSO-groups");

	[Fact]
	public async Task CreateAsync_PostsTheMapping()
		=> (await RequestAssert.SendAsync((c, ct) => c.ProxySsoGroups.CreateAsync(new RoleMappingCreateRequest { Name = "ops", Roles = ["user", "power"] }, ct)))
			.ShouldBe(HttpMethod.Post, "/services/admin/ProxySSO-groups", "name=ops&roles=user&roles=power");

	[Fact]
	public async Task GetAsync_SendsGetForTheName()
		=> (await RequestAssert.SendAsync((c, ct) => c.ProxySsoGroups.GetAsync("ops", ct)))
			.ShouldBe(HttpMethod.Get, "/services/admin/ProxySSO-groups/ops");

	[Fact]
	public async Task UpdateAsync_PostsTheRoles()
		=> (await RequestAssert.SendAsync((c, ct) => c.ProxySsoGroups.UpdateAsync("ops", new RoleMappingUpdateRequest { Roles = ["admin"] }, ct)))
			.ShouldBe(HttpMethod.Post, "/services/admin/ProxySSO-groups/ops", "roles=admin");

	[Fact]
	public async Task DeleteAsync_SendsDelete()
		=> (await RequestAssert.SendAsync((c, ct) => c.ProxySsoGroups.DeleteAsync("ops", ct)))
			.ShouldBe(HttpMethod.Delete, "/services/admin/ProxySSO-groups/ops");

	[Fact]
	public async Task Content_MapsEveryModelledField()
		=> (await RequestAssert.ReadAsync((c, ct) => c.ProxySsoGroups.GetAsync("splunk_api_it_group", ct), RoleMappingJson.Feed)).ShouldBeTheCapturedMapping();

	[Fact]
	public Task Error_RaisesSplunkApiException()
		=> RequestAssert.ShouldRaiseSplunkErrorAsync((c, ct) => c.ProxySsoGroups.GetAsync("missing", ct));
}
