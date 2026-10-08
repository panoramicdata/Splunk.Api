using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Cluster;

/// <summary>The indexer clustering configuration of a node (<c>cluster/config</c>).</summary>
/// <remarks><see cref="SplunkContent.Disabled"/> reports whether clustering is off. Splunk 10.6 reports unset periods such as <c>heartbeat_period</c> as 18446744073709551615, so those stay in <see cref="SplunkContent.AdditionalProperties"/>.</remarks>
public sealed class ClusterConfig : SplunkContent
{
	/// <summary>The sites the manager recognises, comma-separated.</summary>
	[JsonPropertyName("available_sites")]
	public string? AvailableSites { get; init; }

	/// <summary>The cluster label.</summary>
	[JsonPropertyName("cluster_label")]
	public string? ClusterLabel { get; init; }

	/// <summary>The timeout, in seconds, for connecting between cluster nodes.</summary>
	[JsonPropertyName("cxn_timeout")]
	public int? ConnectionTimeout { get; init; }

	/// <summary>The port that receives forwarder data (indexer discovery).</summary>
	[JsonPropertyName("forwarderdata_rcv_port")]
	public int? ForwarderDataReceivePort { get; init; }

	/// <summary>Whether forwarder data is received over TLS.</summary>
	[JsonPropertyName("forwarderdata_use_ssl")]
	public bool? ForwarderDataUseSsl { get; init; }

	/// <summary>The node's GUID.</summary>
	[JsonPropertyName("guid")]
	public string? ClusterGuid { get; init; }

	/// <summary>How long, in seconds, the manager waits before it considers a peer down.</summary>
	[JsonPropertyName("heartbeat_timeout")]
	public int? HeartbeatTimeout { get; init; }

	/// <summary>The cluster manager redundancy switchover mode, for example <c>disabled</c> or <c>auto</c>.</summary>
	[JsonPropertyName("manager_switchover_mode")]
	public string? ManagerSwitchoverMode { get; init; }

	/// <summary>The cluster manager this peer or search head connects to (<c>?</c> when none).</summary>
	[JsonPropertyName("manager_uri")]
	public string? ManagerUri { get; init; }

	/// <summary>The number of jobs a peer may run at once that make buckets searchable.</summary>
	[JsonPropertyName("max_peer_build_load")]
	public int? MaxPeerBuildLoad { get; init; }

	/// <summary>The maximum number of replications a peer may receive at once.</summary>
	[JsonPropertyName("max_peer_rep_load")]
	public int? MaxPeerReplicationLoad { get; init; }

	/// <summary>The node's clustering mode.</summary>
	[JsonPropertyName("mode")]
	public ClusterMode Mode { get; init; }

	/// <summary>Whether the cluster is multisite.</summary>
	[JsonPropertyName("multisite")]
	public bool? Multisite { get; init; }

	/// <summary>How often, in seconds, a peer scans summary folders for updates.</summary>
	[JsonPropertyName("notify_scan_period")]
	public int? NotifyScanPeriod { get; init; }

	/// <summary>Internal: used between the manager and peers.</summary>
	[JsonPropertyName("ping_flag")]
	public bool? PingFlag { get; init; }

	/// <summary>How long, in seconds, the manager waits for peers to join after it starts.</summary>
	[JsonPropertyName("quiet_period")]
	public int? QuietPeriod { get; init; }

	/// <summary>The timeout, in seconds, for receiving data between cluster nodes.</summary>
	[JsonPropertyName("rcv_timeout")]
	public int? ReceiveTimeout { get; init; }

	/// <summary>Reserved by Splunk.</summary>
	[JsonPropertyName("register_forwarder_address")]
	public string? RegisterForwarderAddress { get; init; }

	/// <summary>The address a peer advertises for replication.</summary>
	[JsonPropertyName("register_replication_address")]
	public string? RegisterReplicationAddress { get; init; }

	/// <summary>The address a peer advertises to search heads.</summary>
	[JsonPropertyName("register_search_address")]
	public string? RegisterSearchAddress { get; init; }

	/// <summary>The timeout, in seconds, for connecting to replicate data.</summary>
	[JsonPropertyName("rep_cxn_timeout")]
	public int? ReplicationConnectionTimeout { get; init; }

	/// <summary>The maximum cumulative time, in seconds, for receiving replication acknowledgements.</summary>
	[JsonPropertyName("rep_max_rcv_timeout")]
	public int? ReplicationMaxReceiveTimeout { get; init; }

	/// <summary>The maximum time, in seconds, for sending a replication slice.</summary>
	[JsonPropertyName("rep_max_send_timeout")]
	public int? ReplicationMaxSendTimeout { get; init; }

	/// <summary>The timeout, in seconds, for receiving replication data.</summary>
	[JsonPropertyName("rep_rcv_timeout")]
	public int? ReplicationReceiveTimeout { get; init; }

	/// <summary>The timeout, in seconds, for sending replication data.</summary>
	[JsonPropertyName("rep_send_timeout")]
	public int? ReplicationSendTimeout { get; init; }

	/// <summary>The number of copies of raw data the cluster keeps (manager only).</summary>
	[JsonPropertyName("replication_factor")]
	public int? ReplicationFactor { get; init; }

	/// <summary>The TCP port a peer listens on for replicated data.</summary>
	[JsonPropertyName("replication_port")]
	public int? ReplicationPort { get; init; }

	/// <summary>Whether replication uses TLS.</summary>
	[JsonPropertyName("replication_use_ssl")]
	public bool? ReplicationUseSsl { get; init; }

	/// <summary>How long, in seconds, the manager waits for a restarting peer before fixing its buckets.</summary>
	[JsonPropertyName("restart_timeout")]
	public int? RestartTimeout { get; init; }

	/// <summary>The number of searchable copies of each bucket the cluster keeps (manager only).</summary>
	[JsonPropertyName("search_factor")]
	public int? SearchFactor { get; init; }

	/// <summary>The shared cluster secret (<c>pass4SymmKey</c>), masked by Splunk.</summary>
	[JsonPropertyName("secret")]
	public string? Secret { get; init; }

	/// <summary>The timeout, in seconds, for sending data between cluster nodes.</summary>
	[JsonPropertyName("send_timeout")]
	public int? SendTimeout { get; init; }

	/// <summary>The site of a peer or search head in a multisite cluster, for example <c>site1</c>.</summary>
	[JsonPropertyName("site")]
	public string? Site { get; init; }

	/// <summary>The multisite replication factor, for example <c>origin:2,total:3</c>.</summary>
	[JsonPropertyName("site_replication_factor")]
	public string? SiteReplicationFactor { get; init; }

	/// <summary>The multisite search factor, for example <c>origin:1,total:2</c>.</summary>
	[JsonPropertyName("site_search_factor")]
	public string? SiteSearchFactor { get; init; }

	/// <summary>Whether summary replication is on.</summary>
	[JsonPropertyName("summary_replication")]
	public bool? SummaryReplication { get; init; }
}
