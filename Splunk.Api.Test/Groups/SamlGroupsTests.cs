using Splunk.Api.Models.Access;
using Splunk.Api.Test.Support;

namespace Splunk.Api.Test.Groups;

public class SamlGroupsTests
{
	[Fact]
	public async Task ListAsync_SendsGet()
		=> (await EndpointRequests.SendAsync((c, ct) => c.SamlGroups.ListAsync(null, ct)))
			.ShouldBeEndpointRequest(HttpMethod.Get, "/services/admin/SAML-groups");

	[Fact]
	public async Task CreateAsync_PostsTheMapping()
		=> (await EndpointRequests.SendAsync((c, ct) => c.SamlGroups.CreateAsync(new RoleMappingCreateRequest { Name = "Splunk Users", Roles = ["user"] }, ct)))
			.ShouldBeEndpointRequest(HttpMethod.Post, "/services/admin/SAML-groups", "name=Splunk+Users&roles=user");

	[Fact]
	public async Task DeleteAsync_SendsDeleteWithTheNameAsOneSegment()
		=> (await EndpointRequests.SendAsync((c, ct) => c.SamlGroups.DeleteAsync("Splunk Users", ct)))
			.ShouldBeEndpointRequest(HttpMethod.Delete, "/services/admin/SAML-groups/Splunk%20Users");

	[Fact]
	public async Task Content_MapsEveryModelledField()
		=> (await EndpointRequests.ReadAsync((c, ct) => c.SamlGroups.ListAsync(null, ct), RoleMappingJson.Feed)).ShouldBeTheCapturedMapping();

	[Fact]
	public Task Error_RaisesSplunkApiException()
		=> EndpointRequests.ShouldRaiseSplunkErrorAsync((c, ct) => c.SamlGroups.DeleteAsync("missing", ct));
}
