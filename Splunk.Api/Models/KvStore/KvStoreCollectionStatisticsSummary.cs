using System.Text.Json.Serialization;

namespace Splunk.Api.Models.KvStore;

/// <summary>The numbers in <see cref="KvStoreCollectionStatistics"/>.</summary>
public sealed class KvStoreCollectionStatisticsSummary
{
	/// <summary>The number of documents (<c>record_count</c>).</summary>
	[JsonPropertyName("record_count")]
	public long RecordCount { get; init; }

	/// <summary>The estimated storage size in bytes (<c>estimated_size_bytes</c>).</summary>
	[JsonPropertyName("estimated_size_bytes")]
	public long EstimatedSizeBytes { get; init; }

	/// <summary>The number of indexes, including accelerations (<c>index_count</c>).</summary>
	[JsonPropertyName("index_count")]
	public int IndexCount { get; init; }

	/// <summary>When the collection was last changed (<c>last_updated</c>).</summary>
	[JsonPropertyName("last_updated")]
	public DateTimeOffset? LastUpdated { get; init; }
}
