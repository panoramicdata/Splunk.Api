using Splunk.Api.Models.Outputs;
using Splunk.Api.Test.Support;

namespace Splunk.Api.Test.Groups;

public class TcpOutputDefaultsTests
{
	private const string Path = "/services/data/outputs/tcp/default";

	// Captured from Splunk 10.6.0.5, trimmed.
	private static readonly string DefaultsJson = InputsTestKit.Feed("tcpout", """
		{
			"autoLBFrequency": 30, "compressed": false, "connectionTimeout": 20, "defaultGroup": "primary", "disabled": false,
			"dropEventsOnQueueFull": -1, "eai:acl": null, "forwardedindex.0.whitelist": ".*", "forwardedindex.filter.disable": false,
			"heartbeatFrequency": 30, "indexAndForward": false, "maxQueueSize": "auto", "readTimeout": 300, "sendCookedData": true,
			"useACK": false, "writeTimeout": 300
		}
		""");

	[Fact]
	public async Task ListAsync_SendsExactRequest()
		=> (await InputsTestKit.CaptureAsync((c, ct) => c.TcpOutputDefaults.ListAsync(null, ct))).ShouldBeGet(Path);

	[Fact]
	public async Task CreateAsync_SendsExactRequest()
		=> (await InputsTestKit.CaptureAsync((c, ct) => c.TcpOutputDefaults.CreateAsync(
			new TcpOutputDefaultsCreateRequest
			{
				Name = "tcpout",
				DefaultGroup = "primary",
				Disabled = false,
				DropEventsOnQueueFull = -1,
				HeartbeatFrequency = 30,
				IndexAndForward = true,
				MaxQueueSize = "7MB",
				SendCookedData = true
			},
			ct)))
			.ShouldBePost(Path, "name=tcpout&defaultGroup=primary&disabled=false&dropEventsOnQueueFull=-1&heartbeatFrequency=30&indexAndForward=true&maxQueueSize=7MB&sendCookedData=true");

	[Fact]
	public async Task GetAsync_SendsExactRequest()
		=> (await InputsTestKit.CaptureAsync((c, ct) => c.TcpOutputDefaults.GetAsync("tcpout", ct))).ShouldBeGet(Path + "/tcpout");

	[Fact]
	public async Task UpdateAsync_SendsExactRequest()
		=> (await InputsTestKit.CaptureAsync((c, ct) => c.TcpOutputDefaults.UpdateAsync("tcpout", new TcpOutputDefaultsUpdateRequest { HeartbeatFrequency = 45 }, ct)))
			.ShouldBePost(Path + "/tcpout", "heartbeatFrequency=45");

	[Fact]
	public async Task DeleteAsync_SendsExactRequest()
		=> (await InputsTestKit.CaptureAsync((c, ct) => c.TcpOutputDefaults.DeleteAsync("tcpout", ct))).ShouldBeDelete(Path + "/tcpout");

	[Fact]
	public async Task GetAsync_MapsEveryModelledField()
	{
		var defaults = (await InputsTestKit.MapEntryAsync((c, ct) => c.TcpOutputDefaults.GetAsync("tcpout", ct), DefaultsJson)).Content!;

		defaults.AutoLoadBalanceFrequency.Should().Be(30);
		defaults.Compressed.Should().BeFalse();
		defaults.ConnectionTimeout.Should().Be(20);
		defaults.DefaultGroup.Should().Be("primary");
		defaults.DropEventsOnQueueFull.Should().Be(-1);
		defaults.ForwardedIndexFilterDisabled.Should().BeFalse();
		defaults.HeartbeatFrequency.Should().Be(30);
		defaults.IndexAndForward.Should().BeFalse();
		defaults.MaxQueueSize.Should().Be("auto");
		defaults.ReadTimeout.Should().Be(300);
		defaults.SendCookedData.Should().BeTrue();
		defaults.UseAck.Should().BeFalse();
		defaults.WriteTimeout.Should().Be(300);
		defaults.AdditionalProperties["forwardedindex.0.whitelist"].GetString().Should().Be(".*");
	}

	[Fact]
	public Task GetAsync_Error_RaisesSplunkApiException()
		=> InputsTestKit.ShouldRaiseNotFoundAsync((c, ct) => c.TcpOutputDefaults.GetAsync("nope", ct));
}
