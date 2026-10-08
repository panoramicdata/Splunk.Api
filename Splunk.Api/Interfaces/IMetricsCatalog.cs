using Refit;
using Splunk.Api.Models;
using Splunk.Api.Models.MetricsCatalog;

namespace Splunk.Api.Interfaces;

/// <summary>
/// The metrics catalog (<c>catalog/metricstore/metrics</c> and <c>dimensions</c>): the metric names, dimensions and
/// dimension values in metric indexes.
/// </summary>
/// <remarks>The catalog is built in the background, so data points written moments ago may not be listed yet.</remarks>
public interface IMetricsCatalog
{
	/// <summary>Lists metric names (<c>GET catalog/metricstore/metrics</c>).</summary>
	/// <param name="options">Time range, filter, indexes and paging; <see langword="null"/> for the last day.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with one entry per metric, named after it.</returns>
	[Get("services/catalog/metricstore/metrics")]
	Task<SplunkFeed<MetricCatalogItem>> ListMetricsAsync([Query] MetricListOptions? options, CancellationToken cancellationToken);

	/// <summary>Lists the dimensions of a metric (<c>GET catalog/metricstore/dimensions</c>).</summary>
	/// <param name="metricName">The metric (<c>metric_name</c>); wildcards such as <c>cpu.*</c> or <c>*</c> are allowed.</param>
	/// <param name="options">Time range, filter and paging; <see langword="null"/> for the last day.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with one entry per dimension, named after it.</returns>
	[Get("services/catalog/metricstore/dimensions")]
	Task<SplunkFeed<MetricCatalogItem>> ListDimensionsAsync(
		[AliasAs("metric_name")] string metricName,
		[Query] MetricCatalogOptions? options,
		CancellationToken cancellationToken);

	/// <summary>Lists the values of a dimension (<c>GET catalog/metricstore/dimensions/{dimension-name}/values</c>).</summary>
	/// <param name="dimensionName">The dimension, for example <c>host</c>.</param>
	/// <param name="metricName">The metric (<c>metric_name</c>); wildcards are allowed.</param>
	/// <param name="options">Time range, filter and paging; <see langword="null"/> for the last day.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with one entry per value, named after it.</returns>
	[Get("services/catalog/metricstore/dimensions/{dimensionName}/values")]
	Task<SplunkFeed<MetricCatalogItem>> ListDimensionValuesAsync(
		string dimensionName,
		[AliasAs("metric_name")] string metricName,
		[Query] MetricCatalogOptions? options,
		CancellationToken cancellationToken);
}
