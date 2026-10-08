using Splunk.Api.Models.Access;
using Splunk.Api.Test.Support;

namespace Splunk.Api.Test.Groups;

public partial class SamlProvidersTests
{
	[Fact]
	public async Task ListAsync_SendsGet()
		=> (await EndpointRequests.SendAsync((c, ct) => c.SamlProviders.ListAsync(null, ct)))
			.ShouldBeEndpointRequest(HttpMethod.Get, "/services/authentication/providers/SAML");

	[Fact]
	public async Task CreateAsync_PostsTheConfiguration()
		=> (await EndpointRequests.SendAsync((c, ct) => c.SamlProviders.CreateAsync(
			new SamlConfigurationCreateRequest
			{
				Name = "saml",
				EntityId = "splunk",
				IdpSsoUrl = "https://idp/sso",
				IdpSloUrl = "https://idp/slo",
				IdpAttributeQueryUrl = "https://idp/aq",
				IdpCertPath = "/idp.pem",
				IdpMetadataFile = "/idp.xml",
				Fqdn = "https://lb",
				RedirectPort = 443,
				RedirectAfterLogoutToUrl = "https://bye",
				DefaultRoleIfMissing = "user",
				SignAuthnRequest = true,
				SignedAssertion = true,
				SignatureAlgorithm = "RSA-SHA256",
				NameIdFormat = "unspecified",
				SsoBinding = "HTTPPost",
				SloBinding = "HTTPRedirect",
				AttributeAliasMail = "mail",
				AttributeAliasRealName = "name",
				AttributeAliasRole = "role",
				AttributeQueryRequestSigned = false,
				AttributeQueryResponseSigned = false,
				AttributeQuerySoapUsername = "soap",
				AttributeQuerySoapPassword = "pw",
				AttributeQueryTtl = 3600,
				ErrorUrl = "https://err",
				ErrorUrlLabel = "Help",
				SslVerifyServerCert = true,
				SslVersions = "tls1.2",
				CaCertFile = "/ca.pem",
				SslKeysFile = "/key.pem",
				SslKeysFilePassword = "kp"
			},
			ct)))
			.ShouldBeEndpointRequest(
				HttpMethod.Post,
				"/services/authentication/providers/SAML",
				"name=saml&entityId=splunk&idpSSOUrl=https%3A%2F%2Fidp%2Fsso&idpSLOUrl=https%3A%2F%2Fidp%2Fslo&idpAttributeQueryUrl=https%3A%2F%2Fidp%2Faq"
				+ "&idpCertPath=%2Fidp.pem&idpMetadataFile=%2Fidp.xml&fqdn=https%3A%2F%2Flb&redirectPort=443&redirectAfterLogoutToUrl=https%3A%2F%2Fbye"
				+ "&defaultRoleIfMissing=user&signAuthnRequest=true&signedAssertion=true&signatureAlgorithm=RSA-SHA256&nameIdFormat=unspecified"
				+ "&ssoBinding=HTTPPost&sloBinding=HTTPRedirect&attributeAliasMail=mail&attributeAliasRealName=name&attributeAliasRole=role"
				+ "&attributeQueryRequestSigned=false&attributeQueryResponseSigned=false&attributeQuerySoapUsername=soap&attributeQuerySoapPassword=pw"
				+ "&attributeQueryTTL=3600&errorUrl=https%3A%2F%2Ferr&errorUrlLabel=Help&sslVerifyServerCert=true&sslVersions=tls1.2&caCertFile=%2Fca.pem"
				+ "&sslKeysfile=%2Fkey.pem&sslKeysfilePassword=kp");

	[Fact]
	public async Task GetAsync_SendsGetForTheName()
		=> (await EndpointRequests.SendAsync((c, ct) => c.SamlProviders.GetAsync("saml", ct)))
			.ShouldBeEndpointRequest(HttpMethod.Get, "/services/authentication/providers/SAML/saml");

	[Fact]
	public async Task UpdateAsync_PostsTheChanges()
		=> (await EndpointRequests.SendAsync((c, ct) => c.SamlProviders.UpdateAsync("saml", new SamlConfigurationUpdateRequest { EntityId = "splunk2", IdpSsoUrl = "https://sso" }, ct)))
			.ShouldBeEndpointRequest(HttpMethod.Post, "/services/authentication/providers/SAML/saml", "entityId=splunk2&idpSSOUrl=https%3A%2F%2Fsso");

	[Fact]
	public async Task EnableAsync_PostsToEnable()
		=> (await EndpointRequests.SendAsync((c, ct) => c.SamlProviders.EnableAsync("saml", ct)))
			.ShouldBeEndpointRequest(HttpMethod.Post, "/services/authentication/providers/SAML/saml/enable");

	[Fact]
	public async Task DisableAsync_PostsToDisable()
		=> (await EndpointRequests.SendAsync((c, ct) => c.SamlProviders.DisableAsync("saml", ct)))
			.ShouldBeEndpointRequest(HttpMethod.Post, "/services/authentication/providers/SAML/saml/disable");

	[Fact]
	public Task Error_RaisesSplunkApiException()
		=> EndpointRequests.ShouldRaiseSplunkErrorAsync((c, ct) => c.SamlProviders.GetAsync("missing", ct));
}
