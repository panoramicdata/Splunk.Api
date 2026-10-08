using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Search;

/// <summary>
/// Runs a scheduled view's delivery search now (<c>POST scheduled/views/{name}/dispatch</c>). <c>args.*</c> and other
/// <c>dispatch.*</c> overrides go in <see cref="SplunkFormRequest.AdditionalParameters"/>.
/// </summary>
public sealed class ScheduledViewDispatchRequest : SplunkFormRequest
{
	/// <summary>The time the run treats as now (<c>dispatch.now</c>).</summary>
	[JsonPropertyName("dispatch.now")]
	public string? DispatchNow { get; init; }

	/// <summary>Whether the run sends the email (<c>trigger_actions</c>).</summary>
	[JsonPropertyName("trigger_actions")]
	public bool? TriggerActions { get; init; }

	/// <summary>Whether to start a new run even if one is already running (<c>force_dispatch</c>).</summary>
	[JsonPropertyName("force_dispatch")]
	public bool? ForceDispatch { get; init; }
}
