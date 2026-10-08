using Refit;
using Splunk.Api.Models.Topology;

namespace Splunk.Api.Interfaces;

/// <summary>
/// The Topology REST API (<c>stack-explainer/v1/...</c>): the deployment's nodes, their identities and their trusted
/// connections.
/// </summary>
/// <remarks>
/// These endpoints are served by the Splunk Topology sidecar, not splunkd: they answer plain JSON (not the feed
/// envelope), reject an <c>output_mode</c> parameter (the client sends none), and report errors as
/// <c>{"error":"..."}</c>, which <see cref="SplunkApiException"/> carries as its message. They need the
/// <c>list_topology</c> capability, and the sidecar does not run in FIPS mode. The reference's headings name them
/// <c>topology</c>, <c>node-identity</c> and <c>trusted-connections</c>; the real paths are under
/// <c>stack-explainer/v1/</c>.
/// </remarks>
public interface ITopology
{
	/// <summary>Gets the deployment topology: every managed node and its roles (<c>GET stack-explainer/v1/topology</c>).</summary>
	/// <remarks>Available only on the license manager; other nodes answer 403 "Access allowed only on license manager node".</remarks>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>The topology.</returns>
	[Get("services/stack-explainer/v1/topology")]
	Task<DeploymentTopology> GetAsync(CancellationToken cancellationToken);

	/// <summary>
	/// Gets the deployment topology including unmanaged actors, the nodes that send data to the deployment but cannot be
	/// verified (<c>GET stack-explainer/v1/topology?include_unmanaged_actors</c>).
	/// </summary>
	/// <remarks>
	/// <c>include_unmanaged_actors</c> is a flag: Splunk 10.6 rejects it with a value (<c>=true</c>) with HTTP 400. On a
	/// deployment with no unmanaged actors the response has no <c>unmanaged_actors</c> member, so
	/// <see cref="DeploymentTopology.UnmanagedActors"/> is empty.
	/// </remarks>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>The topology, with <see cref="DeploymentTopology.UnmanagedActors"/>.</returns>
	[Get("services/stack-explainer/v1/topology?include_unmanaged_actors")]
	Task<DeploymentTopology> GetWithUnmanagedActorsAsync(CancellationToken cancellationToken);

	/// <summary>Gets the identity of the node the client is connected to (<c>GET stack-explainer/v1/node-identity</c>).</summary>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>The node's identity.</returns>
	[Get("services/stack-explainer/v1/node-identity")]
	Task<NodeIdentity> GetNodeIdentityAsync(CancellationToken cancellationToken);

	/// <summary>
	/// Gets the identity of another node, through the license manager (<c>GET stack-explainer/v1/node-identity/{guid}</c>).
	/// </summary>
	/// <remarks>Available only on the license manager. An unknown GUID raises HTTP 404.</remarks>
	/// <param name="nodeGuid">The node's GUID, as the topology lists it.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>The node's identity.</returns>
	[Get("services/stack-explainer/v1/node-identity/{nodeGuid}")]
	Task<NodeIdentity> GetRemoteNodeIdentityAsync(string nodeGuid, CancellationToken cancellationToken);

	/// <summary>
	/// Gets the trusted connections of the node the client is connected to: HEC, S2S, TCP and UDP inputs and search
	/// peers (<c>GET stack-explainer/v1/trusted-connections</c>).
	/// </summary>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>The node's trusted connections.</returns>
	[Get("services/stack-explainer/v1/trusted-connections")]
	Task<TrustedConnections> GetTrustedConnectionsAsync(CancellationToken cancellationToken);

	/// <summary>
	/// Gets the trusted connections of another node, through the license manager
	/// (<c>GET stack-explainer/v1/trusted-connections/{guid}</c>).
	/// </summary>
	/// <remarks>Available only on the license manager. An unknown GUID raises HTTP 404.</remarks>
	/// <param name="nodeGuid">The node's GUID, as the topology lists it.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>The node's trusted connections.</returns>
	[Get("services/stack-explainer/v1/trusted-connections/{nodeGuid}")]
	Task<TrustedConnections> GetRemoteTrustedConnectionsAsync(string nodeGuid, CancellationToken cancellationToken);
}
