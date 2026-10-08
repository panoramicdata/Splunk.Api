using Splunk.Api.Test.Support;

namespace Splunk.Api.Test.Groups;

public class LicenseUsageTests
{
	// Captured from Splunk Enterprise 10.6.0.5 (GET licenser/usage), trimmed; host replaced and usage made non-zero.
	private const string UsageJson = """
		{
			"links": {},
			"origin": "https://splunk.test:8089/services/licenser/usage",
			"entry": [
				{ "name": "license_usage", "author": "system", "content": { "eai:acl": null, "peers_usage_bytes": 1048576, "quota": 524288000, "slaves_usage_bytes": 1048576 } }
			],
			"paging": { "total": 1, "perPage": 30, "offset": 0 },
			"messages": []
		}
		""";

	[Fact]
	public async Task GetAsync_SendsGet()
		=> (await EndpointRequests.SendAsync((c, ct) => c.LicenseUsage.GetAsync(null, ct)))
			.ShouldBeEndpointRequest(HttpMethod.Get, "/services/licenser/usage");

	[Fact]
	public async Task Content_MapsEveryModelledField()
	{
		var feed = await EndpointRequests.ReadAsync((c, ct) => c.LicenseUsage.GetAsync(null, ct), UsageJson);

		var usage = feed.Entries.Should().ContainSingle().Subject.Content!;
		usage.PeersUsageBytes.Should().Be(1048576);
		usage.Quota.Should().Be(524288000);
		usage.AdditionalProperties.Should().ContainKey("slaves_usage_bytes");
	}

	[Fact]
	public Task Error_RaisesSplunkApiException()
		=> EndpointRequests.ShouldRaiseSplunkErrorAsync((c, ct) => c.LicenseUsage.GetAsync(null, ct), System.Net.HttpStatusCode.Forbidden);
}
