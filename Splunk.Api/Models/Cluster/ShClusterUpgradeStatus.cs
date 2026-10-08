using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Cluster;

/// <summary>The status of a search head cluster automated upgrade (<c>upgrade/shc/status</c>).</summary>
public sealed class ShClusterUpgradeStatus
{
	/// <summary>The progress.</summary>
	[JsonPropertyName("message")]
	public ShClusterUpgradeProgress? Message { get; init; }
}
