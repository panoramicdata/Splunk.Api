using Splunk.Api.Models.Licensing;
using Splunk.Api.Test.Support;

namespace Splunk.Api.Test.Groups;

public class LicensePoolsTests
{
	// Captured from Splunk Enterprise 10.6.0.5 (GET licenser/pools), trimmed; host replaced and one pool's peer usage added.
	private const string PoolsJson = """
		{
			"links": { "create": "/services/licenser/pools/_new" },
			"origin": "https://splunk.test:8089/services/licenser/pools",
			"entry": [
				{
					"name": "auto_generated_pool_download-trial",
					"author": "nobody",
					"content": {
						"cisco_licensed_weight": 1,
						"description": "auto_generated_pool_download-trial",
						"eai:acl": null,
						"effective_quota": 524288000,
						"estimated_effective_used_bytes": 12,
						"is_iev_eligible": false,
						"is_unlimited": false,
						"peers": ["*"],
						"peers_usage_bytes": { "00000000-0000-0000-0000-000000000001": 2048 },
						"quota": "MAX",
						"raw_cisco_used_bytes": 0,
						"slaves": ["*"],
						"slaves_usage_bytes": null,
						"stack_id": "download-trial",
						"used_bytes": 2048
					}
				},
				{ "name": "auto_generated_pool_free", "content": { "peers": [], "peers_usage_bytes": null, "quota": "MAX", "stack_id": "free" } }
			],
			"paging": { "total": 2, "perPage": 30, "offset": 0 },
			"messages": []
		}
		""";

	[Fact]
	public async Task ListAsync_SendsGet()
		=> (await RequestAssert.SendAsync((c, ct) => c.LicensePools.ListAsync(null, ct)))
			.ShouldBe(HttpMethod.Get, "/services/licenser/pools");

	[Fact]
	public async Task CreateAsync_PostsThePool()
		=> (await RequestAssert.SendAsync((c, ct) => c.LicensePools.CreateAsync(new LicensePoolCreateRequest { Name = "team_a", Quota = "50MB", StackId = "enterprise", Description = "Team A", Peers = "*" }, ct)))
			.ShouldBe(HttpMethod.Post, "/services/licenser/pools", "name=team_a&quota=50MB&stack_id=enterprise&description=Team+A&peers=%2A");

	[Fact]
	public async Task GetAsync_SendsGetForTheName()
		=> (await RequestAssert.SendAsync((c, ct) => c.LicensePools.GetAsync("team_a", ct)))
			.ShouldBe(HttpMethod.Get, "/services/licenser/pools/team_a");

	[Fact]
	public async Task UpdateAsync_PostsTheChanges()
		=> (await RequestAssert.SendAsync((c, ct) => c.LicensePools.UpdateAsync("team_a", new LicensePoolUpdateRequest { Description = "A", Quota = "MAX", Peers = "guid1,guid2", AppendPeers = true }, ct)))
			.ShouldBe(HttpMethod.Post, "/services/licenser/pools/team_a", "description=A&quota=MAX&peers=guid1%2Cguid2&append_peers=true");

	[Fact]
	public async Task DeleteAsync_SendsDelete()
		=> (await RequestAssert.SendAsync((c, ct) => c.LicensePools.DeleteAsync("team_a", ct)))
			.ShouldBe(HttpMethod.Delete, "/services/licenser/pools/team_a");

	[Fact]
	public async Task Content_MapsEveryModelledField()
	{
		var feed = await RequestAssert.ReadAsync((c, ct) => c.LicensePools.ListAsync(null, ct), PoolsJson);

		var pool = feed.Entries[0].Content!;
		pool.Description.Should().Be("auto_generated_pool_download-trial");
		pool.EffectiveQuota.Should().Be(524288000);
		pool.EstimatedEffectiveUsedBytes.Should().Be(12);
		pool.IsUnlimited.Should().BeFalse();
		pool.Peers.Should().Equal("*");
		pool.PeersUsageBytes.Should().ContainSingle().Which.Value.Should().Be(2048);
		pool.Quota.Should().Be("MAX");
		pool.StackId.Should().Be("download-trial");
		pool.UsedBytes.Should().Be(2048);
		pool.AdditionalProperties.Should().ContainKeys("cisco_licensed_weight", "slaves");
		feed.Entries[1].Content!.PeersUsageBytes.Should().BeNull();
	}

	[Fact]
	public Task Error_RaisesSplunkApiException()
		=> RequestAssert.ShouldRaiseSplunkErrorAsync((c, ct) => c.LicensePools.GetAsync("missing", ct));
}
