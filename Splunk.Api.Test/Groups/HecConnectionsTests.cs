using Splunk.Api.Test.Support;

namespace Splunk.Api.Test.Groups;

public class HecConnectionsTests
{
	private const string Path = "/services/data/inputs/http/connections";

	// Shaped as the reference documents (Splunk 10.6.0.5 listed none on the test instance).
	private static readonly string ConnectionJson = InputsTestKit.Feed("10.0.0.7", """{ "eai:acl": null, "ip_address": "10.0.0.7", "last_conn_time": 1791468575 }""");

	[Fact]
	public async Task ListAsync_SendsExactRequest()
		=> (await InputsTestKit.CaptureAsync((c, ct) => c.HecConnections.ListAsync(ct))).ShouldBeGet(Path);

	[Fact]
	public async Task GetAsync_SendsExactRequest()
		=> (await InputsTestKit.CaptureAsync((c, ct) => c.HecConnections.GetAsync("10.0.0.7", ct))).ShouldBeGet(Path + "/10.0.0.7");

	[Fact]
	public async Task GetAsync_MapsEveryModelledField()
	{
		var entry = await InputsTestKit.MapEntryAsync((c, ct) => c.HecConnections.GetAsync("10.0.0.7", ct), ConnectionJson);

		entry.Name.Should().Be("10.0.0.7");
		entry.Content!.IpAddress.Should().Be("10.0.0.7");
		entry.Content.LastConnectionTime.Should().Be(DateTimeOffset.FromUnixTimeSeconds(1791468575));
	}

	[Fact]
	public Task GetAsync_Error_RaisesSplunkApiException()
		=> InputsTestKit.ShouldRaiseNotFoundAsync((c, ct) => c.HecConnections.GetAsync("127.0.0.2", ct));
}
