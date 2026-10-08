using Splunk.Api.Test.Support.Platform;
using System.Net;

namespace Splunk.Api.Test.Groups;

public class ServerRolesTests
{
	[Fact]
	public async Task GetAsync_SendsGet()
		=> await Calls.AssertAsync(c => c.ServerRoles.GetAsync(Calls.Token), HttpMethod.Get, "/services/server/roles", Calls.JsonQuery, null);

	[Fact]
	public async Task GetAsync_MapsTheRoles()
	{
		// Captured from Splunk 10.6.0.5.
		var feed = await Calls.MapAsync(
			c => c.ServerRoles.GetAsync(Calls.Token),
			Feed.Of("result", """{"eai:acl":null,"role_list":["indexer","license_master","license_manager","kv_store"]}"""));

		feed.Entries.Should().ContainSingle().Which.Content!.Roles.Should().Equal("indexer", "license_master", "license_manager", "kv_store");
	}

	[Fact]
	public async Task GetAsync_Error_RaisesSplunkApiException()
		=> await Calls.AssertErrorAsync(c => c.ServerRoles.GetAsync(Calls.Token), HttpStatusCode.Unauthorized);
}
