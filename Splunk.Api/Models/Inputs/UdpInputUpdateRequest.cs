using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Inputs;

/// <summary>Changes a UDP input (<c>POST data/inputs/udp/{name}</c>). Unset properties are left unchanged.</summary>
public class UdpInputUpdateRequest : SplunkFormRequest
{
	/// <summary>How the host field is set from the sender (<c>connection_host</c>).</summary>
	[JsonPropertyName("connection_host")]
	public ConnectionHost? ConnectionHost { get; init; }

	/// <summary>Whether the input is disabled.</summary>
	[JsonPropertyName("disabled")]
	public bool? Disabled { get; init; }

	/// <summary>The host field for events.</summary>
	[JsonPropertyName("host")]
	public string? Host { get; init; }

	/// <summary>The index events are stored in.</summary>
	[JsonPropertyName("index")]
	public string? Index { get; init; }

	/// <summary>Whether Splunk does not prepend a timestamp and host to events (<c>no_appending_timestamp</c>).</summary>
	[JsonPropertyName("no_appending_timestamp")]
	public bool? NoAppendingTimestamp { get; init; }

	/// <summary>Whether Splunk keeps the syslog priority field (<c>no_priority_stripping</c>).</summary>
	[JsonPropertyName("no_priority_stripping")]
	public bool? NoPriorityStripping { get; init; }

	/// <summary>The queue events go to; rarely changed.</summary>
	[JsonPropertyName("queue")]
	public string? Queue { get; init; }

	/// <summary>The only host the input accepts data from (<c>restrictToHost</c>).</summary>
	[JsonPropertyName("restrictToHost")]
	public string? RestrictToHost { get; init; }

	/// <summary>The source field for events.</summary>
	[JsonPropertyName("source")]
	public string? Source { get; init; }

	/// <summary>The sourcetype for events.</summary>
	[JsonPropertyName("sourcetype")]
	public string? Sourcetype { get; init; }
}
