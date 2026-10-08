using Splunk.Api.Serialization;
using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Cluster;

/// <summary>The state of the search head cluster captain (<c>shcluster/captain/info</c>).</summary>
public sealed class ShClusterCaptainInfo : SplunkContent
{
	/// <summary>The captain's name.</summary>
	[JsonPropertyName("label")]
	public string? Label { get; init; }

	/// <summary>The search head cluster ID.</summary>
	[JsonPropertyName("id")]
	public string? ClusterId { get; init; }

	/// <summary>The captain's URI.</summary>
	[JsonPropertyName("peer_scheme_host_port")]
	public string? PeerSchemeHostPort { get; init; }

	/// <summary>When the captain was elected.</summary>
	[JsonPropertyName("elected_captain")]
	[JsonConverter(typeof(EpochSecondsConverter))]
	public DateTimeOffset? ElectedCaptain { get; init; }

	/// <summary>When the captain started.</summary>
	[JsonPropertyName("start_time")]
	[JsonConverter(typeof(EpochSecondsConverter))]
	public DateTimeOffset? StartTime { get; init; }

	/// <summary>Whether the cluster is initialized.</summary>
	[JsonPropertyName("initialized_flag")]
	public bool? InitializedFlag { get; init; }

	/// <summary>Whether the cluster is in maintenance mode.</summary>
	[JsonPropertyName("maintenance_mode")]
	public bool? MaintenanceMode { get; init; }

	/// <summary>Whether at least replication factor members have joined.</summary>
	[JsonPropertyName("min_peers_joined_flag")]
	public bool? MinPeersJoinedFlag { get; init; }

	/// <summary>Whether a rolling restart is in progress.</summary>
	[JsonPropertyName("rolling_restart_flag")]
	public bool? RollingRestartFlag { get; init; }

	/// <summary>Whether the captain is ready to service the cluster.</summary>
	[JsonPropertyName("service_ready_flag")]
	public bool? ServiceReadyFlag { get; init; }
}
