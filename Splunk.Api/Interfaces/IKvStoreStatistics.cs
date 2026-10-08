using Refit;
using Splunk.Api.Models.KvStore;

namespace Splunk.Api.Interfaces;

/// <summary>
/// KV store collection statistics (<c>storage/collections/stats</c>). Needs the <c>kvstore_collection_stats_read</c>
/// capability and a namespace (<c>client.InNamespace("nobody", app)</c>): the global <c>services/</c> context is rejected.
/// </summary>
public interface IKvStoreStatistics
{
	/// <summary>Gets statistics for a whole collection (<c>GET storage/collections/stats</c>).</summary>
	/// <param name="collection">The collection name.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>The statistics. An unknown collection raises <see cref="SplunkApiException"/> with status 403.</returns>
	[Get("services/storage/collections/stats")]
	Task<KvStoreCollectionStatistics> GetCollectionAsync([AliasAs("collection")] string collection, CancellationToken cancellationToken);

	/// <summary>Gets statistics for the documents that match a filter (<c>GET storage/collections/stats/fields</c>).</summary>
	/// <param name="options">The collection, filter, fields and aggregations.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>The statistics.</returns>
	[Get("services/storage/collections/stats/fields")]
	Task<KvStoreFieldStatistics> GetFieldsAsync([Query] KvStoreFieldStatisticsOptions options, CancellationToken cancellationToken);
}
