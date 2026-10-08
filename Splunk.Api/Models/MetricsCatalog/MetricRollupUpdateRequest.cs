using System.Text.Json.Serialization;

namespace Splunk.Api.Models.MetricsCatalog;

/// <summary>Changes a rollup policy (<c>POST catalog/metricstore/rollup/{index}</c>); at least one property must be set.</summary>
public sealed class MetricRollupUpdateRequest : MetricRollupSettings
{
	/// <summary>The summaries, comma-separated <c>&lt;span&gt;|&lt;rollup index&gt;</c> pairs (<c>summaries</c>).</summary>
	[JsonPropertyName("summaries")]
	public string? Summaries { get; init; }
}
