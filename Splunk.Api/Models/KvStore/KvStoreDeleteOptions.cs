using Refit;

namespace Splunk.Api.Models.KvStore;

/// <summary>Which documents to delete (<c>DELETE storage/collections/data/{collection}</c>).</summary>
public sealed class KvStoreDeleteOptions
{
	/// <summary>
	/// A filter as a JSON object (<c>query</c>), as in <see cref="KvStoreQuery.Query"/>. <see langword="null"/> deletes every
	/// document in the collection.
	/// </summary>
	[AliasAs("query")]
	public string? Query { get; init; }
}
