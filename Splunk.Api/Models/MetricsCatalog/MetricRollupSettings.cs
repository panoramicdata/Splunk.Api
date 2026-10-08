using System.Text.Json.Serialization;

namespace Splunk.Api.Models.MetricsCatalog;

/// <summary>The settings of a rollup policy, for <see cref="MetricRollupCreateRequest"/> and <see cref="MetricRollupUpdateRequest"/>.</summary>
public abstract class MetricRollupSettings : SplunkFormRequest
{
	/// <summary>The default aggregation functions, separated by <c>#</c> (<c>default_agg</c>), for example <c>avg#max</c>.</summary>
	[JsonPropertyName("default_agg")]
	public string? DefaultAggregation { get; init; }

	/// <summary>Metric names, comma-separated, filtered by <see cref="MetricListType"/> (<c>metric_list</c>).</summary>
	[JsonPropertyName("metric_list")]
	public string? MetricList { get; init; }

	/// <summary>Whether <see cref="MetricList"/> is <c>included</c> or <c>excluded</c> (<c>metric_list_type</c>); Splunk's default is <c>excluded</c>.</summary>
	[JsonPropertyName("metric_list_type")]
	public string? MetricListType { get; init; }

	/// <summary>Dimensions, comma-separated, filtered by <see cref="DimensionListType"/> (<c>dimension_list</c>).</summary>
	[JsonPropertyName("dimension_list")]
	public string? DimensionList { get; init; }

	/// <summary>Whether <see cref="DimensionList"/> is <c>included</c> or <c>excluded</c> (<c>dimension_list_type</c>).</summary>
	[JsonPropertyName("dimension_list_type")]
	public string? DimensionListType { get; init; }

	/// <summary>
	/// Per-metric aggregation overrides, comma-separated <c>&lt;metric&gt;|&lt;functions&gt;</c> pairs (<c>metric_overrides</c>),
	/// for example <c>cpu.usage|max#min</c>.
	/// </summary>
	[JsonPropertyName("metric_overrides")]
	public string? MetricOverrides { get; init; }
}
