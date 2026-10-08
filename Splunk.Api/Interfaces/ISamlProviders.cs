using Refit;
using Splunk.Api.Models;
using Splunk.Api.Models.Access;

namespace Splunk.Api.Interfaces;

/// <summary>
/// SAML single sign-on configurations (<c>authentication/providers/SAML</c>). Requires the
/// <c>change_authentication</c> capability. Splunk Enterprise only (on Splunk Cloud Platform, contact Support).
/// </summary>
public interface ISamlProviders
{
	/// <summary>Lists the SAML configurations (<c>GET authentication/providers/SAML</c>).</summary>
	/// <param name="options">Paging and filtering, or <see langword="null"/> for Splunk's defaults.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>The configurations.</returns>
	[Get("services/authentication/providers/SAML")]
	Task<SplunkFeed<SamlConfiguration>> ListAsync([Query] ListOptions? options, CancellationToken cancellationToken);

	/// <summary>Creates a SAML configuration (<c>POST authentication/providers/SAML</c>).</summary>
	/// <param name="request">The configuration.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>The created configuration.</returns>
	[Post("services/authentication/providers/SAML")]
	Task<SplunkFeed<SamlConfiguration>> CreateAsync([Body] SamlConfigurationCreateRequest request, CancellationToken cancellationToken);

	/// <summary>Gets one SAML configuration (<c>GET authentication/providers/SAML/{stanza_name}</c>).</summary>
	/// <param name="name">The configuration stanza name.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with the one configuration.</returns>
	[Get("services/authentication/providers/SAML/{name}")]
	Task<SplunkFeed<SamlConfiguration>> GetAsync(string name, CancellationToken cancellationToken);

	/// <summary>Updates a SAML configuration (<c>POST authentication/providers/SAML/{stanza_name}</c>).</summary>
	/// <param name="name">The configuration stanza name.</param>
	/// <param name="request">The settings to change.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>The updated configuration.</returns>
	[Post("services/authentication/providers/SAML/{name}")]
	Task<SplunkFeed<SamlConfiguration>> UpdateAsync(string name, [Body] SamlConfigurationUpdateRequest request, CancellationToken cancellationToken);

	/// <summary>Enables a SAML configuration (<c>POST authentication/providers/SAML/{stanza_name}/enable</c>).</summary>
	/// <param name="name">The configuration stanza name.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A task that completes when the configuration is enabled.</returns>
	[Post("services/authentication/providers/SAML/{name}/enable")]
	Task EnableAsync(string name, CancellationToken cancellationToken);

	/// <summary>Disables a SAML configuration (<c>POST authentication/providers/SAML/{stanza_name}/disable</c>).</summary>
	/// <param name="name">The configuration stanza name.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A task that completes when the configuration is disabled.</returns>
	/// <remarks>The reference's operation text for this path says "Delete a SAML strategy"; the path disables it.</remarks>
	[Post("services/authentication/providers/SAML/{name}/disable")]
	Task DisableAsync(string name, CancellationToken cancellationToken);
}
