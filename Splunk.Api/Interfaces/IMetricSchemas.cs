using Refit;
using Splunk.Api.Models;
using Splunk.Api.Models.Knowledge;

namespace Splunk.Api.Interfaces;

/// <summary>Ingest-time log-to-metrics transformations (<c>data/transforms/metric-schema</c>).</summary>
/// <remarks>Requires the <c>edit_metric_schema</c> capability.</remarks>
public interface IMetricSchemas
{
	/// <summary>Lists log-to-metrics transformations (<c>GET data/transforms/metric-schema</c>).</summary>
	/// <param name="options">Paging and filtering; <see langword="null"/> for Splunk's defaults.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with one entry per schema, named <c>metric-schema:{name}</c>.</returns>
	[Get("services/data/transforms/metric-schema")]
	Task<SplunkFeed<MetricSchema>> ListAsync([Query] ListOptions? options, CancellationToken cancellationToken);

	/// <summary>Creates a log-to-metrics transformation (<c>POST data/transforms/metric-schema</c>).</summary>
	/// <param name="request">The name, measures and dimensions.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with the new schema.</returns>
	[Post("services/data/transforms/metric-schema")]
	Task<SplunkFeed<MetricSchema>> CreateAsync([Body] MetricSchemaCreateRequest request, CancellationToken cancellationToken);

	/// <summary>Deletes a log-to-metrics transformation (<c>DELETE data/transforms/metric-schema/{name}</c>).</summary>
	/// <param name="name">The schema name, with or without the <c>metric-schema:</c> prefix.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A task that completes when the schema is deleted.</returns>
	/// <remarks>
	/// The reference lists DELETE on the collection path, but its own example and Splunk 10.6 need the name: a DELETE
	/// without one fails with "Cannot perform action DELETE without a target name to act on".
	/// </remarks>
	[Delete("services/data/transforms/metric-schema/{name}")]
	Task DeleteAsync(string name, CancellationToken cancellationToken);
}
