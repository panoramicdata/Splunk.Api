using System.Text.Json;
using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Introspection;

/// <summary>Storage statistics for every KV store collection (<c>server/introspection/kvstore/collectionstats</c>).</summary>
public sealed class KvStoreCollectionStats : SplunkContent
{
	/// <summary>The raw statistics: Splunk returns one JSON document per collection, each as a string (<c>data</c>).</summary>
	[JsonPropertyName("data")]
	public IReadOnlyList<string> Data { get; init; } = [];

	/// <summary>The statistics parsed from <see cref="Data"/>, one per collection.</summary>
	[JsonIgnore]
	public IReadOnlyList<KvStoreCollectionStorage> Collections
		=> [.. Data.Select(d => JsonSerializer.Deserialize<KvStoreCollectionStorage>(d, SplunkJson.Options)!)];
}
