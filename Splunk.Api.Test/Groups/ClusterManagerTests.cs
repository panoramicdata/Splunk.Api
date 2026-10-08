using Splunk.Api.Models.Cluster;
using Splunk.Api.Test.Support;
using System.Net;

namespace Splunk.Api.Test.Groups;

public partial class ClusterManagerTests
{
	private const string Path = "/services/cluster/manager";

	[Fact]
	public async Task GetInfoAsync_SendsGet()
		=> (await RequestProbe.SendAsync((c, ct) => c.ClusterManager.GetInfoAsync(ct)))
			.ShouldBeProbed(HttpMethod.Get, $"{Path}/info");

	[Fact]
	public async Task GetHealthAsync_SendsGet()
		=> (await RequestProbe.SendAsync((c, ct) => c.ClusterManager.GetHealthAsync(ct)))
			.ShouldBeProbed(HttpMethod.Get, $"{Path}/health");

	[Fact]
	public async Task GetStatusAsync_SendsGet()
		=> (await RequestProbe.SendAsync((c, ct) => c.ClusterManager.GetStatusAsync(ct)))
			.ShouldBeProbed(HttpMethod.Get, $"{Path}/status");

	[Fact]
	public async Task GetHaActiveStatusAsync_SendsGet()
		=> (await RequestProbe.SendAsync((c, ct) => c.ClusterManager.GetHaActiveStatusAsync(ct)))
			.ShouldBeProbed(HttpMethod.Get, $"{Path}/ha_active_status");

	[Fact]
	public async Task ListFixupsAsync_SendsGetWithTheLevelAndIndex()
		=> (await RequestProbe.SendAsync((c, ct) => c.ClusterManager.ListFixupsAsync(ClusterFixupLevel.ReplicationFactor, new ClusterFixupListOptions { Index = "main" }, ct)))
			.ShouldBeProbed(HttpMethod.Get, $"{Path}/fixup", "?level=replication_factor&index=main&output_mode=json");

	[Fact]
	public async Task ListRedundancyAsync_SendsGet()
		=> (await RequestProbe.SendAsync((c, ct) => c.ClusterManager.ListRedundancyAsync(ct)))
			.ShouldBeProbed(HttpMethod.Get, $"{Path}/redundancy");

	[Fact]
	public async Task SwitchHaModeAsync_SendsPostWithTheActionAndMode()
		=> (await RequestProbe.SendAsync((c, ct) => c.ClusterManager.SwitchHaModeAsync(new ClusterHaModeSwitchRequest { HaMode = ClusterHaMode.Active }, ct)))
			.ShouldBeProbed(HttpMethod.Post, $"{Path}/redundancy", body: "ha_mode=Active&_action=switch_mode");

	[Fact]
	public Task GetHaActiveStatusAsync_Standby_RaisesServiceUnavailable()
		=> RequestProbe.FailsAsync(
			(c, ct) => c.ClusterManager.GetHaActiveStatusAsync(ct),
			HttpStatusCode.ServiceUnavailable,
			"""<?xml version="1.0" encoding="UTF-8"?><response><messages><msg type="ERROR">Cluster manager is in inactive mode.</msg></messages></response>""",
			"Cluster manager is in inactive mode.");
}
