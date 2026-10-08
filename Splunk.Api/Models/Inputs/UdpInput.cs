using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Inputs;

/// <summary>A UDP input (<c>data/inputs/udp</c>). The entry name is the port, or <c>host:port</c>.</summary>
public sealed class UdpInput : InputContent
{
	/// <summary>The input status group, <c>listenerports</c>.</summary>
	[JsonPropertyName("group")]
	public string? Group { get; init; }

	/// <summary>How the host field is set from the sender (<c>connection_host</c>).</summary>
	[JsonPropertyName("connection_host")]
	public ConnectionHost? ConnectionHost { get; init; }

	/// <summary>Whether Splunk does not prepend a timestamp and host to events (<c>no_appending_timestamp</c>).</summary>
	[JsonPropertyName("no_appending_timestamp")]
	public bool? NoAppendingTimestamp { get; init; }

	/// <summary>Whether Splunk keeps the syslog priority field (<c>no_priority_stripping</c>).</summary>
	[JsonPropertyName("no_priority_stripping")]
	public bool? NoPriorityStripping { get; init; }

	/// <summary>The queue events go to.</summary>
	[JsonPropertyName("queue")]
	public string? Queue { get; init; }

	/// <summary>The only host the input accepts data from (<c>restrictToHost</c>).</summary>
	[JsonPropertyName("restrictToHost")]
	public string? RestrictToHost { get; init; }
}
