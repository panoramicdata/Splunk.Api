using Splunk.Api.Interfaces;

namespace Splunk.Api;

public sealed partial class SplunkClient
{
	/// <summary>The metrics catalog: metric names, dimensions and values (<c>catalog/metricstore/metrics</c>, <c>dimensions</c>).</summary>
	public IMetricsCatalog MetricsCatalog => field ??= For<IMetricsCatalog>();

	/// <summary>Metric rollup policies (<c>catalog/metricstore/rollup</c>).</summary>
	public IMetricRollups MetricRollups => field ??= For<IMetricRollups>();
}
