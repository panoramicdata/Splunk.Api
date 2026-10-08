using Splunk.Api.Models.Inputs;
using Splunk.Api.Test.Support;

namespace Splunk.Api.Test.Groups;

public class RawTcpInputsTests
{
	private const string Path = "/services/data/inputs/tcp/raw";

	// Captured from Splunk 10.6.0.5 after creating a raw TCP input; host names replaced.
	internal static readonly string TcpJson = InputsTestKit.Feed("47011", """
		{
			"SSL": false, "_rcvbuf": 1572864, "connection_host": "ip", "disabled": false, "eai:acl": null, "group": "listenerports",
			"host": "web01", "host_resolved": "splunk01", "index": "main", "queue": "parsingQueue", "rawTcpDoneTimeout": 5,
			"restrictToHost": "forwarder.example.com", "source": "tcp:47011", "sourcetype": "app_tcp"
		}
		""");

	// Shaped as the reference documents; a standalone test instance has no live connections.
	internal static readonly string ConnectionJson = InputsTestKit.Feed("10.0.0.5:52144", """
		{ "connection": "10.0.0.5:52144", "eai:acl": null, "group": "listenerports", "servername": "forwarder01" }
		""");

	[Fact]
	public async Task ListAsync_SendsExactRequest()
		=> (await InputsTestKit.CaptureAsync((c, ct) => c.RawTcpInputs.ListAsync(null, ct))).ShouldBeGet(Path);

	[Fact]
	public async Task CreateAsync_SendsExactRequest()
		=> (await InputsTestKit.CaptureAsync((c, ct) => c.RawTcpInputs.CreateAsync(
			new RawTcpInputCreateRequest
			{
				Name = "47011",
				ConnectionHost = ConnectionHost.Ip,
				Disabled = false,
				Host = "web01",
				Index = "main",
				Queue = "parsingQueue",
				RawTcpDoneTimeout = 5,
				RestrictToHost = "forwarder.example.com",
				Ssl = false,
				Source = "tcp",
				Sourcetype = "app_tcp"
			},
			ct)))
			.ShouldBePost(Path, "name=47011&connection_host=ip&disabled=false&host=web01&index=main&queue=parsingQueue&rawTcpDoneTimeout=5&restrictToHost=forwarder.example.com&SSL=false&source=tcp&sourcetype=app_tcp");

	[Fact]
	public async Task GetAsync_SendsExactRequest()
		=> (await InputsTestKit.CaptureAsync((c, ct) => c.RawTcpInputs.GetAsync("10.0.0.5:47011", ct))).ShouldBeGet(Path + "/10.0.0.5%3A47011");

	[Fact]
	public async Task UpdateAsync_SendsExactRequest()
		=> (await InputsTestKit.CaptureAsync((c, ct) => c.RawTcpInputs.UpdateAsync("47011", new RawTcpInputUpdateRequest { ConnectionHost = ConnectionHost.None }, ct)))
			.ShouldBePost(Path + "/47011", "connection_host=none");

	[Fact]
	public async Task DeleteAsync_SendsExactRequest()
		=> (await InputsTestKit.CaptureAsync((c, ct) => c.RawTcpInputs.DeleteAsync("47011", ct))).ShouldBeDelete(Path + "/47011");

	[Fact]
	public async Task ListConnectionsAsync_SendsExactRequest()
		=> (await InputsTestKit.CaptureAsync((c, ct) => c.RawTcpInputs.ListConnectionsAsync("47011", ct))).ShouldBeGet(Path + "/47011/connections");

	[Fact]
	public async Task GetAsync_MapsEveryModelledField()
	{
		var entry = await InputsTestKit.MapEntryAsync((c, ct) => c.RawTcpInputs.GetAsync("47011", ct), TcpJson);

		entry.Name.Should().Be("47011");
		var input = entry.Content!;
		input.Ssl.Should().BeFalse();
		input.ConnectionHost.Should().Be(ConnectionHost.Ip);
		input.Group.Should().Be("listenerports");
		input.Host.Should().Be("web01");
		input.Index.Should().Be("main");
		input.Queue.Should().Be("parsingQueue");
		input.RawTcpDoneTimeout.Should().Be(5);
		input.RestrictToHost.Should().Be("forwarder.example.com");
		input.Source.Should().Be("tcp:47011");
		input.Sourcetype.Should().Be("app_tcp");
	}

	[Fact]
	public async Task ListConnectionsAsync_MapsEveryModelledField()
	{
		var connection = (await InputsTestKit.MapEntryAsync((c, ct) => c.RawTcpInputs.ListConnectionsAsync("47011", ct), ConnectionJson)).Content!;

		connection.Connection.Should().Be("10.0.0.5:52144");
		connection.ServerName.Should().Be("forwarder01");
		connection.Group.Should().Be("listenerports");
	}

	[Fact]
	public Task GetAsync_Error_RaisesSplunkApiException()
		=> InputsTestKit.ShouldRaiseNotFoundAsync((c, ct) => c.RawTcpInputs.GetAsync("1", ct));
}
