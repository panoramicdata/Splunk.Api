using Refit;
using Splunk.Api.Models;
using Splunk.Api.Models.MetricsCatalog;

namespace Splunk.Api.Interfaces;

/// <summary>
/// Metric rollup policies (<c>catalog/metricstore/rollup</c>), stored in metric_rollups.conf. Needs the
/// <c>list_metrics_catalog</c> capability to read and <c>edit_metrics_rollup</c> to change.
/// </summary>
public interface IMetricRollups
{
	/// <summary>Lists the rollup policies (<c>GET catalog/metricstore/rollup</c>).</summary>
	/// <param name="options">Paging and filtering; <see langword="null"/> for Splunk's defaults.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with one entry per source index.</returns>
	[Get("services/catalog/metricstore/rollup")]
	Task<SplunkFeed<MetricRollupPolicy>> ListAsync([Query] ListOptions? options, CancellationToken cancellationToken);

	/// <summary>Creates the rollup policy of a source metric index (<c>POST catalog/metricstore/rollup</c>).</summary>
	/// <param name="request">The source index, summaries and aggregation settings.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with one entry, the new policy.</returns>
	/// <remarks>The source and rollup indexes must be metric indexes.</remarks>
	[Post("services/catalog/metricstore/rollup")]
	Task<SplunkFeed<MetricRollupPolicy>> CreateAsync([Body] MetricRollupCreateRequest request, CancellationToken cancellationToken);

	/// <summary>Gets the rollup policy of a source index (<c>GET catalog/metricstore/rollup/{index}</c>).</summary>
	/// <param name="index">The source metric index.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with one entry.</returns>
	[Get("services/catalog/metricstore/rollup/{index}")]
	Task<SplunkFeed<MetricRollupPolicy>> GetAsync(string index, CancellationToken cancellationToken);

	/// <summary>Changes the rollup policy of a source index (<c>POST catalog/metricstore/rollup/{index}</c>).</summary>
	/// <param name="index">The source metric index.</param>
	/// <param name="request">The settings to change.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with one entry, the changed policy.</returns>
	[Post("services/catalog/metricstore/rollup/{index}")]
	Task<SplunkFeed<MetricRollupPolicy>> UpdateAsync(string index, [Body] MetricRollupUpdateRequest request, CancellationToken cancellationToken);

	/// <summary>Deletes the rollup policy of a source index (<c>DELETE catalog/metricstore/rollup/{index}</c>); the indexes remain.</summary>
	/// <param name="index">The source metric index.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A task that completes when the policy is deleted.</returns>
	[Delete("services/catalog/metricstore/rollup/{index}")]
	Task DeleteAsync(string index, CancellationToken cancellationToken);
}
