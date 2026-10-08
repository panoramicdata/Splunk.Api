using Splunk.Api.Test.Support;

namespace Splunk.Api.Test.Groups;

public class LicensePeersTests
{
	// Captured from Splunk Enterprise 10.6.0.5 (GET licenser/peers), trimmed; host and GUID replaced.
	private const string PeersJson = """
		{
			"links": {},
			"origin": "https://splunk.test:8089/services/licenser/peers",
			"entry": [
				{
					"name": "00000000-0000-0000-0000-000000000001",
					"author": "nobody",
					"content": {
						"active_pool_ids": ["auto_generated_pool_download-trial"],
						"eai:acl": null,
						"label": "splunk01",
						"pool_ids": ["auto_generated_pool_download-trial", "auto_generated_pool_forwarder", "auto_generated_pool_free"],
						"pool_suggestion": null,
						"stack_ids": ["download-trial", "forwarder", "free"],
						"uri": "https://127.0.0.1:8089",
						"warning_count": 0
					}
				}
			],
			"paging": { "total": 1, "perPage": 30, "offset": 0 },
			"messages": []
		}
		""";

	[Fact]
	public async Task ListAsync_SendsGet()
		=> (await EndpointRequests.SendAsync((c, ct) => c.LicensePeers.ListAsync(null, ct)))
			.ShouldBeEndpointRequest(HttpMethod.Get, "/services/licenser/peers");

	[Fact]
	public async Task GetAsync_SendsGetForTheGuid()
		=> (await EndpointRequests.SendAsync((c, ct) => c.LicensePeers.GetAsync("00000000-0000-0000-0000-000000000001", ct)))
			.ShouldBeEndpointRequest(HttpMethod.Get, "/services/licenser/peers/00000000-0000-0000-0000-000000000001");

	[Fact]
	public async Task Content_MapsEveryModelledField()
	{
		var feed = await EndpointRequests.ReadAsync((c, ct) => c.LicensePeers.ListAsync(null, ct), PeersJson);

		var peer = feed.Entries.Should().ContainSingle().Subject.Content!;
		peer.ActivePoolIds.Should().Equal("auto_generated_pool_download-trial");
		peer.Label.Should().Be("splunk01");
		peer.PoolIds.Should().HaveCount(3);
		peer.PoolSuggestion.Should().BeNull();
		peer.StackIds.Should().Equal("download-trial", "forwarder", "free");
		peer.Uri.Should().Be("https://127.0.0.1:8089");
		peer.WarningCount.Should().Be(0);
	}

	[Fact]
	public Task Error_RaisesSplunkApiException()
		=> EndpointRequests.ShouldRaiseSplunkErrorAsync((c, ct) => c.LicensePeers.GetAsync("missing", ct));
}
