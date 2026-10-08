using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Inputs;

/// <summary>A scripted input (<c>data/inputs/script</c>). The entry name is the script's path, with any arguments.</summary>
public sealed class ScriptedInput : InputContent
{
	/// <summary>How often the script runs: seconds, or a cron schedule such as <c>0 */4 * * *</c>.</summary>
	[JsonPropertyName("interval")]
	public string? Interval { get; init; }

	/// <summary>The user the script runs as; Splunk passes it an authentication token for that user (<c>passAuth</c>).</summary>
	[JsonPropertyName("passAuth")]
	public string? PassAuth { get; init; }

	/// <summary>The input status group, <c>exec commands</c>.</summary>
	[JsonPropertyName("group")]
	public string? Group { get; init; }

	/// <summary>When the script last started, where Splunk reports it.</summary>
	[JsonPropertyName("starttime")]
	public string? StartTime { get; init; }

	/// <summary>When the script last stopped, where Splunk reports it.</summary>
	[JsonPropertyName("endtime")]
	public string? EndTime { get; init; }
}
