using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Outputs;

/// <summary>Changes a syslog forwarding group (<c>POST data/outputs/tcp/syslog/{name}</c>).</summary>
/// <remarks>Splunk 10.6 clears <c>server</c> on an update that does not set it.</remarks>
public class SyslogOutputUpdateRequest : SplunkFormRequest
{
	/// <summary>Whether the group is disabled.</summary>
	[JsonPropertyName("disabled")]
	public bool? Disabled { get; init; }

	/// <summary>The syslog priority value.</summary>
	[JsonPropertyName("priority")]
	public int? Priority { get; init; }

	/// <summary>The syslog server as <c>host:port</c>.</summary>
	[JsonPropertyName("server")]
	public string? Server { get; init; }

	/// <summary>The sourcetype rule for data handled as syslog, for example <c>sourcetype::apache_common</c> (<c>syslogSourceType</c>).</summary>
	[JsonPropertyName("syslogSourceType")]
	public string? SyslogSourceType { get; init; }

	/// <summary>The strftime format of the timestamp added to events (<c>timestampformat</c>).</summary>
	[JsonPropertyName("timestampformat")]
	public string? TimestampFormat { get; init; }

	/// <summary>The protocol: <c>tcp</c> or <c>udp</c> (the default).</summary>
	[JsonPropertyName("type")]
	public string? Type { get; init; }
}
