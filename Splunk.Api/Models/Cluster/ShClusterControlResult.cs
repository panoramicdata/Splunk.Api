using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Cluster;

/// <summary>The result of a search head cluster captain control action.</summary>
public sealed class ShClusterControlResult : SplunkContent
{
	/// <summary>Whether the action succeeded.</summary>
	[JsonPropertyName("success")]
	public bool? Success { get; init; }

	/// <summary>Why the action failed, if it did.</summary>
	[JsonPropertyName("msg")]
	public string? Message { get; init; }

	/// <summary>Whether a rolling upgrade is now in progress (<c>yes</c> or <c>no</c>), for the upgrade actions.</summary>
	[JsonPropertyName("upgrade")]
	public string? Upgrade { get; init; }
}
