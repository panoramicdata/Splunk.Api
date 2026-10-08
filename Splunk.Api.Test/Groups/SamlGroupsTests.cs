using Splunk.Api.Models.Access;
using Splunk.Api.Test.Support;

namespace Splunk.Api.Test.Groups;

public class SamlGroupsTests
{
	[Fact]
	public async Task ListAsync_SendsGet()
		=> (await RequestAssert.SendAsync((c, ct) => c.SamlGroups.ListAsync(null, ct)))
			.ShouldBe(HttpMethod.Get, "/services/admin/SAML-groups");

	[Fact]
	public async Task CreateAsync_PostsTheMapping()
		=> (await RequestAssert.SendAsync((c, ct) => c.SamlGroups.CreateAsync(new RoleMappingCreateRequest { Name = "Splunk Users", Roles = ["user"] }, ct)))
			.ShouldBe(HttpMethod.Post, "/services/admin/SAML-groups", "name=Splunk+Users&roles=user");

	[Fact]
	public async Task DeleteAsync_SendsDeleteWithTheNameAsOneSegment()
		=> (await RequestAssert.SendAsync((c, ct) => c.SamlGroups.DeleteAsync("Splunk Users", ct)))
			.ShouldBe(HttpMethod.Delete, "/services/admin/SAML-groups/Splunk%20Users");

	[Fact]
	public async Task Content_MapsEveryModelledField()
		=> (await RequestAssert.ReadAsync((c, ct) => c.SamlGroups.ListAsync(null, ct), RoleMappingJson.Feed)).ShouldBeTheCapturedMapping();

	[Fact]
	public Task Error_RaisesSplunkApiException()
		=> RequestAssert.ShouldRaiseSplunkErrorAsync((c, ct) => c.SamlGroups.DeleteAsync("missing", ct));
}
