using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Inputs;

/// <summary>
/// A raw or cooked TCP input (<c>data/inputs/tcp/raw</c>, <c>data/inputs/tcp/cooked</c>). The entry name is the port, or
/// <c>host:port</c> when the input only accepts one host.
/// </summary>
public sealed class TcpInput : InputContent
{
	/// <summary>The input status group, <c>listenerports</c>.</summary>
	[JsonPropertyName("group")]
	public string? Group { get; init; }

	/// <summary>How the host field is set from the sender (<c>connection_host</c>).</summary>
	[JsonPropertyName("connection_host")]
	public ConnectionHost? ConnectionHost { get; init; }

	/// <summary>Where events are queued: <c>parsingQueue</c> or <c>indexQueue</c>.</summary>
	[JsonPropertyName("queue")]
	public string? Queue { get; init; }

	/// <summary>Seconds of idleness after which the last event is considered complete (<c>rawTcpDoneTimeout</c>).</summary>
	[JsonPropertyName("rawTcpDoneTimeout")]
	public int? RawTcpDoneTimeout { get; init; }

	/// <summary>The only host the input accepts connections from (<c>restrictToHost</c>).</summary>
	[JsonPropertyName("restrictToHost")]
	public string? RestrictToHost { get; init; }

	/// <summary>Whether the input uses SSL (<c>SSL</c>).</summary>
	[JsonPropertyName("SSL")]
	public bool? Ssl { get; init; }
}
