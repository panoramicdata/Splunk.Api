using Refit;
using Splunk.Api.Models;
using Splunk.Api.Models.Access;

namespace Splunk.Api.Interfaces;

/// <summary>
/// Mappings from external SAML groups (in an identity provider's response) to Splunk roles (<c>admin/SAML-groups</c>).
/// Requires the <c>change_authentication</c> capability.
/// </summary>
public interface ISamlGroups
{
	/// <summary>Lists the group mappings (<c>GET admin/SAML-groups</c>).</summary>
	/// <param name="options">Paging and filtering, or <see langword="null"/> for Splunk's defaults.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>Each external group, with the roles mapped to it.</returns>
	[Get("services/admin/SAML-groups")]
	Task<SplunkFeed<RoleMapping>> ListAsync([Query] ListOptions? options, CancellationToken cancellationToken);

	/// <summary>Maps an external group to roles (<c>POST admin/SAML-groups</c>).</summary>
	/// <param name="request">The group name and roles.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>An empty feed: Splunk does not echo the mapping.</returns>
	[Post("services/admin/SAML-groups")]
	Task<SplunkFeed<RoleMapping>> CreateAsync([Body] RoleMappingCreateRequest request, CancellationToken cancellationToken);

	/// <summary>Deletes a group mapping (<c>DELETE admin/SAML-groups/{group_name}</c>).</summary>
	/// <param name="name">The group name.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A task that completes when the mapping is deleted.</returns>
	[Delete("services/admin/SAML-groups/{name}")]
	Task DeleteAsync(string name, CancellationToken cancellationToken);
}
