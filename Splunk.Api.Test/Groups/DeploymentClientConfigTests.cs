using Splunk.Api.Models;
using Splunk.Api.Test.Support;
using System.Net;

namespace Splunk.Api.Test.Groups;

public class DeploymentClientConfigTests
{
	private const string Path = "/services/deployment/client";

	// The reference's example for an enabled client (a 10.6 standalone answers {"disabled":true} only).
	private const string ConfigContent = """
		{
			"disabled": "0",
			"eai:acl": null,
			"serverClasses": ["sc_apps:app1", "sc_mach_type:app2"],
			"targetUri": "ds.example.com:8089"
		}
		""";

	[Fact]
	public async Task ListAsync_SendsGetWithOptions()
		=> (await RequestProbe.SendAsync((c, ct) => c.DeploymentClientConfig.ListAsync(new ListOptions { Offset = 1 }, ct)))
			.ShouldBe(HttpMethod.Get, Path, "?offset=1&output_mode=json");

	[Fact]
	public async Task GetAsync_SendsGet()
		=> (await RequestProbe.SendAsync((c, ct) => c.DeploymentClientConfig.GetAsync(ct)))
			.ShouldBe(HttpMethod.Get, $"{Path}/config");

	[Fact]
	public async Task GetDisabledStatusAsync_SendsGet()
		=> (await RequestProbe.SendAsync((c, ct) => c.DeploymentClientConfig.GetDisabledStatusAsync(ct)))
			.ShouldBe(HttpMethod.Get, $"{Path}/config/listIsDisabled");

	[Fact]
	public async Task ReloadConfigAsync_SendsPost()
		=> (await RequestProbe.SendAsync((c, ct) => c.DeploymentClientConfig.ReloadConfigAsync(ct)))
			.ShouldBe(HttpMethod.Post, $"{Path}/config/reload");

	[Fact]
	public async Task ReloadAsync_SendsPostWithTheName()
		=> (await RequestProbe.SendAsync((c, ct) => c.DeploymentClientConfig.ReloadAsync("deployment-client", ct)))
			.ShouldBe(HttpMethod.Post, $"{Path}/deployment-client/reload");

	[Fact]
	public async Task GetAsync_MapsEveryModelledField()
	{
		var config = await RequestProbe.ReadContentAsync((c, ct) => c.DeploymentClientConfig.GetAsync(ct), "config", ConfigContent);

		config.Disabled.Should().BeFalse();
		config.ServerClasses.Should().Equal("sc_apps:app1", "sc_mach_type:app2");
		config.TargetUri.Should().Be("ds.example.com:8089");
	}

	[Fact]
	public async Task GetDisabledStatusAsync_MapsDisabled()
		=> (await RequestProbe.ReadContentAsync((c, ct) => c.DeploymentClientConfig.GetDisabledStatusAsync(ct), "default", """{"disabled":true,"eai:acl":null}"""))
			.Disabled.Should().BeTrue();

	[Fact]
	public Task GetAsync_Error_RaisesSplunkApiException()
		=> RequestProbe.FailsAsync(
			(c, ct) => c.DeploymentClientConfig.GetAsync(ct),
			HttpStatusCode.Forbidden,
			"""{"messages":[{"type":"ERROR","text":"Insufficient permissions"}]}""",
			"Insufficient permissions");
}
