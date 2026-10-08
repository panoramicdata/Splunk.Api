using Splunk.Api.Models.Inputs;
using Splunk.Api.Test.Support;

namespace Splunk.Api.Test.Groups;

public class HecTokensTests
{
	private const string Path = "/services/data/inputs/http";

	// Captured from Splunk 10.6.0.5 after creating a token; the token value replaced.
	private static readonly string TokenJson = InputsTestKit.Feed("http://app", """
		{
			"_rcvbuf": 1572864, "description": "App events", "disabled": false, "eai:acl": null, "eai:appName": "splunk_httpinput",
			"eai:userName": "nobody", "host": "$decideOnStartup", "index": "main", "indexes": ["main", "_internal"],
			"source": "app", "sourcetype": "app_json", "token": "00000000-0000-0000-0000-000000000004", "useACK": "1"
		}
		""");

	// Captured from Splunk 10.6.0.5 (data/inputs/http/http).
	private static readonly string SettingsJson = InputsTestKit.Feed("http", """
		{
			"_rcvbuf": 1572864, "ackIdleCleanup": "true", "backpressureState": "disabled", "dedicatedIoThreads": "2",
			"disabled": false, "eai:acl": null, "enableSSL": "1", "host": "$decideOnStartup", "index": "default", "indexes": [],
			"maxSockets": "0", "maxThreads": "0", "port": "8088", "sslVersions": "tls1.2, tls1.3", "useDeploymentServer": "0"
		}
		""");

	[Fact]
	public async Task ListAsync_SendsExactRequest()
		=> (await InputsTestKit.CaptureAsync((c, ct) => c.HecTokens.ListAsync(null, ct))).ShouldBeGet(Path);

	[Fact]
	public async Task CreateAsync_SendsExactRequest()
		=> (await InputsTestKit.CaptureAsync((c, ct) => c.HecTokens.CreateAsync(
			new HecTokenCreateRequest
			{
				Name = "app",
				Disabled = false,
				Host = "web01",
				Index = "main",
				Indexes = ["main", "_internal"],
				Source = "app",
				Sourcetype = "app_json",
				UseAck = true,
				Description = "App events"
			},
			ct)))
			.ShouldBePost(Path, "name=app&disabled=false&host=web01&index=main&indexes=main&indexes=_internal&source=app&sourcetype=app_json&useACK=true&description=App+events");

	[Fact]
	public async Task GetAsync_SendsExactRequest()
		=> (await InputsTestKit.CaptureAsync((c, ct) => c.HecTokens.GetAsync("app", ct))).ShouldBeGet(Path + "/app");

	[Fact]
	public async Task UpdateAsync_SendsExactRequest()
		=> (await InputsTestKit.CaptureAsync((c, ct) => c.HecTokens.UpdateAsync("app", new HecTokenUpdateRequest { Sourcetype = "app_v2" }, ct)))
			.ShouldBePost(Path + "/app", "sourcetype=app_v2");

	[Fact]
	public async Task GetSettingsAsync_SendsExactRequest()
		=> (await InputsTestKit.CaptureAsync((c, ct) => c.HecTokens.GetSettingsAsync(HecSettingsUpdateRequest.SettingsName, ct))).ShouldBeGet(Path + "/http");

	[Fact]
	public async Task UpdateSettingsAsync_SendsExactRequest()
		=> (await InputsTestKit.CaptureAsync((c, ct) => c.HecTokens.UpdateSettingsAsync(
			HecSettingsUpdateRequest.SettingsName,
			new HecSettingsUpdateRequest
			{
				Disabled = false,
				EnableSsl = true,
				Port = 8088,
				DedicatedIoThreads = 2,
				MaxSockets = 0,
				MaxThreads = 0,
				UseDeploymentServer = false,
				Index = "main",
				Sourcetype = "hec"
			},
			ct)))
			.ShouldBePost(Path + "/http", "disabled=false&enableSSL=true&port=8088&dedicatedIoThreads=2&maxSockets=0&maxThreads=0&useDeploymentServer=false&index=main&sourcetype=hec");

	[Fact]
	public async Task DeleteAsync_SendsExactRequest()
		=> (await InputsTestKit.CaptureAsync((c, ct) => c.HecTokens.DeleteAsync("app", ct))).ShouldBeDelete(Path + "/app");

	[Fact]
	public async Task DisableAsync_SendsExactRequest()
		=> (await InputsTestKit.CaptureAsync((c, ct) => c.HecTokens.DisableAsync("app", ct))).ShouldBePost(Path + "/app/disable", null);

	[Fact]
	public async Task EnableAsync_SendsExactRequest()
		=> (await InputsTestKit.CaptureAsync((c, ct) => c.HecTokens.EnableAsync("app", ct))).ShouldBePost(Path + "/app/enable", null);

	[Fact]
	public async Task RotateAsync_SendsExactRequest()
		=> (await InputsTestKit.CaptureAsync((c, ct) => c.HecTokens.RotateAsync("app", ct))).ShouldBePost(Path + "/app/rotate", null);

	[Fact]
	public async Task GetAsync_MapsEveryModelledField()
	{
		var entry = await InputsTestKit.MapEntryAsync((c, ct) => c.HecTokens.GetAsync("app", ct), TokenJson);

		entry.Name.Should().Be("http://app");
		var token = entry.Content!;
		token.Token.Should().Be("00000000-0000-0000-0000-000000000004");
		token.Description.Should().Be("App events");
		token.Indexes.Should().Equal("main", "_internal");
		token.UseAck.Should().BeTrue();
		token.EaiAppName.Should().Be("splunk_httpinput");
		token.Index.Should().Be("main");
	}

	[Fact]
	public async Task GetSettingsAsync_MapsEveryModelledField()
	{
		var settings = (await InputsTestKit.MapEntryAsync((c, ct) => c.HecTokens.GetSettingsAsync("http", ct), SettingsJson)).Content!;

		settings.BackpressureState.Should().Be("disabled");
		settings.DedicatedIoThreads.Should().Be(2);
		settings.Disabled.Should().BeFalse();
		settings.EnableSsl.Should().BeTrue();
		settings.Indexes.Should().BeEmpty();
		settings.MaxSockets.Should().Be(0);
		settings.MaxThreads.Should().Be(0);
		settings.Port.Should().Be(8088);
		settings.SslVersions.Should().Be("tls1.2, tls1.3");
		settings.UseDeploymentServer.Should().BeFalse();
		settings.AdditionalProperties["ackIdleCleanup"].GetString().Should().Be("true");
	}

	[Fact]
	public Task GetAsync_Error_RaisesSplunkApiException()
		=> InputsTestKit.ShouldRaiseNotFoundAsync((c, ct) => c.HecTokens.GetAsync("nope", ct));
}
