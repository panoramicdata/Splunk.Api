using Refit;
using Splunk.Api.Models;
using Splunk.Api.Models.Access;

namespace Splunk.Api.Interfaces;

/// <summary>
/// OAuth 2.0 identity provider configurations (<c>authentication/providers/oauth2</c>), used to exchange IdP tokens for
/// Splunk tokens (see <see cref="IOAuth2Tokens"/>). Reading requires <c>list_oauth_configs</c>; changing requires
/// <c>edit_oauth_configs</c>. Splunk stores them in <c>authentication.conf</c> under stanzas prefixed
/// <c>oauth_external_config_</c>.
/// </summary>
public interface IOAuth2Providers
{
	/// <summary>Lists the OAuth provider configurations (<c>GET authentication/providers/oauth2</c>).</summary>
	/// <param name="options">Paging and filtering, or <see langword="null"/> for Splunk's defaults.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>The configurations.</returns>
	[Get("services/authentication/providers/oauth2")]
	Task<SplunkFeed<OAuth2Provider>> ListAsync([Query] ListOptions? options, CancellationToken cancellationToken);

	/// <summary>Creates an OAuth provider configuration (<c>POST authentication/providers/oauth2</c>).</summary>
	/// <param name="request">The configuration.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>The created configuration.</returns>
	[Post("services/authentication/providers/oauth2")]
	Task<SplunkFeed<OAuth2Provider>> CreateAsync([Body] OAuth2ProviderCreateRequest request, CancellationToken cancellationToken);

	/// <summary>Gets one OAuth provider configuration (<c>GET authentication/providers/oauth2/{name}</c>).</summary>
	/// <param name="name">The configuration name.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with the one configuration.</returns>
	[Get("services/authentication/providers/oauth2/{name}")]
	Task<SplunkFeed<OAuth2Provider>> GetAsync(string name, CancellationToken cancellationToken);

	/// <summary>Updates an OAuth provider configuration (<c>POST authentication/providers/oauth2/{name}</c>).</summary>
	/// <param name="name">The configuration name.</param>
	/// <param name="request">The settings to change.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>The updated configuration.</returns>
	[Post("services/authentication/providers/oauth2/{name}")]
	Task<SplunkFeed<OAuth2Provider>> UpdateAsync(string name, [Body] OAuth2ProviderUpdateRequest request, CancellationToken cancellationToken);

	/// <summary>Deletes an OAuth provider configuration (<c>DELETE authentication/providers/oauth2/{name}</c>).</summary>
	/// <param name="name">The configuration name.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A task that completes when the configuration is deleted.</returns>
	[Delete("services/authentication/providers/oauth2/{name}")]
	Task DeleteAsync(string name, CancellationToken cancellationToken);
}
