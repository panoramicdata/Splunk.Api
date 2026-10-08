using Refit;
using Splunk.Api.Models;
using Splunk.Api.Models.FederatedSearch;

namespace Splunk.Api.Interfaces;

/// <summary>Federated index definitions (<c>data/federated/index</c>).</summary>
/// <remarks>
/// Federated Search for Splunk index names have the form <c>federated:&lt;name&gt;</c> (pass the whole name, colon
/// included); Amazon S3 index names have no prefix. Reading and changing need <c>edit_federated_indexes</c> (or the index
/// included in the user's role); deleting needs the power or admin role; Amazon S3 indexes also need
/// <c>edit_federated_providers</c>. A new index goes to the app of the request's namespace (the user's default app
/// under <c>services/</c>).
/// </remarks>
public interface IFederatedIndexes
{
	/// <summary>Lists the federated indexes (<c>GET data/federated/index</c>).</summary>
	/// <param name="options">Paging, filtering and sorting, or <see langword="null"/>.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with one entry per federated index.</returns>
	[Get("services/data/federated/index")]
	Task<SplunkFeed<FederatedIndex>> ListAsync([Query] ListOptions? options, CancellationToken cancellationToken);

	/// <summary>Creates a federated index (<c>POST data/federated/index</c>).</summary>
	/// <param name="request">The new federated index.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with the new index.</returns>
	[Post("services/data/federated/index")]
	Task<SplunkFeed<FederatedIndex>> CreateAsync([Body] FederatedIndexCreateRequest request, CancellationToken cancellationToken);

	/// <summary>Gets a federated index (<c>GET data/federated/index/{name}</c>).</summary>
	/// <remarks>The reference's URL block for this endpoint wrongly reads <c>data/federated/provider/{name}</c>.</remarks>
	/// <param name="name">The index name, for example <c>federated:remote_main</c>.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with the index.</returns>
	[Get("services/data/federated/index/{name}")]
	Task<SplunkFeed<FederatedIndex>> GetAsync(string name, CancellationToken cancellationToken);

	/// <summary>Changes a federated index (<c>POST data/federated/index/{name}</c>).</summary>
	/// <param name="name">The index name.</param>
	/// <param name="request">The settings to change.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with the updated index.</returns>
	[Post("services/data/federated/index/{name}")]
	Task<SplunkFeed<FederatedIndex>> UpdateAsync(string name, [Body] FederatedIndexUpdateRequest request, CancellationToken cancellationToken);

	/// <summary>Deletes a federated index (<c>DELETE data/federated/index/{name}</c>).</summary>
	/// <param name="name">The index name.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A task that completes when the index is deleted.</returns>
	[Delete("services/data/federated/index/{name}")]
	Task DeleteAsync(string name, CancellationToken cancellationToken);

	/// <summary>Turns a federated index off, so it is not searchable (<c>POST data/federated/index/{name}/disable</c>).</summary>
	/// <param name="name">The index name.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with the index.</returns>
	[Post("services/data/federated/index/{name}/disable")]
	Task<SplunkFeed<FederatedIndex>> DisableAsync(string name, CancellationToken cancellationToken);

	/// <summary>Turns a federated index back on (<c>POST data/federated/index/{name}/enable</c>).</summary>
	/// <param name="name">The index name.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with the index.</returns>
	[Post("services/data/federated/index/{name}/enable")]
	Task<SplunkFeed<FederatedIndex>> EnableAsync(string name, CancellationToken cancellationToken);
}
