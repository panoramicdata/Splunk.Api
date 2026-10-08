using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Knowledge;

/// <summary>Creates a log-to-metrics transformation (<c>POST data/transforms/metric-schema</c>).</summary>
/// <remarks>
/// The reference names the fields <c>field_name</c> and <c>blacklist_dimension</c>; Splunk 10.6 takes
/// <c>field_names</c>, <c>blacklist_dimensions</c> and <c>whitelist_dimensions</c>, as its own example does.
/// </remarks>
public sealed class MetricSchemaCreateRequest : SplunkFormRequest
{
	/// <summary>The schema name; Splunk stores it as the <c>transforms.conf</c> stanza <c>metric-schema:{name}</c>.</summary>
	[JsonPropertyName("name")]
	public required string Name { get; init; }

	/// <summary>The measure fields to extract from each event, comma-separated (<c>field_names</c>).</summary>
	[JsonPropertyName("field_names")]
	public required string FieldNames { get; init; }

	/// <summary>The dimension fields to leave out of the metric data points, comma-separated (<c>blacklist_dimensions</c>).</summary>
	[JsonPropertyName("blacklist_dimensions")]
	public string? BlacklistDimensions { get; init; }

	/// <summary>The only dimension fields to keep in the metric data points, comma-separated (<c>whitelist_dimensions</c>).</summary>
	[JsonPropertyName("whitelist_dimensions")]
	public string? WhitelistDimensions { get; init; }

	/// <summary>
	/// For logs with several event schemas, a field shared by every event whose value selects the schema
	/// (<c>metric_name_prefix</c>); the stored keys are then suffixed with it.
	/// </summary>
	[JsonPropertyName("metric_name_prefix")]
	public string? MetricNamePrefix { get; init; }
}
