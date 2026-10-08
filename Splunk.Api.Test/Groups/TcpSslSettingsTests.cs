using Splunk.Api.Models.Inputs;
using Splunk.Api.Test.Support;

namespace Splunk.Api.Test.Groups;

public class TcpSslSettingsTests
{
	private const string Path = "/services/data/inputs/tcp/ssl";

	// Captured from Splunk 10.6.0.5; host names replaced and unset certificate settings filled in.
	private static readonly string SslJson = InputsTestKit.Feed(string.Empty, """
		{
			"_rcvbuf": 1572864, "allowSslRenegotiation": true, "cipherSuite": "ECDHE-RSA-AES256-GCM-SHA384", "disabled": true,
			"eai:acl": null, "ecdhCurves": "prime256v1", "host": "$decideOnStartup", "host_resolved": "splunk01", "index": "default",
			"requireClientCert": false, "rootCA": "/opt/splunk/etc/auth/cacert.pem", "serverCert": "/opt/splunk/etc/auth/server.pem",
			"sslVersions": "tls1.2, tls1.3"
		}
		""");

	[Fact]
	public async Task ListAsync_SendsExactRequest()
		=> (await InputsTestKit.CaptureAsync((c, ct) => c.TcpSslSettings.ListAsync(null, ct))).ShouldBeGet(Path);

	[Fact]
	public async Task GetAsync_SendsExactRequest()
		=> (await InputsTestKit.CaptureAsync((c, ct) => c.TcpSslSettings.GetAsync("ssl", ct))).ShouldBeGet(Path + "/ssl");

	[Fact]
	public async Task UpdateAsync_SendsExactRequest()
		=> (await InputsTestKit.CaptureAsync((c, ct) => c.TcpSslSettings.UpdateAsync(
			"ssl",
			new TcpSslSettingsUpdateRequest
			{
				Disabled = false,
				Password = "secret",
				RequireClientCert = true,
				RootCa = "/certs/ca.pem",
				ServerCert = "/certs/server.pem"
			},
			ct)))
			.ShouldBePost(Path + "/ssl", "disabled=false&password=secret&requireClientCert=true&rootCA=%2Fcerts%2Fca.pem&serverCert=%2Fcerts%2Fserver.pem");

	[Fact]
	public async Task ListAsync_MapsEveryModelledField()
	{
		var entry = await InputsTestKit.MapEntryAsync((c, ct) => c.TcpSslSettings.ListAsync(null, ct), SslJson);

		entry.Name.Should().BeEmpty();
		var ssl = entry.Content!;
		ssl.AllowSslRenegotiation.Should().BeTrue();
		ssl.CipherSuite.Should().Be("ECDHE-RSA-AES256-GCM-SHA384");
		ssl.EcdhCurves.Should().Be("prime256v1");
		ssl.RequireClientCert.Should().BeFalse();
		ssl.RootCa.Should().Be("/opt/splunk/etc/auth/cacert.pem");
		ssl.ServerCert.Should().Be("/opt/splunk/etc/auth/server.pem");
		ssl.SslVersions.Should().Be("tls1.2, tls1.3");
	}

	[Fact]
	public Task GetAsync_Error_RaisesSplunkApiException()
		=> InputsTestKit.ShouldRaiseNotFoundAsync((c, ct) => c.TcpSslSettings.GetAsync("nope", ct));
}
