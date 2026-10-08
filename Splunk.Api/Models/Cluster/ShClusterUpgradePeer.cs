using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Cluster;

/// <summary>The upgrade state of one search head cluster member.</summary>
public sealed class ShClusterUpgradePeer
{
	/// <summary>The member name.</summary>
	[JsonPropertyName("name")]
	public string? Name { get; init; }

	/// <summary>The member's upgrade status, for example <c>upgraded</c>.</summary>
	[JsonPropertyName("status")]
	public string? Status { get; init; }

	/// <summary>When the member's state last changed, as Splunk formats it.</summary>
	[JsonPropertyName("last_modified")]
	public string? LastModified { get; init; }
}
