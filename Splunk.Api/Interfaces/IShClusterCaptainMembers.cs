using Refit;
using Splunk.Api.Models;
using Splunk.Api.Models.Cluster;

namespace Splunk.Api.Interfaces;

/// <summary>The search head cluster members, as the captain sees them (<c>shcluster/captain/members</c>).</summary>
/// <remarks>Call these on the captain; a node without search head clustering answers HTTP 503.</remarks>
public interface IShClusterCaptainMembers
{
	/// <summary>Lists the members (<c>GET shcluster/captain/members</c>).</summary>
	/// <param name="options">Paging, filtering and sorting, or <see langword="null"/>.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with one entry per member, named by GUID.</returns>
	[Get("services/shcluster/captain/members")]
	Task<SplunkFeed<ShClusterMember>> ListAsync([Query] ListOptions? options, CancellationToken cancellationToken);

	/// <summary>Gets a member (<c>GET shcluster/captain/members/{name}</c>).</summary>
	/// <param name="name">The member GUID.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with the member.</returns>
	[Get("services/shcluster/captain/members/{name}")]
	Task<SplunkFeed<ShClusterMember>> GetAsync(string name, CancellationToken cancellationToken);
}
