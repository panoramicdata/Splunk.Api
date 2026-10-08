using Refit;
using Splunk.Api.Models;
using Splunk.Api.Models.Access;

namespace Splunk.Api.Interfaces;

/// <summary>
/// Mappings from identity provider groups to Splunk roles for OAuth 2.0 configurations (<c>admin/oauth2-groups</c>).
/// Reading requires <c>list_oauth_config_role_mappings</c>; creating requires <c>edit_oauth_config_role_mappings</c>.
/// The OAuth provider configuration (<see cref="IOAuth2Providers"/>) must exist first.
/// </summary>
public interface IOAuth2Groups
{
	/// <summary>Lists group-to-role mappings (<c>GET admin/oauth2-groups</c>).</summary>
	/// <param name="options">The configuration to filter by, paging and filtering; or <see langword="null"/>.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>The mappings.</returns>
	[Get("services/admin/oauth2-groups")]
	Task<SplunkFeed<OAuth2GroupMapping>> ListAsync([Query] OAuth2GroupListOptions? options, CancellationToken cancellationToken);

	/// <summary>Creates a group-to-role mapping (<c>POST admin/oauth2-groups</c>).</summary>
	/// <param name="request">The group, configuration and roles.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>The created mapping.</returns>
	[Post("services/admin/oauth2-groups")]
	Task<SplunkFeed<OAuth2GroupMapping>> CreateAsync([Body] OAuth2GroupCreateRequest request, CancellationToken cancellationToken);
}
