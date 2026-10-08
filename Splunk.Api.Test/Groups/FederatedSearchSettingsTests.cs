using Splunk.Api.Models.FederatedSearch;
using Splunk.Api.Test.Support;
using System.Net;

namespace Splunk.Api.Test.Groups;

public class FederatedSearchSettingsTests
{
	private const string Path = "/services/data/federated/settings/general";

	// Captured from Splunk Enterprise 10.6.0.5, trimmed; heartbeatEnabled and verbose_mode added from the reference.
	private const string SettingsContent = """
		{
			"allowIndexBasedProviderFiltering": "1",
			"controlCommandsFeatureEnabled": "1",
			"controlCommandsMaxThreads": "5",
			"controlCommandsMaxTimeThreshold": "5",
			"disabled": false,
			"eai:acl": null,
			"heartbeatEnabled": "1",
			"max_preview_generation_duration": "0",
			"needs_consent": "1",
			"proxyBundlesTTL": "172800",
			"remoteEventsDownloadRetryCountMax": "20",
			"remoteEventsDownloadRetryTimeoutMs": "1000",
			"transparent_mode": true,
			"verbose_mode": "0",
			"federated_search_remote_ttl": 600
		}
		""";

	[Fact]
	public async Task GetAsync_SendsGet()
		=> (await RequestProbe.SendAsync((c, ct) => c.FederatedSearchSettings.GetAsync(ct)))
			.ShouldBe(HttpMethod.Get, Path);

	[Fact]
	public async Task UpdateAsync_SendsPostWithTheSetFields()
		=> (await RequestProbe.SendAsync((c, ct) => c.FederatedSearchSettings.UpdateAsync(
			new FederatedSearchSettingsUpdateRequest
			{
				Disabled = false,
				TransparentMode = false,
				AllowIndexBasedProviderFiltering = true,
				ControlCommandsFeatureEnabled = true,
				ControlCommandsMaxThreads = 6,
				ControlCommandsMaxTimeThreshold = 7,
				HeartbeatEnabled = true,
				MaxPreviewGenerationDuration = 30,
				NeedsConsent = false,
				ProxyBundlesTtl = 3600,
				RemoteEventsDownloadRetryCountMax = 10,
				RemoteEventsDownloadRetryTimeoutMs = 500,
				VerboseMode = true
			},
			ct)))
			.ShouldBe(HttpMethod.Post, Path, body:
				"disabled=false&transparent_mode=false&allowIndexBasedProviderFiltering=true&controlCommandsFeatureEnabled=true"
				+ "&controlCommandsMaxThreads=6&controlCommandsMaxTimeThreshold=7&heartbeatEnabled=true&max_preview_generation_duration=30"
				+ "&needs_consent=false&proxyBundlesTTL=3600&remoteEventsDownloadRetryCountMax=10&remoteEventsDownloadRetryTimeoutMs=500"
				+ "&verbose_mode=true");

	[Fact]
	public async Task GetAsync_MapsEveryModelledField()
	{
		var settings = await RequestProbe.ReadContentAsync((c, ct) => c.FederatedSearchSettings.GetAsync(ct), "general", SettingsContent);

		settings.Disabled.Should().BeFalse();
		settings.TransparentMode.Should().BeTrue();
		settings.AllowIndexBasedProviderFiltering.Should().BeTrue();
		settings.ControlCommandsFeatureEnabled.Should().BeTrue();
		settings.ControlCommandsMaxThreads.Should().Be(5);
		settings.ControlCommandsMaxTimeThreshold.Should().Be(5);
		settings.HeartbeatEnabled.Should().BeTrue();
		settings.MaxPreviewGenerationDuration.Should().Be(0);
		settings.NeedsConsent.Should().BeTrue();
		settings.ProxyBundlesTtl.Should().Be(172800);
		settings.RemoteEventsDownloadRetryCountMax.Should().Be(20);
		settings.RemoteEventsDownloadRetryTimeoutMs.Should().Be(1000);
		settings.VerboseMode.Should().BeFalse();
		settings.AdditionalProperties["federated_search_remote_ttl"].GetInt32().Should().Be(600);
	}

	[Fact]
	public Task UpdateAsync_Error_RaisesSplunkApiException()
		=> RequestProbe.FailsAsync(
			(c, ct) => c.FederatedSearchSettings.UpdateAsync(new FederatedSearchSettingsUpdateRequest(), ct),
			HttpStatusCode.Forbidden,
			"""{"messages":[{"type":"ERROR","text":"You (user=reader) do not have permission to perform this operation (requires capability: edit_federated_provider)."}]}""",
			"You (user=reader) do not have permission to perform this operation (requires capability: edit_federated_provider).");
}
