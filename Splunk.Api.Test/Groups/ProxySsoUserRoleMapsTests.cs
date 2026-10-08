using Splunk.Api.Models.Access;
using Splunk.Api.Test.Support;
using System.Net;

namespace Splunk.Api.Test.Groups;

public class ProxySsoUserRoleMapsTests
{
	[Fact]
	public async Task ListAsync_SendsGet()
		=> (await RequestAssert.SendAsync((c, ct) => c.ProxySsoUserRoleMaps.ListAsync(null, ct)))
			.ShouldBe(HttpMethod.Get, "/services/admin/ProxySSO-user-role-map");

	[Fact]
	public async Task CreateAsync_PostsTheMapping()
		=> (await RequestAssert.SendAsync((c, ct) => c.ProxySsoUserRoleMaps.CreateAsync(new RoleMappingCreateRequest { Name = "jo", Roles = ["user"] }, ct)))
			.ShouldBe(HttpMethod.Post, "/services/admin/ProxySSO-user-role-map", "name=jo&roles=user");

	[Fact]
	public async Task GetAsync_SendsGetForTheName()
		=> (await RequestAssert.SendAsync((c, ct) => c.ProxySsoUserRoleMaps.GetAsync("jo", ct)))
			.ShouldBe(HttpMethod.Get, "/services/admin/ProxySSO-user-role-map/jo");

	[Fact]
	public async Task DeleteAsync_SendsDelete()
		=> (await RequestAssert.SendAsync((c, ct) => c.ProxySsoUserRoleMaps.DeleteAsync("jo", ct)))
			.ShouldBe(HttpMethod.Delete, "/services/admin/ProxySSO-user-role-map/jo");

	[Fact]
	public async Task Content_MapsEveryModelledField()
		=> (await RequestAssert.ReadAsync((c, ct) => c.ProxySsoUserRoleMaps.ListAsync(null, ct), RoleMappingJson.Feed)).ShouldBeTheCapturedMapping();

	[Fact]
	public Task Error_RaisesSplunkApiException()
		=> RequestAssert.ShouldRaiseSplunkErrorAsync((c, ct) => c.ProxySsoUserRoleMaps.DeleteAsync("jo", ct), HttpStatusCode.BadRequest);
}
