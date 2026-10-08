using System.Text.Json.Serialization;

namespace Splunk.Api.Models.KvStore;

/// <summary>Creates a KV store collection (<c>POST storage/collections/config</c>).</summary>
public sealed class KvStoreCollectionCreateRequest : KvStoreCollectionSettings
{
	/// <summary>The collection name (<c>name</c>).</summary>
	[JsonPropertyName("name")]
	public required string Name { get; init; }
}
