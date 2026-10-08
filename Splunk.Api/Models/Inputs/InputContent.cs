using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Inputs;

/// <summary>The properties most data inputs share: where their events go and how they are labelled.</summary>
public abstract class InputContent : SplunkContent
{
	/// <summary>The host field given to events, for example <c>$decideOnStartup</c> (the server's own name).</summary>
	[JsonPropertyName("host")]
	public string? Host { get; init; }

	/// <summary>The host name <see cref="Host"/> resolved to (<c>host_resolved</c>).</summary>
	[JsonPropertyName("host_resolved")]
	public string? HostResolved { get; init; }

	/// <summary>The index events are stored in; <c>default</c> means the server's default index.</summary>
	[JsonPropertyName("index")]
	public string? Index { get; init; }

	/// <summary>The source field given to events, when overridden.</summary>
	[JsonPropertyName("source")]
	public string? Source { get; init; }

	/// <summary>The sourcetype given to events.</summary>
	[JsonPropertyName("sourcetype")]
	public string? Sourcetype { get; init; }

	/// <summary>The socket receive buffer size in bytes (<c>_rcvbuf</c>).</summary>
	[JsonPropertyName("_rcvbuf")]
	public long? ReceiveBufferSize { get; init; }
}
