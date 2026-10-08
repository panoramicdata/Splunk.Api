using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Applications;

/// <summary>An installed app (<c>apps/local</c>); the entry name is the app's folder name.</summary>
public sealed class App : SplunkContent
{
	/// <summary>The label shown in Splunk Web.</summary>
	[JsonPropertyName("label")]
	public string? Label { get; init; }

	/// <summary>The app's description.</summary>
	[JsonPropertyName("description")]
	public string? Description { get; init; }

	/// <summary>The app's author: a Splunkbase user name, or a name and contact details.</summary>
	[JsonPropertyName("author")]
	public string? Author { get; init; }

	/// <summary>The app's version.</summary>
	[JsonPropertyName("version")]
	public string? Version { get; init; }

	/// <summary>Whether the app is visible and navigable in Splunk Web.</summary>
	[JsonPropertyName("visible")]
	public bool? Visible { get; init; }

	/// <summary>Whether the app's custom setup is complete.</summary>
	[JsonPropertyName("configured")]
	public bool? Configured { get; init; }

	/// <summary>Whether Splunk checks Splunkbase for updates to the app.</summary>
	[JsonPropertyName("check_for_updates")]
	public bool? CheckForUpdates { get; init; }

	/// <summary>Whether the app ships with Splunk.</summary>
	[JsonPropertyName("core")]
	public bool? Core { get; init; }

	/// <summary>Whether the app appears in the app navigation.</summary>
	[JsonPropertyName("show_in_nav")]
	public bool? ShowInNav { get; init; }

	/// <summary>Whether enabling or disabling the app requires a restart.</summary>
	[JsonPropertyName("state_change_requires_restart")]
	public bool? StateChangeRequiresRestart { get; init; }

	/// <summary>Whether a deployment server manages the app.</summary>
	[JsonPropertyName("managed_by_deployment_client")]
	public bool? ManagedByDeploymentClient { get; init; }

	/// <summary>The Splunk Web themes the app supports, separated by commas, for example <c>light,dark</c>.</summary>
	[JsonPropertyName("supported_themes")]
	public string? SupportedThemes { get; init; }

	/// <summary>A URL with details about the app.</summary>
	[JsonPropertyName("details")]
	public string? Details { get; init; }

	/// <summary>The installed app's name, returned on create; it can differ from the name requested.</summary>
	[JsonPropertyName("name")]
	public string? Name { get; init; }
}
