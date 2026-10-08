using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Cluster;

/// <summary>The result of starting or recovering a search head cluster automated upgrade.</summary>
public sealed class ShClusterUpgradeResult
{
	/// <summary>A message, for example <c>Upgrade initiated</c>.</summary>
	[JsonPropertyName("message")]
	public string? Message { get; init; }

	/// <summary><c>succeeded</c> or <c>failed</c>.</summary>
	[JsonPropertyName("status")]
	public string? Status { get; init; }
}
