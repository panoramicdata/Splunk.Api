using Splunk.Api.Test.Support.Platform;
using System.Net;

namespace Splunk.Api.Test.Groups;

public class ServerSettingsTests
{
	// Captured from Splunk 10.6.0.5; host names and the key replaced.
	private const string SettingsContent = """
		{
			"SPLUNK_DB": "/opt/splunk/var/lib/splunk",
			"SPLUNK_HOME": "/opt/splunk",
			"appServerPorts": [8065],
			"caTrustStore": "splunk",
			"eai:acl": null,
			"enableSplunkWebSSL": false,
			"host": "splunk01",
			"host_resolved": "splunk01.example.com",
			"httpport": 8000,
			"kvStoreDisabled": false,
			"kvStorePort": 8191,
			"mgmtHostPort": 8089,
			"minFreeSpace": 5000,
			"pass4SymmKey": "$8$encrypted",
			"serverName": "splunk01",
			"sessionTimeout": "1h",
			"startwebserver": true,
			"trustedIP": ""
		}
		""";

	[Fact]
	public async Task GetAsync_SendsGet()
		=> await Calls.AssertAsync(c => c.ServerSettings.GetAsync(Calls.Token), HttpMethod.Get, "/services/server/settings", Calls.JsonQuery, null);

	[Fact]
	public async Task GetAsync_MapsEveryModelledField()
	{
		var feed = await Calls.MapAsync(c => c.ServerSettings.GetAsync(Calls.Token), Feed.Of("settings", SettingsContent));

		var settings = feed.Entries.Should().ContainSingle().Subject.Content!;
		settings.SplunkDb.Should().Be("/opt/splunk/var/lib/splunk");
		settings.SplunkHome.Should().Be("/opt/splunk");
		settings.AppServerPorts.Should().Equal(8065);
		settings.EnableSplunkWebSsl.Should().BeFalse();
		settings.Host.Should().Be("splunk01");
		settings.HostResolved.Should().Be("splunk01.example.com");
		settings.HttpPort.Should().Be(8000);
		settings.KvStoreDisabled.Should().BeFalse();
		settings.KvStorePort.Should().Be(8191);
		settings.ManagementPort.Should().Be(8089);
		settings.MinFreeSpaceMB.Should().Be(5000);
		settings.Pass4SymmKey.Should().Be("$8$encrypted");
		settings.ServerName.Should().Be("splunk01");
		settings.SessionTimeout.Should().Be("1h");
		settings.StartWebServer.Should().BeTrue();
		settings.TrustedIP.Should().BeEmpty();
		settings.AdditionalProperties.Should().ContainKey("caTrustStore");
	}

	[Fact]
	public async Task GetAsync_Error_RaisesSplunkApiException()
		=> await Calls.AssertErrorAsync(c => c.ServerSettings.GetAsync(Calls.Token), HttpStatusCode.Forbidden);
}
