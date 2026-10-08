using System.Text.Json.Serialization;

namespace Splunk.Api.Models.MetricsCatalog;

/// <summary>One rollup summary of a <see cref="MetricRollupPolicy"/>: a span and the index its data points go to.</summary>
public sealed class MetricRollupSummary
{
	/// <summary>The metric index the rolled-up data points are written to.</summary>
	[JsonPropertyName("rollupIndex")]
	public string? RollupIndex { get; init; }

	/// <summary>The rollup span, for example <c>1h</c>.</summary>
	[JsonPropertyName("span")]
	public string? Span { get; init; }
}
