using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Introspection;

/// <summary>A link to the monitoring console of another deployment (<c>saved/bookmarks/monitoring_console</c>).</summary>
public sealed class MonitoringConsoleBookmark : SplunkContent
{
	/// <summary>The monitoring console URL (<c>url</c>).</summary>
	[JsonPropertyName("url")]
	public string? Url { get; init; }
}
