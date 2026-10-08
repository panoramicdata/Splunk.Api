using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Cluster;

/// <summary>The progress of a search head cluster automated upgrade.</summary>
public sealed class ShClusterUpgradeProgress
{
	/// <summary>The cluster's upgrade status, for example <c>completed</c>.</summary>
	[JsonPropertyName("upgrade_status")]
	public string? UpgradeStatus { get; init; }

	/// <summary>The counts.</summary>
	[JsonPropertyName("statistics")]
	public ShClusterUpgradeStatistics? Statistics { get; init; }

	/// <summary>Each member's state.</summary>
	[JsonPropertyName("peers")]
	public IReadOnlyList<ShClusterUpgradePeer> Peers { get; init; } = [];
}
