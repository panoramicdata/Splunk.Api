using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Inputs;

/// <summary>A file or directory monitor input (<c>data/inputs/monitor</c>). The entry name is the monitored path.</summary>
/// <remarks>Splunk reports some settings under other names than the ones it accepts, for example <c>crcSalt</c> for <c>crc-salt</c>.</remarks>
public sealed class MonitorInput : InputContent
{
	/// <summary>A regular expression for paths not to index.</summary>
	[JsonPropertyName("blacklist")]
	public string? Blacklist { get; init; }

	/// <summary>A regular expression for the only paths to index.</summary>
	[JsonPropertyName("whitelist")]
	public string? Whitelist { get; init; }

	/// <summary>A string that changes how files are recognised (<c>crcSalt</c>); <c>&lt;SOURCE&gt;</c> salts with the path.</summary>
	[JsonPropertyName("crcSalt")]
	public string? CrcSalt { get; init; }

	/// <summary>Whether new files are read from their end (<c>followTail</c>).</summary>
	[JsonPropertyName("followTail")]
	public bool? FollowTail { get; init; }

	/// <summary>A regular expression whose capture group sets the host field from the path (<c>host_regex</c>).</summary>
	[JsonPropertyName("host_regex")]
	public string? HostRegex { get; init; }

	/// <summary>The path segment that sets the host field (<c>host_segment</c>).</summary>
	[JsonPropertyName("host_segment")]
	public int? HostSegment { get; init; }

	/// <summary>Files not modified within this window are ignored (<c>ignoreOlderThan</c>), for example <c>7d</c>.</summary>
	[JsonPropertyName("ignoreOlderThan")]
	public string? IgnoreOlderThan { get; init; }

	/// <summary>Whether subdirectories are monitored.</summary>
	[JsonPropertyName("recursive")]
	public bool? Recursive { get; init; }

	/// <summary>Seconds a file is kept open after its end is reached (<c>time_before_close</c>).</summary>
	[JsonPropertyName("time_before_close")]
	public int? TimeBeforeClose { get; init; }

	/// <summary>The number of files monitored (<c>filecount</c>).</summary>
	[JsonPropertyName("filecount")]
	public int? FileCount { get; init; }

	/// <summary>The number of files with tracked state (<c>filestatecount</c>).</summary>
	[JsonPropertyName("filestatecount")]
	public int? FileStateCount { get; init; }

	/// <summary>The forwarding groups events are routed to (<c>_TCP_ROUTING</c>); <c>*</c> means all.</summary>
	[JsonPropertyName("_TCP_ROUTING")]
	public string? TcpRouting { get; init; }
}
