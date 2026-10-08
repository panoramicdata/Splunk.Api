using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Cluster;

/// <summary>The peers to remove from the cluster (<c>POST cluster/manager/control/control/remove_peers</c>).</summary>
/// <remarks>Only peers whose status is <c>Down</c> or <c>GracefulShutdown</c> can be removed.</remarks>
public sealed class ClusterRemovePeersRequest : SplunkFormRequest
{
	/// <summary>The GUIDs of the peers, comma-separated.</summary>
	[JsonPropertyName("peers")]
	public required string Peers { get; init; }
}
