using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Inputs;

/// <summary>Splunk's reply to events sent through <c>receivers/simple</c>: what it received and where the events went.</summary>
public sealed class ReceiverResult
{
	/// <summary>The index the events went to.</summary>
	[JsonPropertyName("index")]
	public string? Index { get; init; }

	/// <summary>The number of bytes received.</summary>
	[JsonPropertyName("bytes")]
	public long Bytes { get; init; }

	/// <summary>The host field given to the events.</summary>
	[JsonPropertyName("host")]
	public string? Host { get; init; }

	/// <summary>The source field given to the events.</summary>
	[JsonPropertyName("source")]
	public string? Source { get; init; }

	/// <summary>The sourcetype given to the events.</summary>
	[JsonPropertyName("sourcetype")]
	public string? Sourcetype { get; init; }
}
