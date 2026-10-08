using Splunk.Api.Models.Cluster;
using Splunk.Api.Test.Support;
using System.Net;

namespace Splunk.Api.Test.Groups;

public class ClusterManagerGenerationsTests
{
	private const string Path = "/services/cluster/manager/generation";

	// From the reference's example (POST response, which adds the *_met and was_forced flags).
	private const string GenerationContent = """
		{
			"eai:acl": null,
			"generation_id": "5",
			"generation_peers": {
				"11111111-1111-1111-1111-111111111111": { "host_port_pair": "idx1.example.com:6431", "peer": "PEER1" }
			},
			"pending_generation_id": "6",
			"pending_last_attempt": "1447117547",
			"pending_last_reason": "",
			"replication_factor_met": "1",
			"search_factor_met": "0",
			"was_forced": "0"
		}
		""";

	[Fact]
	public async Task ListAsync_SendsGet()
		=> (await RequestProbe.SendAsync((c, ct) => c.ClusterManagerGenerations.ListAsync(null, ct)))
			.ShouldBeProbed(HttpMethod.Get, Path);

	[Fact]
	public async Task CreateAsync_SendsPostWithTheSearchHead()
		=> (await RequestProbe.SendAsync((c, ct) => c.ClusterManagerGenerations.CreateAsync(
			new ClusterGenerationCreateRequest { Name = "https://sh1:8089", GenerationPollInterval = 62, Label = "SH1", ManagementPort = "8089", RegisterSearchAddress = "10.0.0.5" },
			ct)))
			.ShouldBeProbed(HttpMethod.Post, Path, body:
				"name=https%3A%2F%2Fsh1%3A8089&generation_poll_interval=62&label=SH1&mgmt_port=8089&register_search_address=10.0.0.5");

	[Fact]
	public async Task GetAsync_SendsGetWithTheName()
		=> (await RequestProbe.SendAsync((c, ct) => c.ClusterManagerGenerations.GetAsync("manager", ct)))
			.ShouldBeProbed(HttpMethod.Get, $"{Path}/manager");

	[Fact]
	public async Task UpdateAsync_SendsPostWithTheSettings()
		=> (await RequestProbe.SendAsync((c, ct) => c.ClusterManagerGenerations.UpdateAsync(
			"SH-GUID",
			new ClusterGenerationUpdateRequest { GenerationPollInterval = 62, Label = "PEER2", ManagementPort = "8089", RegisterSearchAddress = "10.0.0.6" },
			ct)))
			.ShouldBeProbed(HttpMethod.Post, $"{Path}/SH-GUID", body: "generation_poll_interval=62&label=PEER2&mgmt_port=8089&register_search_address=10.0.0.6");

	[Fact]
	public async Task GetAsync_MapsEveryModelledField()
	{
		var generation = await RequestProbe.ReadContentAsync((c, ct) => c.ClusterManagerGenerations.GetAsync("manager", ct), "manager", GenerationContent);

		generation.GenerationId.Should().Be(5);
		var peer = generation.GenerationPeers["11111111-1111-1111-1111-111111111111"];
		peer.HostPortPair.Should().Be("idx1.example.com:6431");
		peer.Peer.Should().Be("PEER1");
		generation.PendingGenerationId.Should().Be(6);
		generation.PendingLastAttempt.Should().Be(DateTimeOffset.FromUnixTimeSeconds(1447117547));
		generation.PendingLastReason.Should().BeEmpty();
		generation.ReplicationFactorMet.Should().BeTrue();
		generation.SearchFactorMet.Should().BeFalse();
		generation.WasForced.Should().BeFalse();
	}

	[Fact]
	public Task ListAsync_NotAManager_RaisesServiceUnavailable()
		=> RequestProbe.FailsAsync(
			(c, ct) => c.ClusterManagerGenerations.ListAsync(null, ct),
			HttpStatusCode.ServiceUnavailable,
			ClusterErrors.ManagerNotEnabled,
			"Cluster manager is not enabled on this node");
}
