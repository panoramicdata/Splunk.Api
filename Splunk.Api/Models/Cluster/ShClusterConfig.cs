using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Cluster;

/// <summary>The search head clustering configuration of a node (<c>shcluster/config</c>).</summary>
/// <remarks><see cref="SplunkContent.Disabled"/> reports whether search head clustering is off. Splunk 10.6 reports an unset <c>heartbeat_period</c> as 18446744073709551615, so it stays in <see cref="SplunkContent.AdditionalProperties"/>.</remarks>
public sealed class ShClusterConfig : SplunkContent
{
	/// <summary>The mode: <c>disabled</c>, <c>member</c>, <c>captain</c> or <c>dynamic_captain</c> (several may be combined).</summary>
	[JsonPropertyName("mode")]
	public string? Mode { get; init; }

	/// <summary>The ID of the search head cluster.</summary>
	[JsonPropertyName("id")]
	public string? ClusterId { get; init; }

	/// <summary>The search head cluster label.</summary>
	[JsonPropertyName("shcluster_label")]
	public string? ShClusterLabel { get; init; }

	/// <summary>The deployer the member fetches configuration from.</summary>
	[JsonPropertyName("conf_deploy_fetch_url")]
	public string? ConfDeployFetchUrl { get; init; }

	/// <summary>Whether the captain is elected.</summary>
	[JsonPropertyName("dynamic_captain")]
	public bool? DynamicCaptain { get; init; }

	/// <summary>Whether the member prefers to be captain.</summary>
	[JsonPropertyName("preferred_captain")]
	public bool? PreferredCaptain { get; init; }

	/// <summary>Whether the member runs no scheduled searches.</summary>
	[JsonPropertyName("adhoc_searchhead")]
	public bool? AdhocSearchHead { get; init; }

	/// <summary>The member's manual detention state.</summary>
	[JsonPropertyName("manual_detention")]
	public ManualDetentionMode ManualDetention { get; init; }

	/// <summary>The rolling restart mode, for example <c>restart</c> or <c>searchable</c>.</summary>
	[JsonPropertyName("rolling_restart")]
	public string? RollingRestart { get; init; }

	/// <summary>How long, in seconds, a restarting member waits for running searches.</summary>
	[JsonPropertyName("decommission_search_jobs_wait_secs")]
	public int? DecommissionSearchJobsWaitSeconds { get; init; }

	/// <summary>The percentage of members restarted at once in a rolling restart.</summary>
	[JsonPropertyName("percent_peers_to_restart")]
	public int? PercentPeersToRestart { get; init; }

	/// <summary>The number of copies of each artifact.</summary>
	[JsonPropertyName("replication_factor")]
	public int? ReplicationFactor { get; init; }

	/// <summary>The member's replication port.</summary>
	[JsonPropertyName("replication_port")]
	public int? ReplicationPort { get; init; }

	/// <summary>Whether replication uses TLS.</summary>
	[JsonPropertyName("replication_use_ssl")]
	public bool? ReplicationUseSsl { get; init; }

	/// <summary>The address the member advertises for replication.</summary>
	[JsonPropertyName("register_replication_address")]
	public string? RegisterReplicationAddress { get; init; }

	/// <summary>The maximum number of replications a member may receive at once.</summary>
	[JsonPropertyName("max_peer_rep_load")]
	public int? MaxPeerReplicationLoad { get; init; }

	/// <summary>How long, in seconds, the captain waits before it considers a member down.</summary>
	[JsonPropertyName("heartbeat_timeout")]
	public int? HeartbeatTimeout { get; init; }

	/// <summary>How long, in seconds, the captain waits for members to join.</summary>
	[JsonPropertyName("quiet_period")]
	public int? QuietPeriod { get; init; }

	/// <summary>How long, in seconds, the captain waits for a restarting member.</summary>
	[JsonPropertyName("restart_timeout")]
	public int? RestartTimeout { get; init; }

	/// <summary>Internal: used between the captain and members.</summary>
	[JsonPropertyName("ping_flag")]
	public bool? PingFlag { get; init; }

	/// <summary>The shared secret (<c>pass4SymmKey</c>), masked by Splunk.</summary>
	[JsonPropertyName("secret")]
	public string? Secret { get; init; }

	/// <summary>The timeout, in seconds, for connecting between members.</summary>
	[JsonPropertyName("cxn_timeout")]
	public int? ConnectionTimeout { get; init; }

	/// <summary>The timeout, in seconds, for sending data between members.</summary>
	[JsonPropertyName("send_timeout")]
	public int? SendTimeout { get; init; }

	/// <summary>The timeout, in seconds, for receiving data between members.</summary>
	[JsonPropertyName("rcv_timeout")]
	public int? ReceiveTimeout { get; init; }

	/// <summary>The timeout, in seconds, for connecting to replicate data.</summary>
	[JsonPropertyName("rep_cxn_timeout")]
	public int? ReplicationConnectionTimeout { get; init; }

	/// <summary>The timeout, in seconds, for sending replication data.</summary>
	[JsonPropertyName("rep_send_timeout")]
	public int? ReplicationSendTimeout { get; init; }

	/// <summary>The timeout, in seconds, for receiving replication data.</summary>
	[JsonPropertyName("rep_rcv_timeout")]
	public int? ReplicationReceiveTimeout { get; init; }

	/// <summary>The maximum time, in seconds, for sending a replication slice.</summary>
	[JsonPropertyName("rep_max_send_timeout")]
	public int? ReplicationMaxSendTimeout { get; init; }

	/// <summary>The maximum cumulative time, in seconds, for receiving replication acknowledgements.</summary>
	[JsonPropertyName("rep_max_rcv_timeout")]
	public int? ReplicationMaxReceiveTimeout { get; init; }
}
