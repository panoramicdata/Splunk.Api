using Splunk.Api.Models.Deployment;
using Splunk.Api.Test.Support;
using System.Net;

namespace Splunk.Api.Test.Groups;

public class DeploymentServerApplicationsTests
{
	private const string Path = "/services/deployment/server/applications";

	// From the reference's example (a 10.6 standalone has no deployment apps), with the update fields added.
	private const string ApplicationContent = """
		{
			"archive": "/opt/splunk/var/run/tmp/sc_new/app1-1375467593.bundle",
			"clientId": "dc95537d0e8fdadc44d00c50fc431e25",
			"continueMatching": "1",
			"eai:acl": null,
			"filterType": "whitelist",
			"hasDeploymentError": "0",
			"loadtime": "Fri Aug 2 11:19:53 2013",
			"machineTypesFilter": "linux-x86_64",
			"repositoryLocation": "$SPLUNK_HOME/etc/deployment-apps",
			"restartSplunkWeb": "0",
			"restartSplunkd": "1",
			"serverclass": "sc_new",
			"serverclasses": ["sc_new", "sc_apps"],
			"size": "112640",
			"stateOnClient": "noop",
			"targetRepositoryLocation": "$SPLUNK_HOME/etc/apps",
			"tmpFolder": "$SPLUNK_HOME/var/run/tmp",
			"whitelist.0": "web*"
		}
		""";

	[Fact]
	public async Task ListAsync_SendsGetWithTheFilters()
		=> (await RequestProbe.SendAsync((c, ct) => c.DeploymentServerApplications.ListAsync(
			new DeploymentApplicationListOptions { ClientId = "abc", HasDeploymentError = true, Count = 5 },
			ct)))
			.ShouldBeProbed(HttpMethod.Get, Path, "?clientId=abc&hasDeploymentError=true&count=5&output_mode=json");

	[Fact]
	public async Task GetAsync_SendsGetWithTheName()
		=> (await RequestProbe.SendAsync((c, ct) => c.DeploymentServerApplications.GetAsync("app1", ct)))
			.ShouldBeProbed(HttpMethod.Get, $"{Path}/app1");

	[Fact]
	public async Task UpdateAsync_SendsPostWithTheSetFields()
		=> (await RequestProbe.SendAsync((c, ct) => c.DeploymentServerApplications.UpdateAsync(
			"app1",
			new DeploymentApplicationUpdateRequest
			{
				ServerClass = "sc_new",
				Deinstall = false,
				Unmap = false,
				ContinueMatching = true,
				FilterType = DeploymentFilterType.Blacklist,
				MachineTypesFilter = "linux-*",
				RepositoryLocation = "/repo",
				RestartSplunkWeb = false,
				RestartSplunkd = true,
				StateOnClient = DeploymentAppState.Disabled,
				TargetRepositoryLocation = "/apps",
				TmpFolder = "/tmp",
				AdditionalParameters = { ["whitelist.0"] = "web*" }
			},
			ct)))
			.ShouldBeProbed(HttpMethod.Post, $"{Path}/app1", body:
				"serverclass=sc_new&deinstall=false&unmap=false&continueMatching=true&filterType=blacklist&machineTypesFilter=linux-%2A"
				+ "&repositoryLocation=%2Frepo&restartSplunkWeb=false&restartSplunkd=true&stateOnClient=disabled&targetRepositoryLocation=%2Fapps"
				+ "&tmpFolder=%2Ftmp&whitelist.0=web%2A");

	[Fact]
	public async Task GetAsync_MapsEveryModelledField()
	{
		var app = await RequestProbe.ReadContentAsync((c, ct) => c.DeploymentServerApplications.GetAsync("app1", ct), "app1", ApplicationContent);

		app.Archive.Should().Be("/opt/splunk/var/run/tmp/sc_new/app1-1375467593.bundle");
		app.ClientId.Should().Be("dc95537d0e8fdadc44d00c50fc431e25");
		app.HasDeploymentError.Should().BeFalse();
		app.LoadTime.Should().Be("Fri Aug 2 11:19:53 2013");
		app.ServerClasses.Should().Equal("sc_new", "sc_apps");
		app.ServerClass.Should().Be("sc_new");
		app.Size.Should().Be(112640);
		app.ContinueMatching.Should().BeTrue();
		app.FilterType.Should().Be(DeploymentFilterType.Whitelist);
		app.MachineTypesFilter.Should().Be("linux-x86_64");
		app.RepositoryLocation.Should().Be("$SPLUNK_HOME/etc/deployment-apps");
		app.RestartSplunkWeb.Should().BeFalse();
		app.RestartSplunkd.Should().BeTrue();
		app.StateOnClient.Should().Be(DeploymentAppState.Noop);
		app.TargetRepositoryLocation.Should().Be("$SPLUNK_HOME/etc/apps");
		app.TmpFolder.Should().Be("$SPLUNK_HOME/var/run/tmp");
		app.AdditionalProperties["whitelist.0"].GetString().Should().Be("web*");
	}

	[Fact]
	public Task ListAsync_UnknownClient_RaisesBadRequest()
		=> RequestProbe.FailsAsync(
			(c, ct) => c.DeploymentServerApplications.ListAsync(new DeploymentApplicationListOptions { ClientId = "x" }, ct),
			HttpStatusCode.BadRequest,
			"""{"messages":[{"type":"ERROR","text":"No client id=x"}]}""",
			"No client id=x");
}
