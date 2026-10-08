using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Server;

/// <summary>Changes a logging category's level (<c>POST server/logger/{name}</c>). The change lasts until splunkd restarts.</summary>
public sealed class LoggerUpdateRequest : SplunkFormRequest
{
	/// <summary>The new level (<c>level</c>).</summary>
	[JsonPropertyName("level")]
	public required SplunkLogLevel Level { get; init; }
}
