using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Deployment;

/// <summary>A deployment server setting Splunk Web cannot change (<c>deployment/server/config/attributesUnsupportedInUI</c>).</summary>
public sealed class DeploymentUnsupportedSetting : SplunkContent
{
	/// <summary>The setting, for example <c>whitelist.0</c>.</summary>
	[JsonPropertyName("property")]
	public string? Property { get; init; }

	/// <summary>Why Splunk Web cannot change it.</summary>
	[JsonPropertyName("reason")]
	public string? Reason { get; init; }

	/// <summary>The serverclass.conf stanza that holds it.</summary>
	[JsonPropertyName("stanza")]
	public string? Stanza { get; init; }
}
