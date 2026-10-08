using Refit;
using Splunk.Api.Models;
using Splunk.Api.Models.Access;

namespace Splunk.Api.Interfaces;

/// <summary>
/// ProxySSO group to role mappings (<c>admin/ProxySSO-groups</c>). Requires the <c>change_authentication</c>
/// capability. Changes are written to the app context.
/// </summary>
public interface IProxySsoGroups
{
	/// <summary>Lists the group mappings (<c>GET admin/ProxySSO-groups</c>).</summary>
	/// <param name="options">Paging and filtering, or <see langword="null"/> for Splunk's defaults.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>Each group, with the roles mapped to it.</returns>
	[Get("services/admin/ProxySSO-groups")]
	Task<SplunkFeed<RoleMapping>> ListAsync([Query] ListOptions? options, CancellationToken cancellationToken);

	/// <summary>Maps a group to roles (<c>POST admin/ProxySSO-groups</c>).</summary>
	/// <param name="request">The group name and roles.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>An empty feed: Splunk does not echo the mapping.</returns>
	[Post("services/admin/ProxySSO-groups")]
	Task<SplunkFeed<RoleMapping>> CreateAsync([Body] RoleMappingCreateRequest request, CancellationToken cancellationToken);

	/// <summary>Gets the roles mapped to one group (<c>GET admin/ProxySSO-groups/{group_name}</c>).</summary>
	/// <param name="name">The group name.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with the one mapping.</returns>
	[Get("services/admin/ProxySSO-groups/{name}")]
	Task<SplunkFeed<RoleMapping>> GetAsync(string name, CancellationToken cancellationToken);

	/// <summary>Creates or replaces a group's mapping (<c>POST admin/ProxySSO-groups/{group_name}</c>).</summary>
	/// <param name="name">The group name.</param>
	/// <param name="request">The roles.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>An empty feed: Splunk does not echo the mapping.</returns>
	[Post("services/admin/ProxySSO-groups/{name}")]
	Task<SplunkFeed<RoleMapping>> UpdateAsync(string name, [Body] RoleMappingUpdateRequest request, CancellationToken cancellationToken);

	/// <summary>Deletes a group's mapping (<c>DELETE admin/ProxySSO-groups/{group_name}</c>).</summary>
	/// <param name="name">The group name.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A task that completes when the mapping is deleted.</returns>
	[Delete("services/admin/ProxySSO-groups/{name}")]
	Task DeleteAsync(string name, CancellationToken cancellationToken);
}
