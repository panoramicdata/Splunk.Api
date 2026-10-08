using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Access;

/// <summary>
/// A SAML single sign-on configuration (<c>authentication/providers/SAML</c>). The common settings are modelled; every
/// other key is in <see cref="SplunkContent.AdditionalProperties"/>.
/// </summary>
public sealed class SamlConfiguration : SplunkContent
{
	/// <summary>The endpoint where the identity provider posts SAML assertions.</summary>
	[JsonPropertyName("assertionConsumerServiceUrl")]
	public string? AssertionConsumerServiceUrl { get; init; }

	/// <summary>The SAML attribute mapped to the email address.</summary>
	[JsonPropertyName("attributeAliasMail")]
	public string? AttributeAliasMail { get; init; }

	/// <summary>The SAML attribute mapped to the real name.</summary>
	[JsonPropertyName("attributeAliasRealName")]
	public string? AttributeAliasRealName { get; init; }

	/// <summary>The SAML attribute mapped to roles.</summary>
	[JsonPropertyName("attributeAliasRole")]
	public string? AttributeAliasRole { get; init; }

	/// <summary>Whether attribute queries are signed.</summary>
	[JsonPropertyName("attributeQueryRequestSigned")]
	public bool? AttributeQueryRequestSigned { get; init; }

	/// <summary>Whether attribute query responses must be signed.</summary>
	[JsonPropertyName("attributeQueryResponseSigned")]
	public bool? AttributeQueryResponseSigned { get; init; }

	/// <summary>The user name for attribute queries over SOAP.</summary>
	[JsonPropertyName("attributeQuerySoapUsername")]
	public string? AttributeQuerySoapUsername { get; init; }

	/// <summary>How long, in seconds, attribute query results are cached.</summary>
	[JsonPropertyName("attributeQueryTTL")]
	public int? AttributeQueryTtl { get; init; }

	/// <summary>The path of the CA certificate.</summary>
	[JsonPropertyName("caCertFile")]
	public string? CaCertFile { get; init; }

	/// <summary>The role to use when a SAML response carries none.</summary>
	[JsonPropertyName("defaultRoleIfMissing")]
	public string? DefaultRoleIfMissing { get; init; }

	/// <summary>The entity ID preconfigured by the identity provider.</summary>
	[JsonPropertyName("entityId")]
	public string? EntityId { get; init; }

	/// <summary>The URL shown for a SAML error.</summary>
	[JsonPropertyName("errorUrl")]
	public string? ErrorUrl { get; init; }

	/// <summary>The label of <see cref="ErrorUrl"/>.</summary>
	[JsonPropertyName("errorUrlLabel")]
	public string? ErrorUrlLabel { get; init; }

	/// <summary>The load balancer URL.</summary>
	[JsonPropertyName("fqdn")]
	public string? Fqdn { get; init; }

	/// <summary>The identity provider URL that attribute queries are sent to.</summary>
	[JsonPropertyName("idpAttributeQueryUrl")]
	public string? IdpAttributeQueryUrl { get; init; }

	/// <summary>The path of the identity provider's certificate.</summary>
	[JsonPropertyName("idpCertPath")]
	public string? IdpCertPath { get; init; }

	/// <summary>The identity provider URL that SAML single logout requests are sent to.</summary>
	[JsonPropertyName("idpSLOUrl")]
	public string? IdpSloUrl { get; init; }

	/// <summary>The identity provider URL that SAML single sign-on requests are sent to.</summary>
	[JsonPropertyName("idpSSOUrl")]
	public string? IdpSsoUrl { get; init; }

	/// <summary>The maximum number of attribute query jobs to queue.</summary>
	[JsonPropertyName("maxAttributeQueryQueueSize")]
	public int? MaxAttributeQueryQueueSize { get; init; }

	/// <summary>The maximum number of threads for asynchronous attribute queries.</summary>
	[JsonPropertyName("maxAttributeQueryThreads")]
	public int? MaxAttributeQueryThreads { get; init; }

	/// <summary>How the subject is identified in an assertion.</summary>
	[JsonPropertyName("nameIdFormat")]
	public string? NameIdFormat { get; init; }

	/// <summary>Where users go after logging out when no single logout URL is configured.</summary>
	[JsonPropertyName("redirectAfterLogoutToUrl")]
	public string? RedirectAfterLogoutToUrl { get; init; }

	/// <summary>The port SAML responses are sent to, when it differs from the Splunk Web port.</summary>
	[JsonPropertyName("redirectPort")]
	public int? RedirectPort { get; init; }

	/// <summary>Whether authentication requests are signed.</summary>
	[JsonPropertyName("signAuthnRequest")]
	public bool? SignAuthnRequest { get; init; }

	/// <summary>The signature algorithm for redirect-binding requests, for example <c>RSA-SHA256</c>.</summary>
	[JsonPropertyName("signatureAlgorithm")]
	public string? SignatureAlgorithm { get; init; }

	/// <summary>Whether SAML assertions must be signed.</summary>
	[JsonPropertyName("signedAssertion")]
	public bool? SignedAssertion { get; init; }

	/// <summary>The URL where the identity provider posts single logout responses.</summary>
	[JsonPropertyName("singleLogoutServiceUrl")]
	public string? SingleLogoutServiceUrl { get; init; }

	/// <summary>The binding of logout requests and responses: <c>HTTPPost</c> or <c>HTTPRedirect</c>.</summary>
	[JsonPropertyName("sloBinding")]
	public string? SloBinding { get; init; }

	/// <summary>The service provider certificate path.</summary>
	[JsonPropertyName("spCertPath")]
	public string? SpCertPath { get; init; }

	/// <summary>Whether certificates are verified when making attribute queries.</summary>
	[JsonPropertyName("sslVerifyServerCert")]
	public bool? SslVerifyServerCert { get; init; }

	/// <summary>The SSL/TLS versions used for attribute queries.</summary>
	[JsonPropertyName("sslVersions")]
	public string? SslVersions { get; init; }

	/// <summary>The binding of service-provider-initiated requests: <c>HTTPPost</c> or <c>HTTPRedirect</c>.</summary>
	[JsonPropertyName("ssoBinding")]
	public string? SsoBinding { get; init; }
}
