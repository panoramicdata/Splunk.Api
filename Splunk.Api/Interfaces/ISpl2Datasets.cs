using Refit;
using Splunk.Api.Models.Spl2;

namespace Splunk.Api.Interfaces;

/// <summary>The datasets SPL2 can read (<c>orchestrator/v1/datasets</c>). These endpoints answer plain JSON, not the feed envelope.</summary>
public interface ISpl2Datasets
{
	/// <summary>Lists datasets (<c>GET orchestrator/v1/datasets</c>).</summary>
	/// <param name="options">Kind, filter, ordering and paging; <see langword="null"/> for the first 1000 datasets.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A page of datasets; <see cref="Spl2DatasetList.NextPaginationToken"/> reads the next one.</returns>
	/// <remarks>Splunk 10.6 rejects some kinds the reference lists, for example <c>kind=metric</c> (400, <c>kind=metric is not allowed</c>).</remarks>
	[Get("services/orchestrator/v1/datasets")]
	Task<Spl2DatasetList> ListAsync([Query] Spl2DatasetListOptions? options, CancellationToken cancellationToken);

	/// <summary>Gets a dataset by ID or resource name (<c>GET orchestrator/v1/datasets/{datasetid}</c>).</summary>
	/// <param name="datasetId">The ID (for example <c>indexes.main</c>) or resource name (for example <c>~indexes.main</c>).</param>
	/// <param name="withConnection">Whether to include the federated connection (<c>with_connection</c>); unified (federated) datasets only.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>The dataset.</returns>
	[Get("services/orchestrator/v1/datasets/{datasetId}")]
	Task<Spl2Dataset> GetAsync(string datasetId, [AliasAs("with_connection")] bool? withConnection, CancellationToken cancellationToken);
}
