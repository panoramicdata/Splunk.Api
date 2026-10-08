using Refit;

namespace Splunk.Api.Models.KvStore;

/// <summary>What to measure with <c>GET storage/collections/stats/fields</c>.</summary>
public sealed class KvStoreFieldStatisticsOptions
{
	/// <summary>The collection (<c>collection</c>).</summary>
	[AliasAs("collection")]
	public required string Collection { get; init; }

	/// <summary>A filter as a JSON object (<c>filter</c>), for example <c>{}</c> for every document.</summary>
	[AliasAs("filter")]
	public required string Filter { get; init; }

	/// <summary>The fields to aggregate (<c>fields</c>, comma-separated).</summary>
	[AliasAs("fields")]
	[Query(CollectionFormat.Csv)]
	public IEnumerable<string>? Fields { get; init; }

	/// <summary>
	/// The aggregation functions (<c>agg</c>, comma-separated), for example <c>count</c>, <c>min</c>, <c>max</c> or
	/// <c>sum</c>. Functions other than <c>count</c> need fields declared as <c>number</c>.
	/// </summary>
	[AliasAs("agg")]
	[Query(CollectionFormat.Csv)]
	public IEnumerable<string>? Aggregations { get; init; }
}
