using Splunk.Api.Models.Outputs;
using Splunk.Api.Test.Support;

namespace Splunk.Api.Test.Groups;

public class TcpOutputGroupsTests
{
	private const string Path = "/services/data/outputs/tcp/group";

	// Captured from Splunk 10.6.0.5 after creating a group.
	private static readonly string GroupJson = InputsTestKit.Feed("primary", """
		{
			"autoLB": true, "compressed": false, "dropEventsOnQueueFull": -1, "eai:acl": null, "heartbeatFrequency": 45,
			"maxQueueSize": "1MB", "method": "autobalance", "sendCookedData": true, "servers": ["192.0.2.1:9997", "192.0.2.2:9997"]
		}
		""");

	[Fact]
	public async Task ListAsync_SendsExactRequest()
		=> (await InputsTestKit.CaptureAsync((c, ct) => c.TcpOutputGroups.ListAsync(null, ct))).ShouldBeGet(Path);

	[Fact]
	public async Task CreateAsync_SendsExactRequest()
		=> (await InputsTestKit.CaptureAsync((c, ct) => c.TcpOutputGroups.CreateAsync(
			new TcpOutputGroupCreateRequest
			{
				Name = "primary",
				Servers = "192.0.2.1:9997,192.0.2.2:9997",
				Compressed = false,
				Disabled = true,
				DropEventsOnQueueFull = -1,
				HeartbeatFrequency = 45,
				MaxQueueSize = "1MB",
				Method = "autobalance",
				SendCookedData = true,
				Token = "00000000-0000-0000-0000-000000000005"
			},
			ct)))
			.ShouldBePost(Path, "name=primary&servers=192.0.2.1%3A9997%2C192.0.2.2%3A9997&compressed=false&disabled=true&dropEventsOnQueueFull=-1&heartbeatFrequency=45&maxQueueSize=1MB&method=autobalance&sendCookedData=true&token=00000000-0000-0000-0000-000000000005");

	[Fact]
	public async Task GetAsync_SendsExactRequest()
		=> (await InputsTestKit.CaptureAsync((c, ct) => c.TcpOutputGroups.GetAsync("primary", ct))).ShouldBeGet(Path + "/primary");

	[Fact]
	public async Task UpdateAsync_SendsExactRequest()
		=> (await InputsTestKit.CaptureAsync((c, ct) => c.TcpOutputGroups.UpdateAsync("primary", new TcpOutputGroupUpdateRequest { Servers = "192.0.2.1:9997" }, ct)))
			.ShouldBePost(Path + "/primary", "servers=192.0.2.1%3A9997");

	[Fact]
	public async Task DeleteAsync_SendsExactRequest()
		=> (await InputsTestKit.CaptureAsync((c, ct) => c.TcpOutputGroups.DeleteAsync("primary", ct))).ShouldBeDelete(Path + "/primary");

	[Fact]
	public async Task GetAsync_MapsEveryModelledField()
	{
		var group = (await InputsTestKit.MapEntryAsync((c, ct) => c.TcpOutputGroups.GetAsync("primary", ct), GroupJson)).Content!;

		group.AutoLoadBalance.Should().BeTrue();
		group.Compressed.Should().BeFalse();
		group.DropEventsOnQueueFull.Should().Be(-1);
		group.HeartbeatFrequency.Should().Be(45);
		group.MaxQueueSize.Should().Be("1MB");
		group.Method.Should().Be("autobalance");
		group.SendCookedData.Should().BeTrue();
		group.Servers.Should().Equal("192.0.2.1:9997", "192.0.2.2:9997");
	}

	[Fact]
	public Task GetAsync_Error_RaisesSplunkApiException()
		=> InputsTestKit.ShouldRaiseNotFoundAsync((c, ct) => c.TcpOutputGroups.GetAsync("nope", ct));
}
