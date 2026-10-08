using System.Text.Json;
using System.Text.Json.Serialization;

namespace Splunk.Api.Models.KvStore;

/// <summary>
/// Statistics for the documents of a KV store collection that match a filter (<c>storage/collections/stats/fields</c>).
/// This is plain JSON, not a feed.
/// </summary>
public sealed class KvStoreFieldStatistics
{
	/// <summary>The collection name.</summary>
	[JsonPropertyName("collection")]
	public string? Collection { get; init; }

	/// <summary>The filter that was applied, as Splunk echoes it.</summary>
	[JsonPropertyName("filter")]
	public JsonElement? Filter { get; init; }

	/// <summary>The statistics.</summary>
	[JsonPropertyName("stats")]
	public KvStoreFieldStatisticsSummary? Stats { get; init; }
}
