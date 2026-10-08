using Splunk.Api.Models.Inputs;
using Splunk.Api.Test.Support;

namespace Splunk.Api.Test.Groups;

public class CookedTcpInputsTests
{
	private const string Path = "/services/data/inputs/tcp/cooked";

	// Captured from Splunk 10.6.0.5 (the default receiving port); host names replaced.
	private static readonly string CookedJson = InputsTestKit.Feed("9997", """
		{ "_rcvbuf": 1572864, "connection_host": "dns", "disabled": false, "eai:acl": null, "group": "listenerports", "host": "$decideOnStartup", "host_resolved": "splunk01", "index": "default" }
		""");

	[Fact]
	public async Task ListAsync_SendsExactRequest()
		=> (await InputsTestKit.CaptureAsync((c, ct) => c.CookedTcpInputs.ListAsync(null, ct))).ShouldBeGet(Path);

	[Fact]
	public async Task CreateAsync_SendsExactRequest()
		=> (await InputsTestKit.CaptureAsync((c, ct) => c.CookedTcpInputs.CreateAsync(
			new CookedTcpInputCreateRequest
			{
				Name = "9998",
				Ssl = false,
				ConnectionHost = ConnectionHost.Dns,
				Disabled = true,
				Host = "web01",
				RestrictToHost = "forwarder.example.com"
			},
			ct)))
			.ShouldBePost(Path, "name=9998&SSL=false&connection_host=dns&disabled=true&host=web01&restrictToHost=forwarder.example.com");

	[Fact]
	public async Task GetAsync_SendsExactRequest()
		=> (await InputsTestKit.CaptureAsync((c, ct) => c.CookedTcpInputs.GetAsync("9997", ct))).ShouldBeGet(Path + "/9997");

	[Fact]
	public async Task UpdateAsync_SendsExactRequest()
		=> (await InputsTestKit.CaptureAsync((c, ct) => c.CookedTcpInputs.UpdateAsync("9997", new CookedTcpInputUpdateRequest { Disabled = false }, ct)))
			.ShouldBePost(Path + "/9997", "disabled=false");

	[Fact]
	public async Task DeleteAsync_SendsExactRequest()
		=> (await InputsTestKit.CaptureAsync((c, ct) => c.CookedTcpInputs.DeleteAsync("9998", ct))).ShouldBeDelete(Path + "/9998");

	[Fact]
	public async Task ListConnectionsAsync_SendsExactRequest()
		=> (await InputsTestKit.CaptureAsync((c, ct) => c.CookedTcpInputs.ListConnectionsAsync("9997", ct))).ShouldBeGet(Path + "/9997/connections");

	[Fact]
	public async Task GetAsync_MapsEveryModelledField()
	{
		var input = (await InputsTestKit.MapEntryAsync((c, ct) => c.CookedTcpInputs.GetAsync("9997", ct), CookedJson)).Content!;

		input.ConnectionHost.Should().Be(ConnectionHost.Dns);
		input.Group.Should().Be("listenerports");
		input.Index.Should().Be("default");
		input.Disabled.Should().BeFalse();
	}

	[Fact]
	public async Task ListConnectionsAsync_MapsTheConnection()
	{
		var connection = (await InputsTestKit.MapEntryAsync((c, ct) => c.CookedTcpInputs.ListConnectionsAsync("9997", ct), RawTcpInputsTests.ConnectionJson)).Content!;

		connection.ServerName.Should().Be("forwarder01");
	}

	[Fact]
	public Task GetAsync_Error_RaisesSplunkApiException()
		=> InputsTestKit.ShouldRaiseNotFoundAsync((c, ct) => c.CookedTcpInputs.GetAsync("1", ct));
}
