using Splunk.Api.Serialization;
using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Cluster;

/// <summary>A cluster generation: the set of peers whose primary bucket copies searches use (<c>cluster/manager/generation</c>, <c>cluster/searchhead/generation</c>).</summary>
public sealed class ClusterGeneration : SplunkContent
{
	/// <summary>The current generation ID.</summary>
	[JsonPropertyName("generation_id")]
	public long? GenerationId { get; init; }

	/// <summary>The peers of the generation, by GUID.</summary>
	[JsonPropertyName("generation_peers")]
	public IReadOnlyDictionary<string, ClusterGenerationPeer> GenerationPeers { get; init; } = new Dictionary<string, ClusterGenerationPeer>();

	/// <summary>The ID the manager will use for the next generation (manager only).</summary>
	[JsonPropertyName("pending_generation_id")]
	public long? PendingGenerationId { get; init; }

	/// <summary>When the manager last tried to commit the pending generation (manager only).</summary>
	[JsonPropertyName("pending_last_attempt")]
	[JsonConverter(typeof(EpochSecondsConverter))]
	public DateTimeOffset? PendingLastAttempt { get; init; }

	/// <summary>Why the last commit attempt failed (manager only).</summary>
	[JsonPropertyName("pending_last_reason")]
	public string? PendingLastReason { get; init; }

	/// <summary>Whether the replication factor is met.</summary>
	[JsonPropertyName("replication_factor_met")]
	public bool? ReplicationFactorMet { get; init; }

	/// <summary>Whether the search factor is met.</summary>
	[JsonPropertyName("search_factor_met")]
	public bool? SearchFactorMet { get; init; }

	/// <summary>Whether the generation was committed by force.</summary>
	[JsonPropertyName("was_forced")]
	public bool? WasForced { get; init; }
}
