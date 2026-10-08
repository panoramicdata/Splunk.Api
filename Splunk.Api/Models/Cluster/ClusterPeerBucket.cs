using Splunk.Api.Serialization;
using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Cluster;

/// <summary>A bucket on a peer node (<c>cluster/peer/buckets</c>).</summary>
public sealed class ClusterPeerBucket : SplunkContent
{
	/// <summary>Used internally to identify the bucket.</summary>
	[JsonPropertyName("checksum")]
	public string? Checksum { get; init; }

	/// <summary>The time of the earliest event.</summary>
	[JsonPropertyName("earliest_time")]
	[JsonConverter(typeof(EpochSecondsConverter))]
	public DateTimeOffset? EarliestTime { get; init; }

	/// <summary>The time of the latest event.</summary>
	[JsonPropertyName("latest_time")]
	[JsonConverter(typeof(EpochSecondsConverter))]
	public DateTimeOffset? LatestTime { get; init; }

	/// <summary>The generation ID, where reported.</summary>
	[JsonPropertyName("generation_id")]
	public long? GenerationId { get; init; }

	/// <summary>The bucket's primacy mask, by generation ID.</summary>
	[JsonPropertyName("generations")]
	public IReadOnlyDictionary<string, string> Generations { get; init; } = new Dictionary<string, string>();

	/// <summary><c>Searchable</c> or <c>Unsearchable</c>.</summary>
	[JsonPropertyName("search_state")]
	public string? SearchState { get; init; }

	/// <summary>The copy status, for example <c>Complete</c>, <c>StreamingSource</c> or <c>StreamingTarget</c>.</summary>
	[JsonPropertyName("status")]
	public string? Status { get; init; }
}
