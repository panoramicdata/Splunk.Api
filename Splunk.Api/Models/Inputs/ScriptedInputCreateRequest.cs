using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Inputs;

/// <summary>
/// Creates a scripted input (<c>POST data/inputs/script</c>). The script must exist on the Splunk server, in
/// <c>$SPLUNK_HOME/bin/scripts</c> or an app's <c>bin</c> directory.
/// </summary>
public sealed class ScriptedInputCreateRequest : SplunkFormRequest
{
	/// <summary>The script's path, for example <c>$SPLUNK_HOME/etc/apps/search/bin/myscript.sh</c>.</summary>
	[JsonPropertyName("name")]
	public required string Name { get; init; }

	/// <summary>How often the script runs: seconds, or a cron schedule.</summary>
	[JsonPropertyName("interval")]
	public required string Interval { get; init; }

	/// <summary>Whether the input is disabled.</summary>
	[JsonPropertyName("disabled")]
	public bool? Disabled { get; init; }

	/// <summary>The host field for events.</summary>
	[JsonPropertyName("host")]
	public string? Host { get; init; }

	/// <summary>The index events are stored in.</summary>
	[JsonPropertyName("index")]
	public string? Index { get; init; }

	/// <summary>The user the script runs as (<c>passAuth</c>).</summary>
	[JsonPropertyName("passAuth")]
	public string? PassAuth { get; init; }

	/// <summary>The source field for events (<c>rename-source</c>).</summary>
	[JsonPropertyName("rename-source")]
	public string? RenameSource { get; init; }

	/// <summary>The source key for events; overriding it is rarely advisable.</summary>
	[JsonPropertyName("source")]
	public string? Source { get; init; }

	/// <summary>The sourcetype for events.</summary>
	[JsonPropertyName("sourcetype")]
	public string? Sourcetype { get; init; }
}
