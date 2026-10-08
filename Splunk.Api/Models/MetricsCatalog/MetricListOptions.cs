using Refit;

namespace Splunk.Api.Models.MetricsCatalog;

/// <summary>Query parameters for <c>GET catalog/metricstore/metrics</c>.</summary>
public sealed class MetricListOptions : MetricCatalogOptions
{
	/// <summary>Whether to return each metric's indexes in <see cref="MetricCatalogItem.Indexes"/> (<c>list_indexes</c>).</summary>
	[AliasAs("list_indexes")]
	public bool? ListIndexes { get; init; }
}
