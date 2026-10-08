using Splunk.Api.Serialization;
using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Inputs;

/// <summary>A recent HTTP Event Collector sender (<c>data/inputs/http/connections</c>). The entry name is its IP address.</summary>
public sealed class HecConnection : SplunkContent
{
	/// <summary>The sender's IP address.</summary>
	[JsonPropertyName("ip_address")]
	public string? IpAddress { get; init; }

	/// <summary>When the sender's last event arrived (<c>last_conn_time</c>).</summary>
	[JsonPropertyName("last_conn_time")]
	[JsonConverter(typeof(EpochSecondsConverter))]
	public DateTimeOffset? LastConnectionTime { get; init; }
}
