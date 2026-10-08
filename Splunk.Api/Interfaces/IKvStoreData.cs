using Refit;
using Splunk.Api.Models;
using Splunk.Api.Models.KvStore;

namespace Splunk.Api.Interfaces;

/// <summary>
/// The documents of a KV store collection (<c>storage/collections/data/{collection}</c>), as your own types.
/// </summary>
/// <remarks>
/// <para>
/// Address collections through a namespace: <c>client.InNamespace("nobody", "search").KvStoreData</c> for shared
/// documents, or a user name for that user's documents. Documents are plain JSON (not the feed envelope), serialized and
/// read with <see cref="SplunkJson.Options"/>; derive document types from <see cref="KvStoreDocument"/> to map
/// <c>_key</c> and <c>_user</c>. A record is at most 16 MB. Updates replace the whole document.
/// </para>
/// <example>
/// <code>
/// var data = client.InNamespace("nobody", "search").KvStoreData;
/// var key = await data.InsertAsync("assets", new JsonBody&lt;Asset&gt;(new Asset { Name = "web01" }), ct);
/// var open = await data.QueryAsync&lt;Asset&gt;("assets", new KvStoreQuery { Query = """{"status":"open"}""", Limit = 10 }, ct);
/// </code>
/// </example>
/// </remarks>
public interface IKvStoreData
{
	/// <summary>Reads documents, optionally filtered, sorted and paged (<c>GET storage/collections/data/{collection}</c>).</summary>
	/// <typeparam name="T">The document type.</typeparam>
	/// <param name="collection">The collection name.</param>
	/// <param name="query">The filter, fields, sort and paging, or <see langword="null"/> for every document.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>The matching documents.</returns>
	[Get("services/storage/collections/data/{collection}")]
	Task<IReadOnlyList<T>> QueryAsync<T>(string collection, [Query] KvStoreQuery? query, CancellationToken cancellationToken);

	/// <summary>Inserts a document (<c>POST storage/collections/data/{collection}</c>, JSON body).</summary>
	/// <typeparam name="T">The document type.</typeparam>
	/// <param name="collection">The collection name.</param>
	/// <param name="document">The document; one without <c>_key</c> gets a generated key.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>The document's key.</returns>
	[Post("services/storage/collections/data/{collection}")]
	Task<KvStoreKey> InsertAsync<T>(string collection, [Body] JsonBody<T> document, CancellationToken cancellationToken);

	/// <summary>Deletes the documents matching a filter, or every document (<c>DELETE storage/collections/data/{collection}</c>).</summary>
	/// <param name="collection">The collection name.</param>
	/// <param name="options">The filter, or <see langword="null"/> to delete every document.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A task that completes when the documents are deleted.</returns>
	[Delete("services/storage/collections/data/{collection}")]
	Task DeleteWhereAsync(string collection, [Query] KvStoreDeleteOptions? options, CancellationToken cancellationToken);

	/// <summary>Gets one document by key (<c>GET storage/collections/data/{collection}/{key}</c>).</summary>
	/// <typeparam name="T">The document type.</typeparam>
	/// <param name="collection">The collection name.</param>
	/// <param name="key">The document's key.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>The document. A missing key raises <see cref="SplunkApiException"/> with status 404.</returns>
	[Get("services/storage/collections/data/{collection}/{key}")]
	Task<T> GetAsync<T>(string collection, string key, CancellationToken cancellationToken);

	/// <summary>Replaces one document, or inserts it under that key (<c>POST storage/collections/data/{collection}/{key}</c>).</summary>
	/// <typeparam name="T">The document type.</typeparam>
	/// <param name="collection">The collection name.</param>
	/// <param name="key">The document's key.</param>
	/// <param name="document">The complete new document; fields not sent are removed.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>The document's key.</returns>
	[Post("services/storage/collections/data/{collection}/{key}")]
	Task<KvStoreKey> UpdateAsync<T>(string collection, string key, [Body] JsonBody<T> document, CancellationToken cancellationToken);

	/// <summary>Deletes one document by key (<c>DELETE storage/collections/data/{collection}/{key}</c>).</summary>
	/// <param name="collection">The collection name.</param>
	/// <param name="key">The document's key.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A task that completes when the document is deleted.</returns>
	[Delete("services/storage/collections/data/{collection}/{key}")]
	Task DeleteAsync(string collection, string key, CancellationToken cancellationToken);

	/// <summary>Runs several queries in one request (<c>POST storage/collections/data/{collection}/batch_find</c>).</summary>
	/// <typeparam name="T">The document type.</typeparam>
	/// <param name="collection">The collection name.</param>
	/// <param name="queries">The queries.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>One list of matching documents per query, in order.</returns>
	/// <remarks>Here <c>sort</c> is a list of objects (<c>[{"n": -1}]</c>), not the <c>n:-1</c> string of a GET.</remarks>
	[Post("services/storage/collections/data/{collection}/batch_find")]
	Task<IReadOnlyList<IReadOnlyList<T>>> BatchFindAsync<T>(string collection, [Body] JsonBody<IEnumerable<KvStoreBatchQuery>> queries, CancellationToken cancellationToken);

	/// <summary>
	/// Inserts or replaces several documents in one request (<c>POST storage/collections/data/{collection}/batch_save</c>):
	/// a document with a <c>_key</c> that exists is replaced.
	/// </summary>
	/// <typeparam name="T">The document type.</typeparam>
	/// <param name="collection">The collection name.</param>
	/// <param name="documents">The documents.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>The documents' keys, in order.</returns>
	[Post("services/storage/collections/data/{collection}/batch_save")]
	Task<IReadOnlyList<string>> BatchSaveAsync<T>(string collection, [Body] JsonBody<IEnumerable<T>> documents, CancellationToken cancellationToken);
}
