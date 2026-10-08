using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Search;

/// <summary>
/// An alert action and its defaults (<c>alerts/alert_actions</c>), for example <c>email</c>, <c>webhook</c> or
/// <c>logevent</c>. Action-specific settings (such as the email server) are in
/// <see cref="SplunkContent.AdditionalProperties"/>.
/// </summary>
public sealed class AlertAction : SplunkContent
{
	/// <summary>The name shown in the UI.</summary>
	[JsonPropertyName("label")]
	public string? Label { get; init; }

	/// <summary>The description.</summary>
	[JsonPropertyName("description")]
	public string? Description { get; init; }

	/// <summary>The search command that runs the action.</summary>
	[JsonPropertyName("command")]
	public string? Command { get; init; }

	/// <summary>The icon file.</summary>
	[JsonPropertyName("icon_path")]
	public string? IconPath { get; init; }

	/// <summary>Whether the action is a custom (app-provided) alert action.</summary>
	[JsonPropertyName("is_custom")]
	public bool IsCustom { get; init; }

	/// <summary>The most results passed to the action.</summary>
	[JsonPropertyName("maxresults")]
	public long MaxResults { get; init; }

	/// <summary>The most time the action may take, for example <c>5m</c>.</summary>
	[JsonPropertyName("maxtime")]
	public string? MaxTime { get; init; }

	/// <summary>Whether running the action records the alert in <c>alerts/fired_alerts</c>.</summary>
	[JsonPropertyName("track_alert")]
	public bool TrackAlert { get; init; }

	/// <summary>How long the action's artifacts are kept: seconds, or periods such as <c>10p</c>.</summary>
	[JsonPropertyName("ttl")]
	public string? Ttl { get; init; }

	/// <summary>The payload format of a custom action, for example <c>json</c>.</summary>
	[JsonPropertyName("payload_format")]
	public string? PayloadFormat { get; init; }
}
