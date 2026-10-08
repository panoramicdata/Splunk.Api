using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Deployment;

/// <summary>An app the deployment server distributes (<c>deployment/server/applications</c>).</summary>
/// <remarks>Whitelist and blacklist entries (<c>whitelist.0</c>, ...) are in <see cref="SplunkContent.AdditionalProperties"/>.</remarks>
public sealed class DeploymentApplication : SplunkContent
{
	/// <summary>The path of the compressed app bundle.</summary>
	[JsonPropertyName("archive")]
	public string? Archive { get; init; }

	/// <summary>The deployment client the information relates to, when filtered by client.</summary>
	[JsonPropertyName("clientId")]
	public string? ClientId { get; init; }

	/// <summary>Whether the app failed to deploy to at least one client.</summary>
	[JsonPropertyName("hasDeploymentError")]
	public bool? HasDeploymentError { get; init; }

	/// <summary>When the deployment server last loaded the app, as Splunk formats it (for example <c>Wed Jul 31 14:17:23 2013</c>); an app mapped to no server class is not loaded.</summary>
	[JsonPropertyName("loadtime")]
	public string? LoadTime { get; init; }

	/// <summary>The server classes the app belongs to.</summary>
	[JsonPropertyName("serverclasses")]
	public IReadOnlyList<string> ServerClasses { get; init; } = [];

	/// <summary>The server class the app was last mapped to.</summary>
	[JsonPropertyName("serverclass")]
	public string? ServerClass { get; init; }

	/// <summary>The size of the compressed bundle, in bytes.</summary>
	[JsonPropertyName("size")]
	public long? Size { get; init; }

	/// <summary>Whether matching continues through later server classes after the first match (<c>true</c>, the default) or stops there.</summary>
	[JsonPropertyName("continueMatching")]
	public bool? ContinueMatching { get; init; }

	/// <summary>Whether whitelist or blacklist filters are applied first.</summary>
	[JsonPropertyName("filterType")]
	public DeploymentFilterType? FilterType { get; init; }

	/// <summary>A comma-separated list of machine type patterns (PCRE, with <c>.</c> and <c>*</c> shortcuts) that clients must also match.</summary>
	[JsonPropertyName("machineTypesFilter")]
	public string? MachineTypesFilter { get; init; }

	/// <summary>Where the deployment server keeps the content to deploy, for example <c>$SPLUNK_HOME/etc/deployment-apps</c>.</summary>
	[JsonPropertyName("repositoryLocation")]
	public string? RepositoryLocation { get; init; }

	/// <summary>Whether clients restart Splunk Web when an app changes.</summary>
	[JsonPropertyName("restartSplunkWeb")]
	public bool? RestartSplunkWeb { get; init; }

	/// <summary>Whether clients restart splunkd when an app changes.</summary>
	[JsonPropertyName("restartSplunkd")]
	public bool? RestartSplunkd { get; init; }

	/// <summary>The state apps get on clients.</summary>
	[JsonPropertyName("stateOnClient")]
	public DeploymentAppState? StateOnClient { get; init; }

	/// <summary>Where clients install the apps; defaults to <c>$SPLUNK_HOME/etc/apps</c>.</summary>
	[JsonPropertyName("targetRepositoryLocation")]
	public string? TargetRepositoryLocation { get; init; }

	/// <summary>The working folder of the deployment server.</summary>
	[JsonPropertyName("tmpFolder")]
	public string? TmpFolder { get; init; }
}
