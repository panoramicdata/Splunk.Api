using Refit;

namespace Splunk.Api.Models.KvStore;

/// <summary>
/// What to read from a KV store collection (<c>GET storage/collections/data/{collection}</c>). Leave a property
/// <see langword="null"/> for Splunk's default.
/// </summary>
public sealed class KvStoreQuery
{
	/// <summary>
	/// A filter as a JSON object (<c>query</c>), for example <c>{"status": "open"}</c> or
	/// <c>{"$or": [{"n": {"$gt": 1}}, {"name": "a"}]}</c>. Operators: <c>$gt</c>, <c>$gte</c>, <c>$lt</c>, <c>$lte</c>,
	/// <c>$ne</c>, <c>$and</c>, <c>$or</c>, <c>$not</c>.
	/// </summary>
	[AliasAs("query")]
	public string? Query { get; init; }

	/// <summary>The fields to return (<c>fields</c>, comma-separated); <c>name:0</c> excludes a field.</summary>
	[AliasAs("fields")]
	[Query(CollectionFormat.Csv)]
	public IEnumerable<string>? Fields { get; init; }

	/// <summary>The sort order (<c>sort</c>), for example <c>n:-1,name</c> (<c>1</c> ascending, <c>-1</c> descending).</summary>
	[AliasAs("sort")]
	public string? Sort { get; init; }

	/// <summary>The maximum number of documents to return (<c>limit</c>).</summary>
	[AliasAs("limit")]
	public int? Limit { get; init; }

	/// <summary>The number of documents to skip (<c>skip</c>).</summary>
	[AliasAs("skip")]
	public int? Skip { get; init; }

	/// <summary>Whether to include documents of user <c>nobody</c> when reading in a user's namespace (<c>shared</c>).</summary>
	[AliasAs("shared")]
	public bool? Shared { get; init; }
}
