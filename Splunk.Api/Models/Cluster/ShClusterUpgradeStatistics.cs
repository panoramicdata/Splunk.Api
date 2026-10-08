using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Cluster;

/// <summary>How far a search head cluster automated upgrade has got.</summary>
public sealed class ShClusterUpgradeStatistics
{
	/// <summary>The number of members to upgrade.</summary>
	[JsonPropertyName("peers_to_upgrade")]
	public int? PeersToUpgrade { get; init; }

	/// <summary>The number of members upgraded so far.</summary>
	[JsonPropertyName("overall_peers_upgraded")]
	public int? OverallPeersUpgraded { get; init; }

	/// <summary>The percentage of members upgraded so far.</summary>
	[JsonPropertyName("overall_peers_upgraded_percentage")]
	public double? OverallPeersUpgradedPercentage { get; init; }
}
