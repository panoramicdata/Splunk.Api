using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Cluster;

/// <summary>How many buckets have a given number of copies.</summary>
public sealed class ClusterCopiesTracker
{
	/// <summary>The number of buckets that have this many copies.</summary>
	[JsonPropertyName("actual_copies_per_slot")]
	public int? ActualCopiesPerSlot { get; init; }

	/// <summary>The number of buckets expected to have this many copies.</summary>
	[JsonPropertyName("expected_total_per_slot")]
	public int? ExpectedTotalPerSlot { get; init; }
}
