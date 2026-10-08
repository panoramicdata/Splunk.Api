using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Applications;

/// <summary>Changes an app's properties (<c>POST apps/local/{name}</c>).</summary>
public sealed class AppUpdateRequest : AppSettings
{
	/// <summary>Whether Splunk checks Splunkbase for updates to the app.</summary>
	[JsonPropertyName("check_for_updates")]
	public bool? CheckForUpdates { get; init; }
}
