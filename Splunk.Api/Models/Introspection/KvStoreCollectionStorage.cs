using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Introspection;

/// <summary>The storage used by one KV store collection (an item of <see cref="KvStoreCollectionStats.Collections"/>).</summary>
public sealed class KvStoreCollectionStorage
{
	/// <summary>The collection, as <c>app.collection</c> (<c>collection</c>).</summary>
	[JsonPropertyName("collection")]
	public string? Collection { get; init; }

	/// <summary>The namespace, as <c>app.collection</c> (<c>ns</c>).</summary>
	[JsonPropertyName("ns")]
	public string? Namespace { get; init; }

	/// <summary>The number of documents (<c>count</c>).</summary>
	[JsonPropertyName("count")]
	public long Count { get; init; }

	/// <summary>The data size in bytes (<c>size</c>).</summary>
	[JsonPropertyName("size")]
	public long Size { get; init; }

	/// <summary>The storage allocated in bytes (<c>storageSize</c>).</summary>
	[JsonPropertyName("storageSize")]
	public long StorageSize { get; init; }

	/// <summary>The size of every index in bytes (<c>totalIndexSize</c>).</summary>
	[JsonPropertyName("totalIndexSize")]
	public long TotalIndexSize { get; init; }

	/// <summary>The number of indexes (<c>nindexes</c>).</summary>
	[JsonPropertyName("nindexes")]
	public int IndexCount { get; init; }

	/// <summary>Each index's size in bytes, keyed by index name (<c>indexSizes</c>).</summary>
	[JsonPropertyName("indexSizes")]
	public IReadOnlyDictionary<string, long> IndexSizes { get; init; } = new Dictionary<string, long>();

	/// <summary>The backing store, for example <c>external</c> (<c>kvstoreType</c>).</summary>
	[JsonPropertyName("kvstoreType")]
	public string? KvStoreType { get; init; }

	/// <summary>When the collection was last changed, often empty (<c>lastModifiedTime</c>).</summary>
	[JsonPropertyName("lastModifiedTime")]
	public string? LastModifiedTime { get; init; }
}
