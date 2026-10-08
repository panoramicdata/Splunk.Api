using Splunk.Api.Models.Inputs;
using Splunk.Api.Test.Support;

namespace Splunk.Api.Test.Groups;

public class UdpInputsTests
{
	private const string Path = "/services/data/inputs/udp";

	// Captured from Splunk 10.6.0.5 after creating a UDP input; host names replaced.
	private static readonly string UdpJson = InputsTestKit.Feed("47013", """
		{
			"_rcvbuf": 1572864, "connection_host": "ip", "disabled": true, "eai:acl": null, "group": "listenerports",
			"host": "$decideOnStartup", "host_resolved": "splunk01", "index": "main", "no_appending_timestamp": true,
			"no_priority_stripping": true, "queue": "parsingQueue", "restrictToHost": "10.0.0.6", "sourcetype": "syslog"
		}
		""");

	[Fact]
	public async Task ListAsync_SendsExactRequest()
		=> (await InputsTestKit.CaptureAsync((c, ct) => c.UdpInputs.ListAsync(null, ct))).ShouldBeGet(Path);

	[Fact]
	public async Task CreateAsync_SendsExactRequest()
		=> (await InputsTestKit.CaptureAsync((c, ct) => c.UdpInputs.CreateAsync(
			new UdpInputCreateRequest
			{
				Name = "47013",
				ConnectionHost = ConnectionHost.Ip,
				Disabled = false,
				Host = "web01",
				Index = "main",
				NoAppendingTimestamp = true,
				NoPriorityStripping = true,
				Queue = "parsingQueue",
				RestrictToHost = "10.0.0.6",
				Source = "udp",
				Sourcetype = "syslog"
			},
			ct)))
			.ShouldBePost(Path, "name=47013&connection_host=ip&disabled=false&host=web01&index=main&no_appending_timestamp=true&no_priority_stripping=true&queue=parsingQueue&restrictToHost=10.0.0.6&source=udp&sourcetype=syslog");

	[Fact]
	public async Task GetAsync_SendsExactRequest()
		=> (await InputsTestKit.CaptureAsync((c, ct) => c.UdpInputs.GetAsync("47013", ct))).ShouldBeGet(Path + "/47013");

	[Fact]
	public async Task UpdateAsync_SendsExactRequest()
		=> (await InputsTestKit.CaptureAsync((c, ct) => c.UdpInputs.UpdateAsync("47013", new UdpInputUpdateRequest { Sourcetype = "syslog" }, ct)))
			.ShouldBePost(Path + "/47013", "sourcetype=syslog");

	[Fact]
	public async Task DeleteAsync_SendsExactRequest()
		=> (await InputsTestKit.CaptureAsync((c, ct) => c.UdpInputs.DeleteAsync("47013", ct))).ShouldBeDelete(Path + "/47013");

	[Fact]
	public async Task ListConnectionsAsync_SendsExactRequest()
		=> (await InputsTestKit.CaptureAsync((c, ct) => c.UdpInputs.ListConnectionsAsync("47013", ct))).ShouldBeGet(Path + "/47013/connections");

	[Fact]
	public async Task GetAsync_MapsEveryModelledField()
	{
		var input = (await InputsTestKit.MapEntryAsync((c, ct) => c.UdpInputs.GetAsync("47013", ct), UdpJson)).Content!;

		input.ConnectionHost.Should().Be(ConnectionHost.Ip);
		input.Disabled.Should().BeTrue();
		input.Group.Should().Be("listenerports");
		input.NoAppendingTimestamp.Should().BeTrue();
		input.NoPriorityStripping.Should().BeTrue();
		input.Queue.Should().Be("parsingQueue");
		input.RestrictToHost.Should().Be("10.0.0.6");
		input.Sourcetype.Should().Be("syslog");
	}

	[Fact]
	public async Task ListConnectionsAsync_MapsTheConnection()
	{
		var connection = (await InputsTestKit.MapEntryAsync((c, ct) => c.UdpInputs.ListConnectionsAsync("47013", ct), RawTcpInputsTests.ConnectionJson)).Content!;

		connection.Connection.Should().Be("10.0.0.5:52144");
	}

	[Fact]
	public Task GetAsync_Error_RaisesSplunkApiException()
		=> InputsTestKit.ShouldRaiseNotFoundAsync((c, ct) => c.UdpInputs.GetAsync("1", ct));
}
