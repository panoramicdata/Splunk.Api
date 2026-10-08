using Refit;
using Splunk.Api.Models;
using Splunk.Api.Models.Introspection;

namespace Splunk.Api.Interfaces;

/// <summary>
/// Indexes (<c>data/indexes</c>). A namespace (<c>client.InNamespace("nobody", app)</c>) chooses the app a new index is
/// defined in; listing may be limited by the <c>indexes_list_all</c> capability.
/// </summary>
public interface IIndexes
{
	/// <summary>Lists the indexes (<c>GET data/indexes</c>).</summary>
	/// <param name="options">Paging, filtering and the data type, or <see langword="null"/> for event indexes with Splunk's defaults.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with one entry per index.</returns>
	[Get("services/data/indexes")]
	Task<SplunkFeed<SplunkIndex>> ListAsync([Query] IndexListOptions? options, CancellationToken cancellationToken);

	/// <summary>Creates an index (<c>POST data/indexes</c>).</summary>
	/// <param name="request">The index name and settings.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with the new index.</returns>
	[Post("services/data/indexes")]
	Task<SplunkFeed<SplunkIndex>> CreateAsync([Body] IndexCreateRequest request, CancellationToken cancellationToken);

	/// <summary>Gets one index (<c>GET data/indexes/{name}</c>).</summary>
	/// <param name="name">The index name.</param>
	/// <param name="options">Whether to summarize, or <see langword="null"/> for every property.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with the index.</returns>
	[Get("services/data/indexes/{name}")]
	Task<SplunkFeed<SplunkIndex>> GetAsync(string name, [Query] IndexGetOptions? options, CancellationToken cancellationToken);

	/// <summary>Changes an index's settings (<c>POST data/indexes/{name}</c>).</summary>
	/// <param name="name">The index name.</param>
	/// <param name="request">The settings to change.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with the updated index.</returns>
	[Post("services/data/indexes/{name}")]
	Task<SplunkFeed<SplunkIndex>> UpdateAsync(string name, [Body] IndexUpdateRequest request, CancellationToken cancellationToken);

	/// <summary>Deletes an index and all its data (<c>DELETE data/indexes/{name}</c>).</summary>
	/// <param name="name">The index name.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A task that completes when the index is removed.</returns>
	/// <remarks>Splunk answers <c>409</c> for an index that was disabled but not yet removed by a restart. Check no input still sends to it.</remarks>
	[Delete("services/data/indexes/{name}")]
	Task DeleteAsync(string name, CancellationToken cancellationToken);
}
