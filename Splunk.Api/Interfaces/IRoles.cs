using Refit;
using Splunk.Api.Models;
using Splunk.Api.Models.Access;

namespace Splunk.Api.Interfaces;

/// <summary>Roles and their permissions (<c>authorization/roles</c>). Changing roles requires <c>edit_roles</c>.</summary>
public interface IRoles
{
	/// <summary>Lists the roles (<c>GET authorization/roles</c>).</summary>
	/// <param name="options">Paging and filtering, or <see langword="null"/> for Splunk's defaults.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>The roles.</returns>
	[Get("services/authorization/roles")]
	Task<SplunkFeed<Role>> ListAsync([Query] ListOptions? options, CancellationToken cancellationToken);

	/// <summary>Creates a role (<c>POST authorization/roles</c>).</summary>
	/// <param name="request">The role.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>The created role.</returns>
	[Post("services/authorization/roles")]
	Task<SplunkFeed<Role>> CreateAsync([Body] RoleCreateRequest request, CancellationToken cancellationToken);

	/// <summary>Gets one role (<c>GET authorization/roles/{name}</c>).</summary>
	/// <param name="name">The role name.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with the one role.</returns>
	[Get("services/authorization/roles/{name}")]
	Task<SplunkFeed<Role>> GetAsync(string name, CancellationToken cancellationToken);

	/// <summary>Updates a role (<c>POST authorization/roles/{name}</c>).</summary>
	/// <param name="name">The role name.</param>
	/// <param name="request">The settings to change. A list replaces the role's whole list.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>The updated role.</returns>
	[Post("services/authorization/roles/{name}")]
	Task<SplunkFeed<Role>> UpdateAsync(string name, [Body] RoleUpdateRequest request, CancellationToken cancellationToken);

	/// <summary>Deletes a role (<c>DELETE authorization/roles/{name}</c>).</summary>
	/// <param name="name">The role name.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A task that completes when the role is deleted.</returns>
	[Delete("services/authorization/roles/{name}")]
	Task DeleteAsync(string name, CancellationToken cancellationToken);
}
