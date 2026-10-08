using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Search;

/// <summary>
/// The settings of a streaming metric alert, for <see cref="MetricAlertCreateRequest"/> and
/// <see cref="MetricAlertUpdateRequest"/>. Actions (<c>action.&lt;name&gt;</c> and <c>action.&lt;name&gt;.&lt;parameter&gt;</c>),
/// labels (<c>label.&lt;name&gt;</c>) and <c>splunk_ui.*</c> go in <see cref="SplunkFormRequest.AdditionalParameters"/>.
/// </summary>
public abstract class MetricAlertSettings : SplunkFormRequest
{
	/// <summary>The dimensions to evaluate the condition per, comma-separated (<c>groupby</c>).</summary>
	[JsonPropertyName("groupby")]
	public string? GroupBy { get; init; }

	/// <summary>A filter on dimensions (<c>filter</c>), for example <c>host=web*</c>.</summary>
	[JsonPropertyName("filter")]
	public string? Filter { get; init; }

	/// <summary>The description (<c>description</c>).</summary>
	[JsonPropertyName("description")]
	public string? Description { get; init; }

	/// <summary>Whether the alert is disabled (<c>disabled</c>).</summary>
	[JsonPropertyName("disabled")]
	public bool? Disabled { get; init; }

	/// <summary>How long a triggered alert is kept (<c>trigger.expires</c>), for example <c>24h</c>.</summary>
	[JsonPropertyName("trigger.expires")]
	public string? TriggerExpires { get; init; }

	/// <summary>The most triggered alerts kept (<c>trigger.max_tracked</c>).</summary>
	[JsonPropertyName("trigger.max_tracked")]
	public int? TriggerMaxTracked { get; init; }

	/// <summary>How long to suppress the alert after it triggers (<c>trigger.suppress</c>).</summary>
	[JsonPropertyName("trigger.suppress")]
	public string? TriggerSuppress { get; init; }
}
