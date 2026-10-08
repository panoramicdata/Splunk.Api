using Splunk.Api.Test.Support;
using System.Net;

namespace Splunk.Api.Test.Groups;

public class ClusterSearchHeadGenerationsTests
{
	private const string Path = "/services/cluster/searchhead/generation";

	[Fact]
	public async Task ListAsync_SendsGet()
		=> (await RequestProbe.SendAsync((c, ct) => c.ClusterSearchHeadGenerations.ListAsync(null, ct)))
			.ShouldBeProbed(HttpMethod.Get, Path);

	[Fact]
	public async Task GetAsync_SendsGetWithTheDoubleEncodedManagerUri()
		=> (await RequestProbe.SendAsync((c, ct) => c.ClusterSearchHeadGenerations.GetAsync("https:%2F%2Fcm:8089", ct)))
			.ShouldBeProbed(HttpMethod.Get, $"{Path}/https%3A%252F%252Fcm%3A8089");

	[Fact]
	public async Task GetAsync_MapsTheGeneration()
	{
		// From the reference's example.
		var generation = await RequestProbe.ReadContentAsync(
			(c, ct) => c.ClusterSearchHeadGenerations.GetAsync("x", ct),
			"https://cm:8089",
			"""{"generation_id":"3","generation_peers":{"33333333-3333-3333-3333-333333333333":{"host_port_pair":"10.1.42.3:53309","peer":"peer3"}}}""");

		generation.GenerationId.Should().Be(3);
		generation.GenerationPeers["33333333-3333-3333-3333-333333333333"].Peer.Should().Be("peer3");
		generation.PendingGenerationId.Should().BeNull();
	}

	[Fact]
	public Task ListAsync_NotASearchHead_RaisesServiceUnavailable()
		=> RequestProbe.FailsAsync(
			(c, ct) => c.ClusterSearchHeadGenerations.ListAsync(null, ct),
			HttpStatusCode.ServiceUnavailable,
			"""{"messages":[{"type":"ERROR","text":"Search head or cluster manager is not enabled on this node."}]}""",
			"Search head or cluster manager is not enabled on this node.");
}
