using Splunk.Api.Models.Deployment;
using Splunk.Api.Test.Support;
using System.Net;

namespace Splunk.Api.Test.Groups;

public class DeploymentServerClientsTests
{
	private const string Path = "/services/deployment/server/clients";
	private const string FilterQuery = "application=app1&hasDeploymentError=false&maxPhonehome_latency_to_avgInterval_ratio=2.5"
		+ "&minLatestPhonehomeTime=1375375291&minPhonehome_latency_to_avgInterval_ratio=0.5&serverclasses=sc1%2Csc2";

	// From the reference's example (a 10.6 standalone has no deployment clients); host names and addresses replaced.
	private const string ClientContent = """
		{
			"applications": {
				"app1": {
					"action": "Install",
					"archive": "/opt/splunk/var/run/tmp/sc_new/app1-1375305443.bundle",
					"restartSplunkWeb": "0",
					"restartSplunkd": "1",
					"result": "Ok",
					"serverclasses": ["sc_new", "sc_apps"],
					"size": "112640",
					"stateOnClient": "enabled",
					"timestamp": "Wed Jul 31 14:11:23 2013"
				}
			},
			"averagePhoneHomeInterval": "60",
			"build": "172889",
			"clientName": "4D4EA12E-FDBA-41D3-99CD-2A61CC1DAB29",
			"dns": "uf01.example.com",
			"eai:acl": null,
			"guid": "dc95537d0e8fdadc44d00c50fc431e25",
			"hasDeploymentError": "0",
			"hostname": "uf01",
			"id": "connection_192.0.2.7_8089_uf01.example.com_uf01_Ombra",
			"ip": "192.0.2.7",
			"lastPhoneHomeTime": "1375375291",
			"mgmt": "8089",
			"name": "Ombra",
			"serverClasses": {
				"sc_apps": {
					"loadTime": "1375305443",
					"repositoryLocation": "/opt/splunk/etc/deployment-apps",
					"restartSplunkWeb": "0",
					"restartSplunkd": "0",
					"stateOnClient": "enabled"
				}
			},
			"utsname": "linux-x86_64"
		}
		""";

	[Fact]
	public async Task ListAsync_SendsGetWithTheFilters()
		=> (await RequestProbe.SendAsync((c, ct) => c.DeploymentServerClients.ListAsync(
			new DeploymentServerClientListOptions
			{
				Action = "phonehome",
				Application = "app1",
				HasDeploymentError = false,
				MaxPhoneHomeLatencyRatio = 2.5,
				MinLatestPhoneHomeTime = 1375375291,
				MinPhoneHomeLatencyRatio = 0.5,
				ServerClasses = "sc1,sc2"
			},
			ct)))
			.ShouldBe(HttpMethod.Get, Path, $"?action=phonehome&{FilterQuery}&output_mode=json");

	[Fact]
	public async Task CountByMachineTypeAsync_SendsGet()
		=> (await RequestProbe.SendAsync((c, ct) => c.DeploymentServerClients.CountByMachineTypeAsync(ct)))
			.ShouldBe(HttpMethod.Get, $"{Path}/countClients_by_machineType");

	[Fact]
	public async Task CountRecentDownloadsAsync_SendsGetWithMaxAgeSecs()
		=> (await RequestProbe.SendAsync((c, ct) => c.DeploymentServerClients.CountRecentDownloadsAsync(3600, ct)))
			.ShouldBe(HttpMethod.Get, $"{Path}/countRecentDownloads", "?maxAgeSecs=3600&output_mode=json");

	[Fact]
	public async Task GetAsync_SendsGetWithTheFilters()
		=> (await RequestProbe.SendAsync((c, ct) => c.DeploymentServerClients.GetAsync(
			"dc95537d",
			new DeploymentServerClientFilter
			{
				Application = "app1",
				HasDeploymentError = false,
				MaxPhoneHomeLatencyRatio = 2.5,
				MinLatestPhoneHomeTime = 1375375291,
				MinPhoneHomeLatencyRatio = 0.5,
				ServerClasses = "sc1,sc2"
			},
			ct)))
			.ShouldBe(HttpMethod.Get, $"{Path}/dc95537d", $"?{FilterQuery}&output_mode=json");

