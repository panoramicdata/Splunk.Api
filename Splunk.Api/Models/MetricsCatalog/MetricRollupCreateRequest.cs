using System.Text.Json.Serialization;

namespace Splunk.Api.Models.MetricsCatalog;

/// <summary>Creates the rollup policy of a source metric index (<c>POST catalog/metricstore/rollup</c>).</summary>
public sealed class MetricRollupCreateRequest : MetricRollupSettings
{
	/// <summary>The source metric index (<c>name</c>).</summary>
	[JsonPropertyName("name")]
	public required string Name { get; init; }

	/// <summary>
	/// The summaries, comma-separated <c>&lt;span&gt;|&lt;rollup index&gt;</c> pairs (<c>summaries</c>), for example
	/// <c>1h|metrics_1h,1d|metrics_1d</c>. Spans are limited to 1-60 minutes, 1-24 hours or 1 day.
	/// </summary>
	[JsonPropertyName("summaries")]
	public required string Summaries { get; init; }
}
