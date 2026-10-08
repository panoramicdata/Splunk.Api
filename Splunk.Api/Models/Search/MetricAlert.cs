using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Search;

/// <summary>
/// A streaming metric alert (<c>alerts/metric_alerts</c>). Action settings (<c>action.&lt;name&gt;.*</c>) and labels
/// (<c>label.*</c>) are in <see cref="SplunkContent.AdditionalProperties"/>.
/// </summary>
public sealed class MetricAlert : SplunkContent
{
	/// <summary>The condition that triggers the alert, for example <c>'avg(cpu.usage)' &gt; 90</c>.</summary>
	[JsonPropertyName("condition")]
	public string? Condition { get; init; }

	/// <summary>The metric indexes the alert watches, comma-separated.</summary>
	[JsonPropertyName("metric_indexes")]
	public string? MetricIndexes { get; init; }

	/// <summary>The dimensions the condition is evaluated per, comma-separated.</summary>
	[JsonPropertyName("groupby")]
	public string? GroupBy { get; init; }

	/// <summary>A filter on dimensions, for example <c>host=web*</c>.</summary>
	[JsonPropertyName("filter")]
	public string? Filter { get; init; }

	/// <summary>The description.</summary>
	[JsonPropertyName("description")]
	public string? Description { get; init; }

	/// <summary>How long a triggered alert is kept, for example <c>24h</c>.</summary>
	[JsonPropertyName("trigger.expires")]
	public string? TriggerExpires { get; init; }

	/// <summary>The most triggered alerts kept.</summary>
	[JsonPropertyName("trigger.max_tracked")]
	public int TriggerMaxTracked { get; init; }

	/// <summary>How long to suppress the alert after it triggers.</summary>
	[JsonPropertyName("trigger.suppress")]
	public string? TriggerSuppress { get; init; }

	/// <summary>The internal group key of the streaming alert (<c>_group_key</c>).</summary>
	[JsonPropertyName("_group_key")]
	public string? GroupKey { get; init; }
}
