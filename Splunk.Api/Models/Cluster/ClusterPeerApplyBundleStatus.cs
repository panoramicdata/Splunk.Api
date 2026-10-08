using System.Text.Json;
using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Cluster;

/// <summary>The bundle state of a peer, as the manager sees it.</summary>
public sealed class ClusterPeerApplyBundleStatus
{
	/// <summary>The error of the last bundle reload, if any.</summary>
	[JsonPropertyName("reload_error")]
	public string? ReloadError { get; init; }

	/// <summary>Whether the peer must restart to apply the bundle.</summary>
	[JsonPropertyName("restart_required_for_apply_bundle")]
	public bool? RestartRequiredForApplyBundle { get; init; }

	/// <summary>Every property Splunk returned that this type does not model, keyed by its wire name.</summary>
	[JsonExtensionData]
	public IDictionary<string, JsonElement> AdditionalProperties { get; init; } = new Dictionary<string, JsonElement>();
}
