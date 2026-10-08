using System.Text.Json;
using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Cluster;

/// <summary>The progress of bundle validation, reload and rolling restart on the cluster manager.</summary>
public sealed class ClusterApplyBundleStatus
{
	/// <summary>The bundle operation status, for example <c>None</c>.</summary>
	[JsonPropertyName("status")]
	public string? Status { get; init; }

	/// <summary>Whether a bundle reload was issued.</summary>
	[JsonPropertyName("reload_bundle_issued")]
	public bool? ReloadBundleIssued { get; init; }

	/// <summary>Every property Splunk returned that this type does not model, keyed by its wire name.</summary>
	[JsonExtensionData]
	public IDictionary<string, JsonElement> AdditionalProperties { get; init; } = new Dictionary<string, JsonElement>();
}
