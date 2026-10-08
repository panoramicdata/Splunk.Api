using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Inputs;

/// <summary>Changes a monitor input (<c>POST data/inputs/monitor/{name}</c>). Unset properties are left unchanged.</summary>
public class MonitorInputUpdateRequest : SplunkFormRequest
{
	/// <summary>A regular expression for paths not to index.</summary>
	[JsonPropertyName("blacklist")]
	public string? Blacklist { get; init; }

	/// <summary>When <see langword="true"/>, Splunk checks that <see cref="Index"/> names an existing index (<c>check-index</c>).</summary>
	[JsonPropertyName("check-index")]
	public bool? CheckIndex { get; init; }

	/// <summary>When <see langword="true"/>, Splunk checks that the path exists (<c>check-path</c>).</summary>
	[JsonPropertyName("check-path")]
	public bool? CheckPath { get; init; }

	/// <summary>A string that changes how files are recognised (<c>crc-salt</c>); <c>&lt;SOURCE&gt;</c> salts with the path.</summary>
	[JsonPropertyName("crc-salt")]
	public string? CrcSalt { get; init; }

	/// <summary>Whether the input is disabled.</summary>
	[JsonPropertyName("disabled")]
	public bool? Disabled { get; init; }

	/// <summary>Whether new files are read from their end (<c>followTail</c>).</summary>
	[JsonPropertyName("followTail")]
	public bool? FollowTail { get; init; }

	/// <summary>The host field for events.</summary>
	[JsonPropertyName("host")]
	public string? Host { get; init; }

	/// <summary>A regular expression with one capture group that sets the host field from the path (<c>host_regex</c>).</summary>
	[JsonPropertyName("host_regex")]
	public string? HostRegex { get; init; }

	/// <summary>The slash-separated path segment that sets the host field (<c>host_segment</c>).</summary>
	[JsonPropertyName("host_segment")]
	public int? HostSegment { get; init; }

	/// <summary>Files not modified within this window are ignored (<c>ignore-older-than</c>), for example <c>7d</c>.</summary>
	[JsonPropertyName("ignore-older-than")]
	public string? IgnoreOlderThan { get; init; }

	/// <summary>The index events are stored in.</summary>
	[JsonPropertyName("index")]
	public string? Index { get; init; }

	/// <summary>Whether subdirectories are monitored.</summary>
	[JsonPropertyName("recursive")]
	public bool? Recursive { get; init; }

	/// <summary>The source field for events (<c>rename-source</c>).</summary>
	[JsonPropertyName("rename-source")]
	public string? RenameSource { get; init; }

	/// <summary>The sourcetype for events.</summary>
	[JsonPropertyName("sourcetype")]
	public string? Sourcetype { get; init; }

	/// <summary>Seconds a file is kept open after its end is reached (<c>time-before-close</c>).</summary>
	[JsonPropertyName("time-before-close")]
	public int? TimeBeforeClose { get; init; }

	/// <summary>A regular expression for the only paths to index.</summary>
	[JsonPropertyName("whitelist")]
	public string? Whitelist { get; init; }
}
