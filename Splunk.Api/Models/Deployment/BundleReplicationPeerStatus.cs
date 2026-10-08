using System.Text.Json;
using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Deployment;

/// <summary>The replication state of a knowledge bundle on one search peer.</summary>
public sealed class BundleReplicationPeerStatus
{
	/// <summary>The search peer.</summary>
	[JsonPropertyName("peer_name")]
	public string? PeerName { get; init; }

	/// <summary>The classic replication state, for example <c>succeeded</c>.</summary>
	[JsonPropertyName("classic_replication_state")]
	public string? ClassicReplicationState { get; init; }

	/// <summary>Every property Splunk returned that this type does not model, keyed by its wire name.</summary>
	[JsonExtensionData]
	public IDictionary<string, JsonElement> AdditionalProperties { get; init; } = new Dictionary<string, JsonElement>();
}
