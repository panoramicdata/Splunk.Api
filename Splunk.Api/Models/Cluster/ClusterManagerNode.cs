using Splunk.Api.Serialization;
using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Cluster;

/// <summary>A cluster manager taking part in cluster manager redundancy (<c>cluster/manager/redundancy</c>).</summary>
public sealed class ClusterManagerNode : SplunkContent
{
	/// <summary>The manager's server name.</summary>
	[JsonPropertyName("server_name")]
	public string? ServerName { get; init; }

	/// <summary>The manager's management URI.</summary>
	[JsonPropertyName("uri")]
	public string? Uri { get; init; }

	/// <summary>The manager's high availability mode.</summary>
	[JsonPropertyName("ha_mode")]
	public ClusterHaMode HaMode { get; init; }

	/// <summary>The switchover mode, for example <c>auto</c> or <c>manual</c>.</summary>
	[JsonPropertyName("manager_switchover_mode")]
	public string? ManagerSwitchoverMode { get; init; }

	/// <summary>The active bundle ID as this manager knows it.</summary>
	[JsonPropertyName("active_bundle_id")]
	public string? ActiveBundleId { get; init; }

	/// <summary>The last committed generation ID as this manager knows it.</summary>
	[JsonPropertyName("generation_id")]
	public long? GenerationId { get; init; }

	/// <summary>The number of peers this manager knows.</summary>
	[JsonPropertyName("peers_count")]
	public int? PeersCount { get; init; }

	/// <summary>When the active manager last heard from this standby manager; <see langword="null"/> for the active manager.</summary>
	[JsonPropertyName("last_heartbeat")]
	[JsonConverter(typeof(EpochSecondsConverter))]
	public DateTimeOffset? LastHeartbeat { get; init; }
}
