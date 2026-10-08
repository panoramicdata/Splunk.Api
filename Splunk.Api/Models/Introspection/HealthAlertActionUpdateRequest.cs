using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Introspection;

/// <summary>Configures an alert action of the health report (<c>POST server/health-config/alert_action:{name}</c>).</summary>
public sealed class HealthAlertActionUpdateRequest : SplunkFormRequest
{
	/// <summary>Email recipients (<c>action.to</c>).</summary>
	[JsonPropertyName("action.to")]
	public string? To { get; init; }

	/// <summary>Email copy recipients (<c>action.cc</c>).</summary>
	[JsonPropertyName("action.cc")]
	public string? Cc { get; init; }

	/// <summary>Email blind copy recipients (<c>action.bcc</c>).</summary>
	[JsonPropertyName("action.bcc")]
	public string? Bcc { get; init; }

	/// <summary>A PagerDuty integration URL that overrides the default (<c>action.integration_url_override</c>).</summary>
	[JsonPropertyName("action.integration_url_override")]
	public string? IntegrationUrlOverride { get; init; }

	/// <summary>A webhook URL (<c>action.url</c>).</summary>
	[JsonPropertyName("action.url")]
	public string? Url { get; init; }

	/// <summary>Whether the action is disabled (<c>disabled</c>).</summary>
	[JsonPropertyName("disabled")]
	public bool? Disabled { get; init; }
}
