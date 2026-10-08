using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Deployment;

/// <summary>Changes to how the deployment server distributes an app (<c>POST deployment/server/applications/{name}</c>).</summary>
/// <remarks>Whitelist and blacklist filters are numbered families (<c>whitelist.0</c>, <c>whitelist.1</c>, <c>blacklist.0</c>, ...; ordinals start at 0 and are consecutive): set them in <see cref="SplunkFormRequest.AdditionalParameters"/>.</remarks>
public sealed class DeploymentApplicationUpdateRequest : SplunkFormRequest
{
	/// <summary>The server class to map the app to; leave unset when <see cref="Deinstall"/> is <see langword="true"/>.</summary>
	[JsonPropertyName("serverclass")]
	public string? ServerClass { get; init; }

	/// <summary>Whether to remove the app from every server class and from the clients.</summary>
	[JsonPropertyName("deinstall")]
	public bool? Deinstall { get; init; }

	/// <summary>Whether to remove the mapping of the app to <see cref="ServerClass"/>.</summary>
	[JsonPropertyName("unmap")]
	public bool? Unmap { get; init; }

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
