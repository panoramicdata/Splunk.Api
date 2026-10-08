using Splunk.Api.Test.Support;

namespace Splunk.Api.Test.Groups;

public class CapabilitiesTests
{
	// Captured from Splunk Enterprise 10.6.0.5 (GET authorization/capabilities), trimmed; host replaced.
	private const string CapabilitiesJson = """
		{
			"links": {},
			"origin": "https://splunk.test:8089/services/authorization/capabilities",
			"entry": [
				{
					"name": "capabilities",
					"author": "system",
					"acl": { "app": "", "owner": "system", "sharing": "system", "perms": { "read": ["*"], "write": ["*"] } },
					"content": { "capabilities": ["accelerate_datamodel", "accelerate_search", "admin_all_objects"], "eai:acl": null }
				}
			],
			"paging": { "total": 1, "perPage": 30, "offset": 0 },
			"messages": []
		}
		""";

	[Fact]
	public async Task ListAsync_SendsGet()
		=> (await RequestAssert.SendAsync((c, ct) => c.Capabilities.ListAsync(ct)))
			.ShouldBe(HttpMethod.Get, "/services/authorization/capabilities");

	[Fact]
	public async Task ListGrantableAsync_SendsGet()
		=> (await RequestAssert.SendAsync((c, ct) => c.Capabilities.ListGrantableAsync(ct)))
			.ShouldBe(HttpMethod.Get, "/services/authorization/grantable_capabilities");

	[Fact]
	public async Task Content_MapsEveryModelledField()
	{
		var feed = await RequestAssert.ReadAsync((c, ct) => c.Capabilities.ListGrantableAsync(ct), CapabilitiesJson);

		var entry = feed.Entries.Should().ContainSingle().Subject;
		entry.Name.Should().Be("capabilities");
		entry.Content!.Capabilities.Should().Equal("accelerate_datamodel", "accelerate_search", "admin_all_objects");
	}

	[Fact]
	public Task Error_RaisesSplunkApiException()
		=> RequestAssert.ShouldRaiseSplunkErrorAsync((c, ct) => c.Capabilities.ListAsync(ct), System.Net.HttpStatusCode.Forbidden);
}
