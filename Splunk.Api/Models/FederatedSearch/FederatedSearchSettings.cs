using System.Text.Json.Serialization;

namespace Splunk.Api.Models.FederatedSearch;

/// <summary>The general federated search settings of the deployment (<c>data/federated/settings/general</c>).</summary>
/// <remarks>These settings do not apply to Federated Search for Amazon S3. <see cref="SplunkContent.Disabled"/> reports whether federated search is turned off.</remarks>
public sealed class FederatedSearchSettings : SplunkContent
{
	/// <summary>Whether transparent mode federated search is turned on, so searches can run over transparent mode providers as well as standard mode ones. After changing it, reload <c>configs/conf-federated</c> for it to take effect.</summary>
	[JsonPropertyName("transparent_mode")]
	public bool? TransparentMode { get; init; }

	/// <summary>Whether federated searches can run in verbose mode.</summary>
	[JsonPropertyName("verbose_mode")]
	public bool? VerboseMode { get; init; }

	/// <summary>Whether Splunk Web asks users to acknowledge the compliance implications of providers and federated index permissions.</summary>
	[JsonPropertyName("needs_consent")]
	public bool? NeedsConsent { get; init; }

	/// <summary>Whether providers are filtered by the federated indexes a search names (with each provider's <c>fedSrchIndexesAllowed</c>). Change only when Splunk Support says so.</summary>
	[JsonPropertyName("allowIndexBasedProviderFiltering")]
	public bool? AllowIndexBasedProviderFiltering { get; init; }

	/// <summary>Whether the heartbeat that monitors remote providers runs.</summary>
	[JsonPropertyName("heartbeatEnabled")]
	public bool? HeartbeatEnabled { get; init; }

	/// <summary>Whether the federated search head can send actions such as search cancellation to providers.</summary>
	[JsonPropertyName("controlCommandsFeatureEnabled")]
	public bool? ControlCommandsFeatureEnabled { get; init; }

	/// <summary>The maximum number of threads that run federated search actions.</summary>
	[JsonPropertyName("controlCommandsMaxThreads")]
	public int? ControlCommandsMaxThreads { get; init; }

	/// <summary>The maximum number of seconds to wait for a federated search action to complete.</summary>
	[JsonPropertyName("controlCommandsMaxTimeThreshold")]
	public int? ControlCommandsMaxTimeThreshold { get; init; }

	/// <summary>The maximum number of seconds spent generating result previews; 0 means unlimited.</summary>
	[JsonPropertyName("max_preview_generation_duration")]
	public int? MaxPreviewGenerationDuration { get; init; }

	/// <summary>How long, in seconds, a proxy bundle lives on the remote search head after its last use.</summary>
	[JsonPropertyName("proxyBundlesTTL")]
	public int? ProxyBundlesTtl { get; init; }

	/// <summary>The maximum number of event download retries in a verbose-mode federated search.</summary>
	[JsonPropertyName("remoteEventsDownloadRetryCountMax")]
	public int? RemoteEventsDownloadRetryCountMax { get; init; }

	/// <summary>The interval, in milliseconds, between event download retries.</summary>
	[JsonPropertyName("remoteEventsDownloadRetryTimeoutMs")]
	public int? RemoteEventsDownloadRetryTimeoutMs { get; init; }
}
