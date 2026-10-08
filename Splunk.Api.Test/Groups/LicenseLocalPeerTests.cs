using Splunk.Api.Test.Support;

namespace Splunk.Api.Test.Groups;

public class LicenseLocalPeerTests
{
	// Captured from Splunk Enterprise 10.6.0.5 (GET licenser/localpeer), trimmed; host and GUIDs replaced, features shortened.
	private const string LocalPeerJson = """
		{
			"links": {},
			"origin": "https://splunk.test:8089/services/licenser/localpeer",
			"entry": [
				{
					"name": "license",
					"author": "system",
					"content": {
						"add_ons": { "hadoop": { "parameters": { "erp_type": "report", "maxNodes": "200" }, "type": "external_results_provider" } },
						"connection_timeout": 30,
						"eai:acl": null,
						"features": { "Acceleration": "ENABLED", "AWSMarketplace": "DISABLED_DUE_TO_LICENSE" },
						"guid": ["6F416E61-B40E-461C-A782-CBC186E98133"],
						"last_manager_contact_attempt_time": 1791467357,
						"last_manager_contact_success_time": 1791467358,
						"last_master_contact_attempt_time": 1791467357,
						"last_trackerdb_service_time": 0,
						"license_keys": ["5C52DA5145AD67B8188604C49962D12F2C3B2CF1B82A6878E46F68CA2812807B"],
						"manager_guid": "00000000-0000-0000-0000-000000000001",
						"manager_uri": "self",
						"master_uri": "self",
						"peer_id": "00000000-0000-0000-0000-000000000001",
						"peer_label": "splunk01",
						"receive_timeout": 30,
						"send_timeout": 31,
						"slave_label": "splunk01",
						"squash_threshold": 2000
					}
				}
			],
			"paging": { "total": 1, "perPage": 30, "offset": 0 },
			"messages": []
		}
		""";

	[Fact]
	public async Task GetAsync_SendsGet()
		=> (await EndpointRequests.SendAsync((c, ct) => c.LicenseLocalPeer.GetAsync(ct)))
			.ShouldBeEndpointRequest(HttpMethod.Get, "/services/licenser/localpeer");

	[Fact]
	public async Task Content_MapsEveryModelledField()
	{
		var feed = await EndpointRequests.ReadAsync((c, ct) => c.LicenseLocalPeer.GetAsync(ct), LocalPeerJson);

		var entry = feed.Entries.Should().ContainSingle().Subject;
		entry.Name.Should().Be("license");
		var peer = entry.Content!;
		peer.AddOns!["hadoop"].GetProperty("type").GetString().Should().Be("external_results_provider");
		peer.ConnectionTimeout.Should().Be(30);
		peer.Features.Should().Contain("Acceleration", "ENABLED").And.Contain("AWSMarketplace", "DISABLED_DUE_TO_LICENSE");
		peer.LicenseGuids.Should().Equal("6F416E61-B40E-461C-A782-CBC186E98133");
		peer.LastManagerContactAttemptTime.Should().Be(DateTimeOffset.FromUnixTimeSeconds(1791467357));
		peer.LastManagerContactSuccessTime.Should().Be(DateTimeOffset.FromUnixTimeSeconds(1791467358));
		peer.LastTrackerDbServiceTime.Should().BeNull();
		peer.LicenseKeys.Should().Equal("5C52DA5145AD67B8188604C49962D12F2C3B2CF1B82A6878E46F68CA2812807B");
		peer.ManagerGuid.Should().Be("00000000-0000-0000-0000-000000000001");
		peer.ManagerUri.Should().Be("self");
		peer.PeerId.Should().Be("00000000-0000-0000-0000-000000000001");
		peer.PeerLabel.Should().Be("splunk01");
		peer.ReceiveTimeout.Should().Be(30);
		peer.SendTimeout.Should().Be(31);
		peer.SquashThreshold.Should().Be(2000);
		peer.AdditionalProperties.Should().ContainKeys("master_uri", "slave_label");
	}

	[Fact]
	public Task Error_RaisesSplunkApiException()
		=> EndpointRequests.ShouldRaiseSplunkErrorAsync((c, ct) => c.LicenseLocalPeer.GetAsync(ct), System.Net.HttpStatusCode.Forbidden);
}
