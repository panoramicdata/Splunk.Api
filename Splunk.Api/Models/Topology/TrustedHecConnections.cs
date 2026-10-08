using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Topology;

/// <summary>HTTP Event Collector details of a node's trusted connections.</summary>
public sealed class TrustedHecConnections
{
	/// <summary>Whether HEC is turned on.</summary>
	[JsonPropertyName("enabled")]
	public bool? Enabled { get; init; }

	/// <summary>The HEC port.</summary>
	[JsonPropertyName("port")]
	public int? Port { get; init; }

	/// <summary>SHA-256 hashes of the HEC tokens.</summary>
	[JsonPropertyName("hashed_tokens")]
	public IReadOnlyList<string> HashedTokens { get; init; } = [];

	/// <summary>The IP addresses that sent data to HEC.</summary>
	[JsonPropertyName("senders")]
	public IReadOnlyList<string> Senders { get; init; } = [];
}
