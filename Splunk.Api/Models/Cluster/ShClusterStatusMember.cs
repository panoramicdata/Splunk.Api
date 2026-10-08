using System.Text.Json;
using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Cluster;

/// <summary>A member in the search head cluster status.</summary>
public sealed class ShClusterStatusMember
{
	/// <summary>The member's label.</summary>
	[JsonPropertyName("label")]
	public string? Label { get; init; }

	/// <summary>The member status.</summary>
	[JsonPropertyName("status")]
	public ClusterPeerStatus Status { get; init; }

	/// <summary>The member's management URI.</summary>
	[JsonPropertyName("mgmt_uri")]
	public string? ManagementUri { get; init; }

	/// <summary>The member's alternative management URI.</summary>
	[JsonPropertyName("mgmt_uri_alias")]
	public string? ManagementUriAlias { get; init; }

	/// <summary>When the member last pulled configuration from the captain, as Splunk formats it.</summary>
	[JsonPropertyName("last_conf_replication")]
	public string? LastConfReplication { get; init; }

	/// <summary>The member's manual detention state.</summary>
	[JsonPropertyName("manual_detention")]
	public ManualDetentionMode ManualDetention { get; init; }

	/// <summary>Whether the member is out of sync.</summary>
	[JsonPropertyName("out_of_sync_node")]
	public bool? OutOfSyncNode { get; init; }

	/// <summary>Whether the member prefers to be captain.</summary>
	[JsonPropertyName("preferred_captain")]
	public bool? PreferredCaptain { get; init; }

	/// <summary>Whether the member asked for a restart.</summary>
	[JsonPropertyName("restart_required")]
	public bool? RestartRequired { get; init; }

	/// <summary>The member's Splunk version.</summary>
	[JsonPropertyName("splunk_version")]
	public string? SplunkVersion { get; init; }

	/// <summary>Every property Splunk returned that this type does not model, keyed by its wire name.</summary>
	[JsonExtensionData]
	public IDictionary<string, JsonElement> AdditionalProperties { get; init; } = new Dictionary<string, JsonElement>();
}
