using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Knowledge;

/// <summary>A bookmark to another deployment's monitoring console (<c>saved/bookmarks/monitoring_console</c>).</summary>
public sealed class MonitoringConsoleBookmark : SplunkContent
{
	/// <summary>The URL of the other deployment's monitoring console.</summary>
	[JsonPropertyName("url")]
	public string? Url { get; init; }
}
