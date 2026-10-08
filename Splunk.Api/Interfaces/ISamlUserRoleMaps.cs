using Refit;
using Splunk.Api.Models;
using Splunk.Api.Models.Access;

namespace Splunk.Api.Interfaces;

/// <summary>
/// SAML users and their roles, for running saved searches when the identity provider does not support attribute query
/// requests (<c>admin/SAML-user-role-map</c>). Requires the <c>edit_user</c> capability.
/// </summary>
public interface ISamlUserRoleMaps
{
	/// <summary>Lists the SAML users (<c>GET admin/SAML-user-role-map</c>).</summary>
	/// <param name="options">Paging and filtering, or <see langword="null"/> for Splunk's defaults.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>Each user, with the roles assigned to it.</returns>
	[Get("services/admin/SAML-user-role-map")]
	Task<SplunkFeed<RoleMapping>> ListAsync([Query] ListOptions? options, CancellationToken cancellationToken);

	/// <summary>Adds or updates a SAML user and its roles (<c>POST admin/SAML-user-role-map</c>).</summary>
	/// <param name="request">The user name and roles.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>The user's mapping.</returns>
	/// <remarks>
	/// Splunk looks the user up first; for a user it has no cached information about, a live Splunk 10.6 answers 400
	/// <c>Failed to get cachedUserInfo for user=...</c>.
	/// </remarks>
	[Post("services/admin/SAML-user-role-map")]
	Task<SplunkFeed<RoleMapping>> CreateAsync([Body] RoleMappingCreateRequest request, CancellationToken cancellationToken);

	/// <summary>The collection DELETE the reference lists (<c>DELETE admin/SAML-user-role-map</c>).</summary>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A task that completes if Splunk accepts the request.</returns>
	/// <remarks>
	/// The reference's section for this operation says only "See admin/SAML-user-role-map/{name}". A live Splunk 10.6
	/// rejects it with 400 <c>Cannot perform action "DELETE" without a target name to act on.</c>: use
	/// <see cref="DeleteAsync(string, CancellationToken)"/> to remove a user.
	/// </remarks>
	[Delete("services/admin/SAML-user-role-map")]
	Task DeleteAllAsync(CancellationToken cancellationToken);

	/// <summary>Removes a SAML user (<c>DELETE admin/SAML-user-role-map/{name}</c>).</summary>
	/// <param name="name">The SAML user name.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A task that completes when the user is removed.</returns>
	[Delete("services/admin/SAML-user-role-map/{name}")]
	Task DeleteAsync(string name, CancellationToken cancellationToken);
}
