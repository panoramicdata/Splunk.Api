using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Cluster;

/// <summary>Options of re-adding a peer to the manager (<c>POST cluster/peer/control/control/re-add-peer</c>).</summary>
public sealed class ClusterPeerReAddRequest : SplunkFormRequest
{
	/// <summary>Whether the manager reassigns all primary copies (<c>true</c>, the default) or keeps the peer's.</summary>
	[JsonPropertyName("clearMasks")]
	public bool? ClearMasks { get; init; }
}
