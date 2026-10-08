using Refit;
using Splunk.Api.Models;
using Splunk.Api.Models.Access;

namespace Splunk.Api.Interfaces;

/// <summary>
/// ProxySSO user to role mappings (<c>admin/ProxySSO-user-role-map</c>). Requires the <c>edit_user</c> capability.
/// Changes are written to <c>etc/system/local</c>. Creating and deleting mappings needs a configured ProxySSO manager;
/// without one, Splunk answers 400 <c>Proxy SSO Manager not configured.</c>
/// </summary>
public interface IProxySsoUserRoleMaps
{
	/// <summary>Lists the user mappings (<c>GET admin/ProxySSO-user-role-map</c>).</summary>
	/// <param name="options">Paging and filtering, or <see langword="null"/> for Splunk's defaults.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>Each user, with the roles mapped to it.</returns>
	[Get("services/admin/ProxySSO-user-role-map")]
	Task<SplunkFeed<RoleMapping>> ListAsync([Query] ListOptions? options, CancellationToken cancellationToken);

	/// <summary>Maps a user to roles (<c>POST admin/ProxySSO-user-role-map</c>).</summary>
	/// <param name="request">The user name and roles.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>The mappings.</returns>
	[Post("services/admin/ProxySSO-user-role-map")]
	Task<SplunkFeed<RoleMapping>> CreateAsync([Body] RoleMappingCreateRequest request, CancellationToken cancellationToken);

	/// <summary>Gets the roles mapped to one user (<c>GET admin/ProxySSO-user-role-map/{user_name}</c>).</summary>
	/// <param name="name">The user name.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with the one mapping.</returns>
	[Get("services/admin/ProxySSO-user-role-map/{name}")]
	Task<SplunkFeed<RoleMapping>> GetAsync(string name, CancellationToken cancellationToken);

	/// <summary>Deletes a user's mapping (<c>DELETE admin/ProxySSO-user-role-map/{user_name}</c>).</summary>
	/// <param name="name">The user name.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A task that completes when the mapping is deleted.</returns>
	[Delete("services/admin/ProxySSO-user-role-map/{name}")]
	Task DeleteAsync(string name, CancellationToken cancellationToken);
}
