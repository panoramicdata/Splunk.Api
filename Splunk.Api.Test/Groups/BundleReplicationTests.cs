using Splunk.Api.Models;
using Splunk.Api.Test.Support;
using System.Net;

namespace Splunk.Api.Test.Groups;

public class BundleReplicationTests
{
	private const string Path = "/services/search/distributed";

	// Captured from Splunk Enterprise 10.6.0.5.
	private const string ConfigContent = """
		{
			"bundleTransferTimeout": 0,
			"concerningReplicatedFileSize": 524288000,
			"connectionTimeout": 60,
			"eai:acl": null,
			"maxBundleSize": 2147483648,
			"receiveTimeout": 60,
			"replicationPeriod": 60,
			"replicationPolicy": "classic",
			"replicationThreads": 12,
			"sendTimeout": 60,
			"slowReplicationLoggingInterval": 30,
			"statusQueueSize": 5,
			"warnMaxBundleSizePerc": 0.75
		}
		""";

	// From the reference's example (a 10.6 standalone has no search peers, so no cycles); host names replaced.
	private const string CycleContent = """
		{
			"bundle_id": "sh01-1566601784",
			"current_bundle": "/opt/splunk/var/run/sh01-1566601784.bundle",
			"current_repl_start_time": "1566602286",
			"cycle_id": "80CC124B-2D46-44A7-95C2-A92ECC32C050",
			"delta_path": "/opt/splunk/var/run/sh01-1566601708-1566601784.delta",
			"is_repl_in_progress": "0",
			"peers_status": {
				"https://192.0.2.37:8089": { "classic_replication_state": "succeeded", "peer_name": "https://192.0.2.37:8089", "duration": "1" }
			},
			"replicationPolicy": "classic"
		}
		""";

	private const string FileContent = """
		{
			"checksum": "13134207368020721783",
			"filename": "sh01-1381336958.bundle",
			"location": "/opt/splunk/var/run/sh01-1381336958.bundle",
			"size": "1048576",
			"timestamp": "1381336958"
		}
		""";

	[Fact]
	public async Task GetConfigAsync_SendsGet()
		=> (await RequestProbe.SendAsync((c, ct) => c.BundleReplication.GetConfigAsync(ct)))
			.ShouldBeProbed(HttpMethod.Get, $"{Path}/bundle/replication/config");

	[Fact]
	public async Task ListCyclesAsync_SendsGetWithLatest()
		=> (await RequestProbe.SendAsync((c, ct) => c.BundleReplication.ListCyclesAsync(true, ct)))
			.ShouldBeProbed(HttpMethod.Get, $"{Path}/bundle/replication/cycles", "?latest=true&output_mode=json");

	[Fact]
	public async Task ListCyclesAsync_WithoutLatest_SendsNoParameter()
		=> (await RequestProbe.SendAsync((c, ct) => c.BundleReplication.ListCyclesAsync(null, ct)))
			.ShouldBeProbed(HttpMethod.Get, $"{Path}/bundle/replication/cycles");

	[Fact]
	public async Task ListFilesAsync_SendsGetWithOptions()
		=> (await RequestProbe.SendAsync((c, ct) => c.BundleReplication.ListFilesAsync(new ListOptions { Search = "x" }, ct)))
			.ShouldBeProbed(HttpMethod.Get, $"{Path}/bundle-replication-files", "?search=x&output_mode=json");

	[Fact]
	public async Task GetFileAsync_SendsGetWithForceListAll()
		=> (await RequestProbe.SendAsync((c, ct) => c.BundleReplication.GetFileAsync("13134207368020721783", true, ct)))
			.ShouldBeProbed(HttpMethod.Get, $"{Path}/bundle-replication-files/13134207368020721783", "?force_list_all=true&output_mode=json");

	[Fact]
	public async Task GetConfigAsync_MapsEveryModelledField()
	{
		var config = await RequestProbe.ReadContentAsync((c, ct) => c.BundleReplication.GetConfigAsync(ct), "bundleReplicationConfig", ConfigContent);

		config.ConcerningReplicatedFileSize.Should().Be(524288000);
		config.ConnectionTimeout.Should().Be(60);
		config.MaxBundleSize.Should().Be(2147483648);
		config.ReceiveTimeout.Should().Be(60);
		config.ReplicationPeriod.Should().Be(60);
		config.ReplicationPolicy.Should().Be("classic");
		config.ReplicationThreads.Should().Be(12);
		config.SendTimeout.Should().Be(60);
		config.StatusQueueSize.Should().Be(5);
		config.AdditionalProperties["warnMaxBundleSizePerc"].GetDouble().Should().Be(0.75);
	}

	[Fact]
	public async Task ListCyclesAsync_MapsEveryModelledField()
	{
		var cycle = await RequestProbe.ReadContentAsync((c, ct) => c.BundleReplication.ListCyclesAsync(null, ct), "80CC124B-2D46-44A7-95C2-A92ECC32C050", CycleContent);

		cycle.BundleId.Should().Be("sh01-1566601784");
		cycle.CurrentBundle.Should().Be("/opt/splunk/var/run/sh01-1566601784.bundle");
		cycle.CurrentReplicationStartTime.Should().Be(DateTimeOffset.FromUnixTimeSeconds(1566602286));
		cycle.CycleId.Should().Be("80CC124B-2D46-44A7-95C2-A92ECC32C050");
		cycle.DeltaPath.Should().Be("/opt/splunk/var/run/sh01-1566601708-1566601784.delta");
		cycle.IsReplicationInProgress.Should().BeFalse();
		var peer = cycle.PeersStatus["https://192.0.2.37:8089"];
		peer.ClassicReplicationState.Should().Be("succeeded");
		peer.PeerName.Should().Be("https://192.0.2.37:8089");
		peer.AdditionalProperties["duration"].GetString().Should().Be("1");
		cycle.ReplicationPolicy.Should().Be("classic");
	}

	[Fact]
	public async Task GetFileAsync_MapsEveryModelledField()
	{
		var file = await RequestProbe.ReadContentAsync((c, ct) => c.BundleReplication.GetFileAsync("13134207368020721783", null, ct), "13134207368020721783", FileContent);

		file.Checksum.Should().Be("13134207368020721783");
		file.FileName.Should().Be("sh01-1381336958.bundle");
		file.Location.Should().Be("/opt/splunk/var/run/sh01-1381336958.bundle");
		file.Size.Should().Be(1048576);
		file.Timestamp.Should().Be(DateTimeOffset.FromUnixTimeSeconds(1381336958));
	}

	[Fact]
	public Task GetFileAsync_NotAChecksum_RaisesBadRequest()
		=> RequestProbe.FailsAsync(
			(c, ct) => c.BundleReplication.GetFileAsync("x", null, ct),
			HttpStatusCode.BadRequest,
			"""{"messages":[{"type":"ERROR","text":"Entity name must be a valid checksum number: x"}]}""",
			"Entity name must be a valid checksum number: x");
}
