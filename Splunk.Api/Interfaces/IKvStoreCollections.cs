using Refit;
using Splunk.Api.Models;
using Splunk.Api.Models.KvStore;

namespace Splunk.Api.Interfaces;

/// <summary>
/// KV store collection definitions (<c>storage/collections/config</c>).
/// </summary>
/// <remarks>
/// Collections belong to an app and must be addressed through the <c>nobody</c> namespace:
/// <c>client.InNamespace("nobody", "search").KvStoreCollections</c>. Splunk rejects the global <c>services/</c> context
/// and user namespaces ("Must use user context of 'nobody'").
/// </remarks>
public interface IKvStoreCollections
{
	/// <summary>Lists the collections (<c>GET storage/collections/config</c>).</summary>
	/// <param name="options">Paging, filtering and sorting, or <see langword="null"/> for Splunk's defaults.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with one entry per collection.</returns>
	[Get("services/storage/collections/config")]
	Task<SplunkFeed<KvStoreCollectionDefinition>> ListAsync([Query] ListOptions? options, CancellationToken cancellationToken);

	/// <summary>Creates a collection (<c>POST storage/collections/config</c>).</summary>
	/// <param name="request">The name, settings, field types and accelerations.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with the new collection.</returns>
	[Post("services/storage/collections/config")]
	Task<SplunkFeed<KvStoreCollectionDefinition>> CreateAsync([Body] KvStoreCollectionCreateRequest request, CancellationToken cancellationToken);

	/// <summary>Gets a collection (<c>GET storage/collections/config/{collection}</c>).</summary>
	/// <param name="collection">The collection name.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with the collection.</returns>
	[Get("services/storage/collections/config/{collection}")]
	Task<SplunkFeed<KvStoreCollectionDefinition>> GetAsync(string collection, CancellationToken cancellationToken);

	/// <summary>Changes a collection's settings, field types or accelerations (<c>POST storage/collections/config/{collection}</c>).</summary>
	/// <param name="collection">The collection name.</param>
	/// <param name="request">What to change.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with the updated collection.</returns>
	[Post("services/storage/collections/config/{collection}")]
	Task<SplunkFeed<KvStoreCollectionDefinition>> UpdateAsync(string collection, [Body] KvStoreCollectionUpdateRequest request, CancellationToken cancellationToken);

	/// <summary>Deletes a collection and all its documents (<c>DELETE storage/collections/config/{collection}</c>).</summary>
	/// <param name="collection">The collection name.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A task that completes when the collection is deleted.</returns>
	[Delete("services/storage/collections/config/{collection}")]
	Task DeleteAsync(string collection, CancellationToken cancellationToken);
}
