using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Server;

/// <summary>A splunkd logging category (<c>server/logger</c>).</summary>
public sealed class LoggerCategory : SplunkContent
{
	/// <summary>The category's logging level.</summary>
	[JsonPropertyName("level")]
	public SplunkLogLevel Level { get; init; }

	/// <summary>Whether the category buffers its output.</summary>
	[JsonPropertyName("buffering")]
	public bool? Buffering { get; init; }
}
