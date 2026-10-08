using Splunk.Api.Interfaces;

namespace Splunk.Api;

public sealed partial class SplunkClient
{
	/// <summary>Logging in and the active sessions (<c>auth/login</c> and <c>authentication/httpauth-tokens</c>).</summary>
	public ISessions Sessions => field ??= For<ISessions>();

	/// <summary>The authenticated user (<c>authentication/current-context</c>).</summary>
	public ICurrentContext CurrentContext => field ??= For<ICurrentContext>();

	/// <summary>Users (<c>authentication/users</c>).</summary>
	public IUsers Users => field ??= For<IUsers>();

	/// <summary>Roles (<c>authorization/roles</c>).</summary>
	public IRoles Roles => field ??= For<IRoles>();

	/// <summary>Capabilities (<c>authorization/capabilities</c> and <c>authorization/grantable_capabilities</c>).</summary>
	public ICapabilities Capabilities => field ??= For<ICapabilities>();

	/// <summary>Authentication (bearer) tokens (<c>authorization/tokens</c>).</summary>
	public IAuthenticationTokens AuthenticationTokens => field ??= For<IAuthenticationTokens>();

	/// <summary>Field filters (<c>authorization/fieldfilters</c>).</summary>
	public IFieldFilters FieldFilters => field ??= For<IFieldFilters>();

	/// <summary>Stored credentials (<c>storage/passwords</c>). Reads return passwords in clear text.</summary>
	public IStoragePasswords StoragePasswords => field ??= For<IStoragePasswords>();

	/// <summary>Duo multifactor authentication (<c>admin/Duo-MFA</c>).</summary>
	public IDuoMfa DuoMfa => field ??= For<IDuoMfa>();

	/// <summary>RSA multifactor authentication (<c>admin/Rsa-MFA</c>).</summary>
	public IRsaMfa RsaMfa => field ??= For<IRsaMfa>();

	/// <summary>LDAP authentication strategies (<c>authentication/providers/LDAP</c>).</summary>
	public ILdapStrategies LdapStrategies => field ??= For<ILdapStrategies>();

	/// <summary>LDAP group to role mappings (<c>admin/LDAP-groups</c>).</summary>
	public ILdapGroups LdapGroups => field ??= For<ILdapGroups>();

	/// <summary>SAML configurations (<c>authentication/providers/SAML</c>).</summary>
	public ISamlProviders SamlProviders => field ??= For<ISamlProviders>();

	/// <summary>SAML group to role mappings (<c>admin/SAML-groups</c>).</summary>
	public ISamlGroups SamlGroups => field ??= For<ISamlGroups>();

	/// <summary>SAML users for saved searches (<c>admin/SAML-user-role-map</c>).</summary>
	public ISamlUserRoleMaps SamlUserRoleMaps => field ??= For<ISamlUserRoleMaps>();

	/// <summary>SAML metadata and certificate replication (<c>admin/SAML-idp-metadata</c>, <c>admin/SAML-sp-metadata</c>, <c>admin/replicate-SAML-certs</c>).</summary>
	public ISamlMetadata SamlMetadata => field ??= For<ISamlMetadata>();

	/// <summary>ProxySSO configurations (<c>admin/ProxySSO-auth</c>).</summary>
	public IProxySsoConfigurations ProxySsoConfigurations => field ??= For<IProxySsoConfigurations>();

	/// <summary>ProxySSO group to role mappings (<c>admin/ProxySSO-groups</c>).</summary>
	public IProxySsoGroups ProxySsoGroups => field ??= For<IProxySsoGroups>();

	/// <summary>ProxySSO user to role mappings (<c>admin/ProxySSO-user-role-map</c>).</summary>
	public IProxySsoUserRoleMaps ProxySsoUserRoleMaps => field ??= For<IProxySsoUserRoleMaps>();

	/// <summary>OAuth 2.0 identity provider configurations (<c>authentication/providers/oauth2</c>).</summary>
	public IOAuth2Providers OAuth2Providers => field ??= For<IOAuth2Providers>();

	/// <summary>OAuth 2.0 group to role mappings (<c>admin/oauth2-groups</c>).</summary>
	public IOAuth2Groups OAuth2Groups => field ??= For<IOAuth2Groups>();

	/// <summary>OAuth 2.0 token exchange (<c>oauth2/v1/token</c>).</summary>
	public IOAuth2Tokens OAuth2Tokens => field ??= For<IOAuth2Tokens>();

	/// <summary>The metrics processor (<c>admin/metrics-reload</c>).</summary>
	public IMetricsProcessor MetricsProcessor => field ??= For<IMetricsProcessor>();
}
