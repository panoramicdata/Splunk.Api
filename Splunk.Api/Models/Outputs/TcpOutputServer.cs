using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Outputs;

/// <summary>A receiver this server forwards to, a <c>[tcpout-server://{host:port}]</c> stanza (<c>data/outputs/tcp/server</c>).</summary>
public sealed class TcpOutputServer : SplunkContent
{
	/// <summary>The receiver's host name (<c>destHost</c>).</summary>
	[JsonPropertyName("destHost")]
	public string? DestinationHost { get; init; }

	/// <summary>The receiver's IP address (<c>destIp</c>).</summary>
	[JsonPropertyName("destIp")]
	public string? DestinationIp { get; init; }

	/// <summary>The receiver's port (<c>destPort</c>).</summary>
	[JsonPropertyName("destPort")]
	public int? DestinationPort { get; init; }

	/// <summary>The local port data is sent from (<c>sourcePort</c>).</summary>
	[JsonPropertyName("sourcePort")]
	public int? SourcePort { get; init; }

	/// <summary>The connection status, for example <c>connect_done</c> or <c>not_connected</c>.</summary>
	[JsonPropertyName("status")]
	public string? Status { get; init; }

	/// <summary>How data is distributed in the receiver's group, for example <c>autobalance</c> or <c>clone</c>.</summary>
	[JsonPropertyName("method")]
	public string? Method { get; init; }

	/// <summary>Whether the receiver's certificate is verified (<c>sslVerifyServerCert</c>).</summary>
	[JsonPropertyName("sslVerifyServerCert")]
	public bool? SslVerifyServerCert { get; init; }
}
