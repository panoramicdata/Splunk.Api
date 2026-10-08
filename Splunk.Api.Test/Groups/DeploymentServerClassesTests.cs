using Splunk.Api.Models.Deployment;
using Splunk.Api.Test.Support;
using System.Net;

namespace Splunk.Api.Test.Groups;

public class DeploymentServerClassesTests
{
	private const string Path = "/services/deployment/server/serverclasses";

	private const string SettingsBody = "continueMatching=false&filterType=whitelist&machineTypesFilter=linux-x86_64&repositoryLocation=%2Frepo"
		+ "&restartSplunkWeb=true&restartSplunkd=false&stateOnClient=enabled&targetRepositoryLocation=%2Fapps&tmpFolder=%2Ftmp&whitelist.0=web%2A";

	// From the reference's example (a 10.6 standalone has no server classes), with the update fields added.
	private const string ServerClassContent = """
		{
			"blacklist-size": "0",
			"clientId": "dc95537d0e8fdadc44d00c50fc431e25",
			"continueMatching": "true",
			"currentDownloads": "2",
			"eai:acl": null,
			"filterType": "blacklist",
			"hasDeploymentError": "1",
			"loadTime": "1375305443",
			"machineTypesFilter": "linux-x86_64,",
			"repositoryList": {},
			"repositoryLocation": "/opt/splunk/etc/deployment-apps",
			"restartSplunkWeb": "0",
			"restartSplunkd": "1",
			"stateOnClient": "enabled",
			"targetRepositoryLocation": "$SPLUNK_HOME/etc/apps",
			"tmpFolder": "$SPLUNK_HOME/var/run/tmp",
			"whitelist-size": "1",
			"whitelist.0": "web*"
		}
		""";

	private static DeploymentServerClassUpdateRequest Settings => new()
	{
		ContinueMatching = false,
		FilterType = DeploymentFilterType.Whitelist,
		MachineTypesFilter = "linux-x86_64",
		RepositoryLocation = "/repo",
		RestartSplunkWeb = true,
		RestartSplunkd = false,
		StateOnClient = DeploymentAppState.Enabled,
		TargetRepositoryLocation = "/apps",
		TmpFolder = "/tmp",
		AdditionalParameters = { ["whitelist.0"] = "web*" }
	};

	[Fact]
	public async Task ListAsync_SendsGet()
		=> (await RequestProbe.SendAsync((c, ct) => c.DeploymentServerClasses.ListAsync(null, ct)))
			.ShouldBeProbed(HttpMethod.Get, Path);

	[Fact]
	public async Task CreateAsync_SendsPostWithTheServerClass()
	{
		var settings = Settings;

		var call = await RequestProbe.SendAsync((c, ct) => c.DeploymentServerClasses.CreateAsync(
			new DeploymentServerClassCreateRequest
			{
				Name = "sc_web",
				ContinueMatching = settings.ContinueMatching,
				FilterType = settings.FilterType,
				MachineTypesFilter = settings.MachineTypesFilter,
				RepositoryLocation = settings.RepositoryLocation,
				RestartSplunkWeb = settings.RestartSplunkWeb,
				RestartSplunkd = settings.RestartSplunkd,
				StateOnClient = settings.StateOnClient,
				TargetRepositoryLocation = settings.TargetRepositoryLocation,
				TmpFolder = settings.TmpFolder,
				AdditionalParameters = settings.AdditionalParameters
			},
			ct));

		call.ShouldBeProbed(HttpMethod.Post, Path, body: $"name=sc_web&{SettingsBody}");
	}

	[Fact]
	public async Task RenameAsync_SendsPostWithBothNames()
		=> (await RequestProbe.SendAsync((c, ct) => c.DeploymentServerClasses.RenameAsync(new DeploymentServerClassRenameRequest { OldName = "a", NewName = "b" }, ct)))
			.ShouldBeProbed(HttpMethod.Post, $"{Path}/rename", body: "oldName=a&newName=b");

	[Fact]
	public async Task GetAsync_SendsGetWithTheFilter()
		=> (await RequestProbe.SendAsync((c, ct) => c.DeploymentServerClasses.GetAsync("sc_web", new DeploymentServerClassFilter { ClientId = "abc", HasDeploymentError = true }, ct)))
			.ShouldBeProbed(HttpMethod.Get, $"{Path}/sc_web", "?clientId=abc&hasDeploymentError=true&output_mode=json");

	[Fact]
	public async Task UpdateAsync_SendsPostWithTheSetFields()
		=> (await RequestProbe.SendAsync((c, ct) => c.DeploymentServerClasses.UpdateAsync("sc_web", Settings, ct)))
			.ShouldBeProbed(HttpMethod.Post, $"{Path}/sc_web", body: SettingsBody);

	[Fact]
	public async Task DeleteAsync_SendsDelete()
		=> (await RequestProbe.SendAsync((c, ct) => c.DeploymentServerClasses.DeleteAsync("sc_web", ct)))
			.ShouldBeProbed(HttpMethod.Delete, $"{Path}/sc_web");

	[Fact]
	public async Task GetAsync_MapsEveryModelledField()
	{
		var serverClass = await RequestProbe.ReadContentAsync((c, ct) => c.DeploymentServerClasses.GetAsync("sc_web", null, ct), "sc_web", ServerClassContent);

		serverClass.BlacklistSize.Should().Be(0);
		serverClass.WhitelistSize.Should().Be(1);
		serverClass.ClientId.Should().Be("dc95537d0e8fdadc44d00c50fc431e25");
		serverClass.CurrentDownloads.Should().Be(2);
		serverClass.HasDeploymentError.Should().BeTrue();
		serverClass.LoadTime.Should().Be(DateTimeOffset.FromUnixTimeSeconds(1375305443));
		serverClass.ContinueMatching.Should().BeTrue();
		serverClass.FilterType.Should().Be(DeploymentFilterType.Blacklist);
		serverClass.MachineTypesFilter.Should().Be("linux-x86_64,");
		serverClass.RepositoryLocation.Should().Be("/opt/splunk/etc/deployment-apps");
		serverClass.RestartSplunkWeb.Should().BeFalse();
		serverClass.RestartSplunkd.Should().BeTrue();
		serverClass.StateOnClient.Should().Be(DeploymentAppState.Enabled);
		serverClass.TargetRepositoryLocation.Should().Be("$SPLUNK_HOME/etc/apps");
		serverClass.TmpFolder.Should().Be("$SPLUNK_HOME/var/run/tmp");
		serverClass.AdditionalProperties["whitelist.0"].GetString().Should().Be("web*");
	}

	[Fact]
	public Task DeleteAsync_Missing_RaisesTheNoConfigError()
		=> RequestProbe.FailsAsync(
			(c, ct) => c.DeploymentServerClasses.DeleteAsync("x", ct),
			HttpStatusCode.InternalServerError,
			"""{"messages":[{"type":"ERROR","text":"No config found: sc=x"}]}""",
			"No config found: sc=x");
}
