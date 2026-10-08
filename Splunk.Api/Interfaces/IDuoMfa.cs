using Refit;
using Splunk.Api.Models;
using Splunk.Api.Models.Access;

namespace Splunk.Api.Interfaces;

/// <summary>
/// Duo multifactor authentication configurations (<c>admin/Duo-MFA</c>). Every operation requires the
/// <c>change_authentication</c> capability. Disable any single sign-on (such as SAML) before enabling Duo for the first
/// time: Duo works only with local authentication.
/// </summary>
public interface IDuoMfa
{
	/// <summary>Lists the Duo configurations (<c>GET admin/Duo-MFA</c>).</summary>
	/// <param name="options">Paging and filtering, or <see langword="null"/> for Splunk's defaults.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>The configurations. With none, the feed is empty and carries a <c>WARN</c> message.</returns>
	[Get("services/admin/Duo-MFA")]
	Task<SplunkFeed<DuoMfaConfiguration>> ListAsync([Query] ListOptions? options, CancellationToken cancellationToken);

	/// <summary>Creates a Duo configuration (<c>POST admin/Duo-MFA</c>).</summary>
	/// <param name="request">The configuration.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>The created configuration.</returns>
	[Post("services/admin/Duo-MFA")]
	Task<SplunkFeed<DuoMfaConfiguration>> CreateAsync([Body] DuoMfaCreateRequest request, CancellationToken cancellationToken);

	/// <summary>Gets one Duo configuration (<c>GET admin/Duo-MFA/{name}</c>).</summary>
	/// <param name="name">The configuration stanza name.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with the one configuration.</returns>
	[Get("services/admin/Duo-MFA/{name}")]
	Task<SplunkFeed<DuoMfaConfiguration>> GetAsync(string name, CancellationToken cancellationToken);

	/// <summary>Updates a Duo configuration (<c>POST admin/Duo-MFA/{name}</c>).</summary>
	/// <param name="name">The configuration stanza name.</param>
	/// <param name="request">The settings to change.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>The updated configuration.</returns>
	[Post("services/admin/Duo-MFA/{name}")]
	Task<SplunkFeed<DuoMfaConfiguration>> UpdateAsync(string name, [Body] DuoMfaUpdateRequest request, CancellationToken cancellationToken);

	/// <summary>Deletes a Duo configuration (<c>DELETE admin/Duo-MFA/{name}</c>).</summary>
	/// <param name="name">The configuration stanza name.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A task that completes when the configuration is deleted.</returns>
	[Delete("services/admin/Duo-MFA/{name}")]
	Task DeleteAsync(string name, CancellationToken cancellationToken);
}
