using Splunk.Api.Test.Support;
using System.Net;

namespace Splunk.Api.Test.Groups;

public class DeploymentServerConfigTests
{
	private const string Path = "/services/deployment/server/config";

	// Captured from Splunk Enterprise 10.6.0.5 (GET deployment/server/config), with the reference's whitelist entry added.
	private const string ConfigContent = """
		{
			"clientMatchingCacheVersion": 2,
			"currentDownloads": "0",
			"disabled": false,
			"eai:acl": null,
			"loadTime": 1791465098,
			"repositoryLocation": "$SPLUNK_HOME/etc/deployment-apps",
			"whitelist.0": "*"
		}
		""";

	[Fact]
	public async Task PostAsync_SendsPostWithTheFields()
		=> (await RequestProbe.SendAsync((c, ct) => c.DeploymentServerConfig.PostAsync(new Dictionary<string, string?> { ["count"] = "1" }, ct)))
			.ShouldBe(HttpMethod.Post, Path, body: "count=1");

	[Fact]
	public async Task ListUnsupportedAttributesAsync_SendsGet()
		=> (await RequestProbe.SendAsync((c, ct) => c.DeploymentServerConfig.ListUnsupportedAttributesAsync(ct)))
			.ShouldBe(HttpMethod.Get, $"{Path}/attributesUnsupportedInUI");

	[Fact]
	public async Task GetDisabledStatusAsync_SendsGet()
		=> (await RequestProbe.SendAsync((c, ct) => c.DeploymentServerConfig.GetDisabledStatusAsync(ct)))
			.ShouldBe(HttpMethod.Get, $"{Path}/listIsDisabled");

	[Fact]
	public async Task PostAsync_MapsTheConfiguration()
	{
		var config = await RequestProbe.ReadContentAsync((c, ct) => c.DeploymentServerConfig.PostAsync(new Dictionary<string, string?>(), ct), "config", ConfigContent);

		config.CurrentDownloads.Should().Be(0);
		config.Disabled.Should().BeFalse();
		config.LoadTime.Should().Be(DateTimeOffset.FromUnixTimeSeconds(1791465098));
		config.RepositoryLocation.Should().Be("$SPLUNK_HOME/etc/deployment-apps");
		config.AdditionalProperties["whitelist.0"].GetString().Should().Be("*");
	}

	[Fact]
	public async Task ListUnsupportedAttributesAsync_MapsEveryModelledField()
	{
		var setting = await RequestProbe.ReadContentAsync(
			(c, ct) => c.DeploymentServerConfig.ListUnsupportedAttributesAsync(ct),
			"item_0",
			"""{"property":"whitelist.0","reason":"unsupported at this level","stanza":"global"}""");

		setting.Property.Should().Be("whitelist.0");
		setting.Reason.Should().Be("unsupported at this level");
		setting.Stanza.Should().Be("global");
	}

	[Fact]
	public Task PostAsync_OnSplunk106_RaisesTheMissingTargetError()
		=> RequestProbe.FailsAsync(
			(c, ct) => c.DeploymentServerConfig.PostAsync(new Dictionary<string, string?>(), ct),
			HttpStatusCode.BadRequest,
			"""{"messages":[{"type":"ERROR","text":"Cannot perform action \"POST\" without a target name to act on."}]}""",
			"Cannot perform action \"POST\" without a target name to act on.");
}
