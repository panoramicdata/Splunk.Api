using System.Text.Json.Serialization;

namespace Splunk.Api.Models.KvStore;

/// <summary>Statistics for a whole KV store collection (<c>storage/collections/stats</c>). This is plain JSON, not a feed.</summary>
public sealed class KvStoreCollectionStatistics
{
	/// <summary>The collection name.</summary>
	[JsonPropertyName("collection")]
	public string? Collection { get; init; }

	/// <summary>The app the collection belongs to.</summary>
	[JsonPropertyName("app")]
	public string? App { get; init; }

	/// <summary>The statistics.</summary>
	[JsonPropertyName("stats")]
	public KvStoreCollectionStatisticsSummary? Stats { get; init; }
}
