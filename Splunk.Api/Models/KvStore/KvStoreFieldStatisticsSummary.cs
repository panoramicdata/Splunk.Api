using System.Text.Json;
using System.Text.Json.Serialization;

namespace Splunk.Api.Models.KvStore;

/// <summary>The numbers in <see cref="KvStoreFieldStatistics"/>.</summary>
public sealed class KvStoreFieldStatisticsSummary
{
	/// <summary>The number of matching documents (<c>count</c>).</summary>
	[JsonPropertyName("count")]
	public long Count { get; init; }

	/// <summary>
	/// The requested aggregations, keyed by field, each an object keyed by function, for example
	/// <c>{"n": {"count": 3}}</c>; empty when none were requested.
	/// </summary>
	[JsonPropertyName("aggregations")]
	public IReadOnlyDictionary<string, IReadOnlyDictionary<string, JsonElement>> Aggregations { get; init; }
		= new Dictionary<string, IReadOnlyDictionary<string, JsonElement>>();

	/// <summary>How long the query took, in milliseconds (<c>execution_time_ms</c>).</summary>
	[JsonPropertyName("execution_time_ms")]
	public long ExecutionTimeMs { get; init; }

	/// <summary>When the collection was last changed (<c>last_updated</c>).</summary>
	[JsonPropertyName("last_updated")]
	public DateTimeOffset? LastUpdated { get; init; }
}
