using Refit;
using Splunk.Api.Models;
using Splunk.Api.Models.Cluster;

namespace Splunk.Api.Interfaces;

/// <summary>This search head cluster member: its state, artifacts, consensus and detention (<c>shcluster/member</c>).</summary>
/// <remarks>Call these on a member; a node without search head clustering answers HTTP 503 (HTTP 400 for <c>consensus</c>).</remarks>
public interface IShClusterMember
{
	/// <summary>Gets the member's state (<c>GET shcluster/member/info</c>).</summary>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with one entry, <c>member</c>.</returns>
	[Get("services/shcluster/member/info")]
	Task<SplunkFeed<ShClusterMemberInfo>> GetInfoAsync(CancellationToken cancellationToken);

	/// <summary>Lists the member's search artifacts (<c>GET shcluster/member/artifacts</c>).</summary>
	/// <param name="options">Paging, filtering and sorting, or <see langword="null"/>.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with one entry per artifact.</returns>
	[Get("services/shcluster/member/artifacts")]
	Task<SplunkFeed<ShClusterMemberArtifact>> ListArtifactsAsync([Query] ListOptions? options, CancellationToken cancellationToken);

	/// <summary>Gets one of the member's search artifacts (<c>GET shcluster/member/artifacts/{name}</c>).</summary>
	/// <param name="name">The artifact's search ID.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with the artifact.</returns>
	[Get("services/shcluster/member/artifacts/{name}")]
	Task<SplunkFeed<ShClusterMemberArtifact>> GetArtifactAsync(string name, CancellationToken cancellationToken);

	/// <summary>
	/// Gets the latest cluster configuration agreed by Raft consensus (<c>GET shcluster/member/consensus</c>).
	/// </summary>
	/// <remarks>Without search head clustering this answers HTTP 400 "... Raft REST endpoints are not available!".</remarks>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with one entry, <c>shc_cluster_configuration</c>.</returns>
	[Get("services/shcluster/member/consensus")]
	Task<SplunkFeed<ShClusterConsensus>> GetConsensusAsync(CancellationToken cancellationToken);

	/// <summary>
	/// Puts the member into manual detention, where it takes no new searches, or takes it out
	/// (<c>POST shcluster/member/control/control/set_manual_detention</c>).
	/// </summary>
	/// <remarks>Search head cluster members accept only <see cref="ManualDetentionMode.On"/> and <see cref="ManualDetentionMode.Off"/>.</remarks>
	/// <param name="request">The detention state.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>The feed Splunk answers with.</returns>
	[Post("services/shcluster/member/control/control/set_manual_detention")]
	Task<SplunkFeed<SplunkDynamicContent>> SetManualDetentionAsync([Body] ManualDetentionRequest request, CancellationToken cancellationToken);
}
