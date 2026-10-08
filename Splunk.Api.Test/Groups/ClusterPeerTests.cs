using Splunk.Api.Models.Cluster;
using Splunk.Api.Test.Support;
using System.Net;

namespace Splunk.Api.Test.Groups;

public class ClusterPeerTests
{
	private const string Path = "/services/cluster/peer";

	// From the reference's example.
	private const string InfoContent = """
		{
			"active_bundle": { "bundle_path": "/opt/splunk/var/run/splunk/cluster/remote-bundle/0f60-1346858928.bundle", "checksum": "36a883f4", "timestamp": "1346858928" },
			"base_generation_id": "2",
			"eai:acl": null,
			"invalid_bundle_ids": ["bad1"],
			"is_registered": "1",
			"last_heartbeat_attempt": "1346874358",
			"latest_bundle": { "bundle_path": "/b", "checksum": "36a883f4", "timestamp": "1346858928" },
			"restart_state": "NoRestart",
			"status": "Up"
		}
		""";

	[Fact]
	public async Task GetInfoAsync_SendsGet()
		=> (await RequestProbe.SendAsync((c, ct) => c.ClusterPeer.GetInfoAsync(ct)))
			.ShouldBeProbed(HttpMethod.Get, $"{Path}/info");

	[Fact]
	public async Task DecommissionAsync_SendsPost()
		=> (await RequestProbe.SendAsync((c, ct) => c.ClusterPeer.DecommissionAsync(ct)))
			.ShouldBeProbed(HttpMethod.Post, $"{Path}/control/control/decommission");

	[Fact]
	public async Task ReAddAsync_SendsPostWithClearMasks()
		=> (await RequestProbe.SendAsync((c, ct) => c.ClusterPeer.ReAddAsync(new ClusterPeerReAddRequest { ClearMasks = false }, ct)))
			.ShouldBeProbed(HttpMethod.Post, $"{Path}/control/control/re-add-peer", body: "clearMasks=false");

	[Fact]
	public async Task SetManualDetentionAsync_SendsPostWithTheState()
		=> (await RequestProbe.SendAsync((c, ct) => c.ClusterPeer.SetManualDetentionAsync(new ManualDetentionRequest { ManualDetention = ManualDetentionMode.OnPortsEnabled }, ct)))
			.ShouldBeProbed(HttpMethod.Post, $"{Path}/control/control/set_manual_detention", body: "manual_detention=on_ports_enabled");

	[Fact]
	public async Task GetInfoAsync_MapsEveryModelledField()
	{
		var info = await RequestProbe.ReadContentAsync((c, ct) => c.ClusterPeer.GetInfoAsync(ct), "peer", InfoContent);

		info.ActiveBundle!.Checksum.Should().Be("36a883f4");
		info.ActiveBundle.Timestamp.Should().Be(DateTimeOffset.FromUnixTimeSeconds(1346858928));
		info.BaseGenerationId.Should().Be(2);
		info.InvalidBundleIds.Should().Equal("bad1");
		info.IsRegistered.Should().BeTrue();
		info.LastHeartbeatAttempt.Should().Be(DateTimeOffset.FromUnixTimeSeconds(1346874358));
		info.LatestBundle!.BundlePath.Should().Be("/b");
		info.RestartState.Should().Be("NoRestart");
		info.Status.Should().Be("Up");
	}

	[Fact]
	public Task GetInfoAsync_NotAPeer_RaisesServiceUnavailable()
		=> RequestProbe.FailsAsync(
			(c, ct) => c.ClusterPeer.GetInfoAsync(ct),
			HttpStatusCode.ServiceUnavailable,
			ClusterErrors.PeerNotEnabled,
			"Cluster peer is not enabled on this node, check clustering stanza in server.conf");
}
