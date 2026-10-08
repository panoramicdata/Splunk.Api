using Refit;
using Splunk.Api.Models;
using Splunk.Api.Models.Access;

namespace Splunk.Api.Interfaces;

/// <summary>
/// LDAP authentication strategies (<c>authentication/providers/LDAP</c>), the LDAP stanzas of
/// <c>authentication.conf</c>. Requires the <c>change_authentication</c> capability. Splunk Enterprise only (on Splunk
/// Cloud Platform, contact Support).
/// </summary>
public interface ILdapStrategies
{
	/// <summary>Lists the LDAP strategies (<c>GET authentication/providers/LDAP</c>).</summary>
	/// <param name="options">The strategy to filter by, paging and filtering; or <see langword="null"/>.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>The strategies.</returns>
	[Get("services/authentication/providers/LDAP")]
	Task<SplunkFeed<LdapStrategy>> ListAsync([Query] LdapStrategyListOptions? options, CancellationToken cancellationToken);

	/// <summary>Creates an LDAP strategy (<c>POST authentication/providers/LDAP</c>).</summary>
	/// <param name="request">The strategy settings.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>The created strategy.</returns>
	[Post("services/authentication/providers/LDAP")]
	Task<SplunkFeed<LdapStrategy>> CreateAsync([Body] LdapStrategyCreateRequest request, CancellationToken cancellationToken);

	/// <summary>Updates an LDAP strategy (<c>POST authentication/providers/LDAP/{LDAP_strategy_name}</c>).</summary>
	/// <param name="name">The strategy name.</param>
	/// <param name="request">The settings to change.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>The updated strategy.</returns>
	[Post("services/authentication/providers/LDAP/{name}")]
	Task<SplunkFeed<LdapStrategy>> UpdateAsync(string name, [Body] LdapStrategyUpdateRequest request, CancellationToken cancellationToken);

	/// <summary>Deletes an LDAP strategy (<c>DELETE authentication/providers/LDAP/{LDAP_strategy_name}</c>).</summary>
	/// <param name="name">The strategy name.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A task that completes when the strategy is deleted.</returns>
	[Delete("services/authentication/providers/LDAP/{name}")]
	Task DeleteAsync(string name, CancellationToken cancellationToken);

	/// <summary>Enables an LDAP strategy (<c>POST authentication/providers/LDAP/{LDAP_strategy_name}/enable</c>).</summary>
	/// <param name="name">The strategy name.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A task that completes when the strategy is enabled.</returns>
	[Post("services/authentication/providers/LDAP/{name}/enable")]
	Task EnableAsync(string name, CancellationToken cancellationToken);

	/// <summary>Disables an LDAP strategy (<c>POST authentication/providers/LDAP/{LDAP_strategy_name}/disable</c>).</summary>
	/// <param name="name">The strategy name.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A task that completes when the strategy is disabled.</returns>
	[Post("services/authentication/providers/LDAP/{name}/disable")]
	Task DisableAsync(string name, CancellationToken cancellationToken);
}
