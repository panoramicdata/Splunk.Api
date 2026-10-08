using Splunk.Api.Models.Outputs;
using Splunk.Api.Test.Support;

namespace Splunk.Api.Test.Groups;

public class TcpOutputServersTests
{
	private const string Path = "/services/data/outputs/tcp/server";
	private const string Item = Path + "/192.0.2.1%3A9997";

	// Captured from Splunk 10.6.0.5 after adding a receiver at a documentation address.
	private static readonly string ServerJson = InputsTestKit.Feed("192.0.2.1:9997", """
		{
			"destHost": "192.0.2.1", "destIp": "192.0.2.1", "destPort": 9997, "disabled": true, "eai:acl": null, "method": "autobalance",
			"sourcePort": 8089, "sslVerifyServerCert": false, "status": "not_connected"
		}
		""");

	[Fact]
	public async Task ListAsync_SendsExactRequest()
		=> (await InputsTestKit.CaptureAsync((c, ct) => c.TcpOutputServers.ListAsync(null, ct))).ShouldBeGet(Path);

	[Fact]
	public async Task CreateAsync_SendsExactRequest()
		=> (await InputsTestKit.CaptureAsync((c, ct) => c.TcpOutputServers.CreateAsync(
			new TcpOutputServerCreateRequest
			{
				Name = "192.0.2.1:9997",
				Disabled = true,
				Method = "autobalance",
				SslAltNameToCheck = "idx.example.com",
				SslCertPath = "/certs/client.pem",
				SslCipher = "HIGH",
				SslCommonNameToCheck = "idx",
				SslPassword = "secret",
				SslRootCaPath = "/certs/ca.pem",
				SslVerifyServerCert = true
			},
			ct)))
			.ShouldBePost(Path, "name=192.0.2.1%3A9997&disabled=true&method=autobalance&sslAltNameToCheck=idx.example.com&sslCertPath=%2Fcerts%2Fclient.pem&sslCipher=HIGH&sslCommonNameToCheck=idx&sslPassword=secret&sslRootCAPath=%2Fcerts%2Fca.pem&sslVerifyServerCert=true");

	[Fact]
	public async Task GetAsync_SendsExactRequest()
		=> (await InputsTestKit.CaptureAsync((c, ct) => c.TcpOutputServers.GetAsync("192.0.2.1:9997", ct))).ShouldBeGet(Item);

	[Fact]
	public async Task UpdateAsync_SendsExactRequest()
		=> (await InputsTestKit.CaptureAsync((c, ct) => c.TcpOutputServers.UpdateAsync("192.0.2.1:9997", new TcpOutputServerUpdateRequest { Method = "clone" }, ct)))
			.ShouldBePost(Item, "method=clone");

	[Fact]
	public async Task DeleteAsync_SendsExactRequest()
		=> (await InputsTestKit.CaptureAsync((c, ct) => c.TcpOutputServers.DeleteAsync("192.0.2.1:9997", ct))).ShouldBeDelete(Item);

	[Fact]
	public async Task ListConnectionsAsync_SendsExactRequest()
		=> (await InputsTestKit.CaptureAsync((c, ct) => c.TcpOutputServers.ListConnectionsAsync("192.0.2.1:9997", ct))).ShouldBeGet(Item + "/allconnections");

	[Fact]
	public async Task GetAsync_MapsEveryModelledField()
	{
		var server = (await InputsTestKit.MapEntryAsync((c, ct) => c.TcpOutputServers.GetAsync("192.0.2.1:9997", ct), ServerJson)).Content!;

		server.DestinationHost.Should().Be("192.0.2.1");
		server.DestinationIp.Should().Be("192.0.2.1");
		server.DestinationPort.Should().Be(9997);
		server.Disabled.Should().BeTrue();
		server.Method.Should().Be("autobalance");
		server.SourcePort.Should().Be(8089);
		server.SslVerifyServerCert.Should().BeFalse();
		server.Status.Should().Be("not_connected");
	}

	[Fact]
	public Task GetAsync_Error_RaisesSplunkApiException()
		=> InputsTestKit.ShouldRaiseNotFoundAsync((c, ct) => c.TcpOutputServers.GetAsync("nope", ct));
}
