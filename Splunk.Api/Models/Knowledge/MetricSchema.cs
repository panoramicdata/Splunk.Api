using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Knowledge;

/// <summary>A log-to-metrics transformation, a <c>metric-schema:</c> stanza in <c>transforms.conf</c> (<c>data/transforms/metric-schema</c>).</summary>
/// <remarks>
/// The entry name is <c>metric-schema:{name}</c>. A schema with a <c>metric_name_prefix</c> reports
/// <c>METRIC-SCHEMA-MEASURES-{prefix}</c> and similar keys, which are kept in <see cref="SplunkContent.AdditionalProperties"/>.
/// </remarks>
public sealed class MetricSchema : SplunkContent
{
	/// <summary>The measure fields, comma-separated (<c>METRIC-SCHEMA-MEASURES</c>).</summary>
	[JsonPropertyName("METRIC-SCHEMA-MEASURES")]
	public string? Measures { get; init; }

	/// <summary>The dimension fields left out of the metric data points (<c>METRIC-SCHEMA-BLACKLIST-DIMS</c>).</summary>
	[JsonPropertyName("METRIC-SCHEMA-BLACKLIST-DIMS")]
	public string? BlacklistDimensions { get; init; }

	/// <summary>The only dimension fields kept in the metric data points (<c>METRIC-SCHEMA-WHITELIST-DIMS</c>).</summary>
	[JsonPropertyName("METRIC-SCHEMA-WHITELIST-DIMS")]
	public string? WhitelistDimensions { get; init; }
}