	[Fact]
	public async Task DeleteAsync_SendsDelete()
		=> (await RequestProbe.SendAsync((c, ct) => c.DeploymentServerClients.DeleteAsync("dc95537d", ct)))
			.ShouldBe(HttpMethod.Delete, $"{Path}/dc95537d");

	[Fact]
	public async Task GetAsync_MapsEveryModelledField()
	{
		var client = await RequestProbe.ReadContentAsync((c, ct) => c.DeploymentServerClients.GetAsync("dc95537d", null, ct), "dc95537d", ClientContent);

		var app = client.Applications["app1"];
		app.Action.Should().Be("Install");
		app.Archive.Should().Be("/opt/splunk/var/run/tmp/sc_new/app1-1375305443.bundle");
		app.RestartSplunkWeb.Should().BeFalse();
		app.RestartSplunkd.Should().BeTrue();
		app.Result.Should().Be("Ok");
		app.ServerClasses.Should().Equal("sc_new", "sc_apps");
		app.Size.Should().Be(112640);
		app.StateOnClient.Should().Be(DeploymentAppState.Enabled);
		app.Timestamp.Should().Be("Wed Jul 31 14:11:23 2013");
		client.AveragePhoneHomeInterval.Should().Be(60);
		client.Build.Should().Be("172889");
		client.ClientName.Should().Be("4D4EA12E-FDBA-41D3-99CD-2A61CC1DAB29");
		client.Dns.Should().Be("uf01.example.com");
		client.ClientGuid.Should().Be("dc95537d0e8fdadc44d00c50fc431e25");
		client.HasDeploymentError.Should().BeFalse();
		client.Hostname.Should().Be("uf01");
		client.ConnectionId.Should().Be("connection_192.0.2.7_8089_uf01.example.com_uf01_Ombra");
		client.Ip.Should().Be("192.0.2.7");
		client.LastPhoneHomeTime.Should().Be(DateTimeOffset.FromUnixTimeSeconds(1375375291));
		client.ManagementPort.Should().Be(8089);
		client.ClientDisplayName.Should().Be("Ombra");
		var serverClass = client.ServerClasses["sc_apps"];
		serverClass.LoadTime.Should().Be(DateTimeOffset.FromUnixTimeSeconds(1375305443));
		serverClass.RepositoryLocation.Should().Be("/opt/splunk/etc/deployment-apps");
		serverClass.RestartSplunkWeb.Should().BeFalse();
		serverClass.RestartSplunkd.Should().BeFalse();
		serverClass.StateOnClient.Should().Be(DeploymentAppState.Enabled);
		client.UtsName.Should().Be("linux-x86_64");
	}

	[Fact]
	public async Task CountByMachineTypeAsync_MapsTheCounts()
		=> (await RequestProbe.ReadContentAsync((c, ct) => c.DeploymentServerClients.CountByMachineTypeAsync(ct), "default", """{"counts":{"linux-x86_64":"3"}}"""))
			.Counts.Should().ContainKey("linux-x86_64").WhoseValue.Should().Be(3);

	[Fact]
	public async Task CountByMachineTypeAsync_WithoutClients_HasNullCounts()
		=> (await RequestProbe.ReadContentAsync((c, ct) => c.DeploymentServerClients.CountByMachineTypeAsync(ct), "default", """{"counts":null}"""))
			.Counts.Should().BeNull();

	[Fact]
	public async Task CountRecentDownloadsAsync_MapsTheCount()
		=> (await RequestProbe.ReadContentAsync((c, ct) => c.DeploymentServerClients.CountRecentDownloadsAsync(1, ct), "default", """{"count":"6"}"""))
			.Count.Should().Be(6);

	[Fact]
	public Task DeleteAsync_OnSplunk106_RaisesTheDeprecationError()
		=> RequestProbe.FailsAsync(
			(c, ct) => c.DeploymentServerClients.DeleteAsync("dc95537d", ct),
			HttpStatusCode.NotFound,
			"""{"messages":[{"type":"ERROR","text":"This functionality has been deprecated"}]}""",
			"This functionality has been deprecated");
}
