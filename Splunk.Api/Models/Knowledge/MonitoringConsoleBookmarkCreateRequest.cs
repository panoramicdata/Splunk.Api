using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Knowledge;

/// <summary>Adds a bookmark to another deployment's monitoring console (<c>POST saved/bookmarks/monitoring_console</c>).</summary>
public sealed class MonitoringConsoleBookmarkCreateRequest : SplunkFormRequest
{
	/// <summary>The bookmark name.</summary>
	[JsonPropertyName("name")]
	public required string Name { get; init; }

	/// <summary>The full URL of the other deployment's monitoring console, for example <c>https://host:8000/en-US/app/splunk_monitoring_console</c>.</summary>
	[JsonPropertyName("url")]
	public required string Url { get; init; }
}
