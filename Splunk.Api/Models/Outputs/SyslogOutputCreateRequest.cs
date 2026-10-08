using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Outputs;

/// <summary>Creates a syslog forwarding group (<c>POST data/outputs/tcp/syslog</c>).</summary>
public sealed class SyslogOutputCreateRequest : SyslogOutputUpdateRequest
{
	/// <summary>The group's name.</summary>
	[JsonPropertyName("name")]
	public required string Name { get; init; }
}
