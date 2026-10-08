using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Introspection;

/// <summary>The indexer's current state (<c>server/introspection/indexer</c>).</summary>
public sealed class IndexerStatus : SplunkContent
{
	/// <summary>The indexing state, for example <c>normal</c> or <c>throttled</c> (<c>status</c>).</summary>
	[JsonPropertyName("status")]
	public string? Status { get; init; }

	/// <summary>Why indexing is not normal, if it is not (<c>reason</c>).</summary>
	[JsonPropertyName("reason")]
	public string? Reason { get; init; }

	/// <summary>The average indexing throughput in kilobytes per second (<c>average_KBps</c>).</summary>
	[JsonPropertyName("average_KBps")]
	public double AverageKBps { get; init; }
}
