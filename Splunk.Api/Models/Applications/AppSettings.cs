using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Applications;

/// <summary>The properties of an app, shared by <see cref="AppCreateRequest"/> and <see cref="AppUpdateRequest"/>.</summary>
public abstract class AppSettings : SplunkFormRequest
{
	/// <summary>The label shown in Splunk Web: 5 to 80 characters, excluding a "Splunk For" prefix.</summary>
	[JsonPropertyName("label")]
	public string? Label { get; init; }

	/// <summary>A short description, shown below the app's title in Splunk Web.</summary>
	[JsonPropertyName("description")]
	public string? Description { get; init; }

	/// <summary>The author: a Splunkbase user name, or a name and contact details.</summary>
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
}
