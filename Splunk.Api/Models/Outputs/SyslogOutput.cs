using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Outputs;

/// <summary>A syslog forwarding group, a <c>[syslog:{name}]</c> stanza (<c>data/outputs/tcp/syslog</c>).</summary>
public sealed class SyslogOutput : SplunkContent
{
	/// <summary>The syslog server as <c>host:port</c>.</summary>
	[JsonPropertyName("server")]
	public string? Server { get; init; }

	/// <summary>The protocol: <c>tcp</c> or <c>udp</c>.</summary>
	[JsonPropertyName("type")]
	public string? Type { get; init; }

	/// <summary>The syslog priority value.</summary>
	[JsonPropertyName("priority")]
	public int? Priority { get; init; }

	/// <summary>The sourcetype rule for data handled as syslog (<c>syslogSourceType</c>).</summary>
	[JsonPropertyName("syslogSourceType")]
	public string? SyslogSourceType { get; init; }

	/// <summary>The strftime format of the timestamp added to events (<c>timestampformat</c>).</summary>
	[JsonPropertyName("timestampformat")]
	public string? TimestampFormat { get; init; }
}
