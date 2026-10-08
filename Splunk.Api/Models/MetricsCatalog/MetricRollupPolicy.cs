using System.Text.Json;
using System.Text.Json.Serialization;

namespace Splunk.Api.Models.MetricsCatalog;

/// <summary>
/// The rollup policy of a source metric index (<c>catalog/metricstore/rollup</c>); the entry's name is the source index.
/// Per-metric aggregation overrides (<c>aggregation.&lt;metric&gt;</c>) are gathered in <see cref="MetricOverrides"/>.
/// </summary>
public sealed class MetricRollupPolicy : SplunkContent
{
	private const string OverridePrefix = "aggregation.";

	/// <summary>The default aggregation functions, separated by <c>#</c>, for example <c>avg#max</c>.</summary>
	[JsonPropertyName("defaultAggregation")]
	public string? DefaultAggregation { get; init; }

	/// <summary>The metrics filtered by <see cref="MetricListType"/>, comma-separated.</summary>
	[JsonPropertyName("metricList")]
	public string? MetricList { get; init; }

	/// <summary>Whether <see cref="MetricList"/> lists the metrics <c>included</c> or <c>excluded</c>.</summary>
	[JsonPropertyName("metricListType")]
	public string? MetricListType { get; init; }

	/// <summary>The dimensions filtered by <see cref="DimensionListType"/>, comma-separated.</summary>
	[JsonPropertyName("dimensionList")]
	public string? DimensionList { get; init; }

	/// <summary>Whether <see cref="DimensionList"/> lists the dimensions <c>included</c> or <c>excluded</c>.</summary>
	[JsonPropertyName("dimensionListType")]
	public string? DimensionListType { get; init; }

	/// <summary>The smallest span a summary may use, in seconds.</summary>
	[JsonPropertyName("minSpanAllowed")]
	public int MinSpanAllowed { get; init; }

	/// <summary>The app the policy is stored in.</summary>
	[JsonPropertyName("appName")]
	public string? AppName { get; init; }

	/// <summary>
	/// The rollup summaries, keyed by position (<c>"0"</c>, <c>"1"</c>, ...). Splunk 10.6 returns an object, not the
	/// comma-separated list the reference describes.
	/// </summary>
	[JsonPropertyName("summaries")]
	public IReadOnlyDictionary<string, MetricRollupSummary> Summaries { get; init; } = new Dictionary<string, MetricRollupSummary>();

	/// <summary>The per-metric aggregation overrides (<c>aggregation.&lt;metric&gt;</c>), keyed by metric name.</summary>
	[JsonIgnore]
	public IReadOnlyDictionary<string, string> MetricOverrides
		=> AdditionalProperties
			.Where(p => p.Key.StartsWith(OverridePrefix, StringComparison.Ordinal) && p.Value.ValueKind == JsonValueKind.String)
			.ToDictionary(p => p.Key[OverridePrefix.Length..], p => p.Value.GetString()!, StringComparer.Ordinal);
}
