using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Introspection;

/// <summary>Adds a monitoring console bookmark (<c>POST saved/bookmarks/monitoring_console</c>).</summary>
public sealed class MonitoringConsoleBookmarkCreateRequest : SplunkFormRequest
{
	/// <summary>The bookmark name, at most 25 characters (<c>name</c>).</summary>
	[JsonPropertyName("name")]
	public required string Name { get; init; }

	/// <summary>The monitoring console URL; Splunk requires it to contain <c>splunk_monitoring_console</c> (<c>url</c>).</summary>
	[JsonPropertyName("url")]
	public required string Url { get; init; }
}
