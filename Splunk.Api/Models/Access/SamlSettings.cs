using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Access;

/// <summary>
/// The common settings of a SAML configuration, shared by <see cref="SamlConfigurationCreateRequest"/> and
/// <see cref="SamlConfigurationUpdateRequest"/>. Set any other <c>authentication.conf</c> SAML key in
/// <see cref="SplunkFormRequest.AdditionalParameters"/>.
/// </summary>
public abstract class SamlSettings : SplunkFormRequest
{
	/// <summary>The identity provider URL that SAML single sign-on requests are sent to.</summary>
	[JsonPropertyName("idpSSOUrl")]
	public string? IdpSsoUrl { get; init; }

	/// <summary>The identity provider URL that SAML single logout requests are sent to.</summary>
	[JsonPropertyName("idpSLOUrl")]
	public string? IdpSloUrl { get; init; }

	/// <summary>The identity provider URL that attribute queries are sent to.</summary>
	[JsonPropertyName("idpAttributeQueryUrl")]
	public string? IdpAttributeQueryUrl { get; init; }

	/// <summary>The path of the identity provider's certificate.</summary>
	[JsonPropertyName("idpCertPath")]
	public string? IdpCertPath { get; init; }

	/// <summary>The full path, on the Splunk server, of identity provider metadata to read the URLs and certificate from.</summary>
	[JsonPropertyName("idpMetadataFile")]
	public string? IdpMetadataFile { get; init; }

	/// <summary>The load balancer URL.</summary>
	[JsonPropertyName("fqdn")]
	public string? Fqdn { get; init; }

	/// <summary>The port SAML responses are sent to, when it differs from the Splunk Web port.</summary>
	[JsonPropertyName("redirectPort")]
	public int? RedirectPort { get; init; }

	/// <summary>Where users go after logging out when no single logout URL is configured.</summary>
	[JsonPropertyName("redirectAfterLogoutToUrl")]
	public string? RedirectAfterLogoutToUrl { get; init; }

	/// <summary>The role to use when a SAML response carries none.</summary>
	[JsonPropertyName("defaultRoleIfMissing")]
	public string? DefaultRoleIfMissing { get; init; }

	/// <summary>Whether to sign authentication requests.</summary>
	[JsonPropertyName("signAuthnRequest")]
	public bool? SignAuthnRequest { get; init; }

	/// <summary>Whether SAML assertions must be signed.</summary>
	[JsonPropertyName("signedAssertion")]
	public bool? SignedAssertion { get; init; }

	/// <summary>The signature algorithm for redirect-binding requests: <c>RSA-SHA1</c>, <c>RSA-SHA256</c>, ...</summary>
	[JsonPropertyName("signatureAlgorithm")]
	public string? SignatureAlgorithm { get; init; }

	/// <summary>How the subject is identified in an assertion.</summary>
	[JsonPropertyName("nameIdFormat")]
	public string? NameIdFormat { get; init; }

	/// <summary>The binding of service-provider-initiated requests: <c>HTTPPost</c> or <c>HTTPRedirect</c>.</summary>
	[JsonPropertyName("ssoBinding")]
	public string? SsoBinding { get; init; }

	/// <summary>The binding of logout requests and responses: <c>HTTPPost</c> or <c>HTTPRedirect</c>.</summary>
	[JsonPropertyName("sloBinding")]
	public string? SloBinding { get; init; }

	/// <summary>The SAML attribute mapped to the email address.</summary>
	[JsonPropertyName("attributeAliasMail")]
	public string? AttributeAliasMail { get; init; }

	/// <summary>The SAML attribute mapped to the real name.</summary>
	[JsonPropertyName("attributeAliasRealName")]
	public string? AttributeAliasRealName { get; init; }

	/// <summary>The SAML attribute mapped to roles.</summary>
	[JsonPropertyName("attributeAliasRole")]
	public string? AttributeAliasRole { get; init; }

	/// <summary>Whether to sign attribute queries.</summary>
	[JsonPropertyName("attributeQueryRequestSigned")]
	public bool? AttributeQueryRequestSigned { get; init; }

	/// <summary>Whether attribute query responses must be signed.</summary>
	[JsonPropertyName("attributeQueryResponseSigned")]
	public bool? AttributeQueryResponseSigned { get; init; }

	/// <summary>The user name for attribute queries over SOAP.</summary>
	[JsonPropertyName("attributeQuerySoapUsername")]
	public string? AttributeQuerySoapUsername { get; init; }

	/// <summary>The password for attribute queries over SOAP. A secret.</summary>
	[JsonPropertyName("attributeQuerySoapPassword")]
	public string? AttributeQuerySoapPassword { get; init; }

	/// <summary>How long, in seconds, to cache attribute query results.</summary>
	[JsonPropertyName("attributeQueryTTL")]
	public int? AttributeQueryTtl { get; init; }

	/// <summary>The URL shown for a SAML error. The reference spells it <c>errorUrL</c>; Splunk accepts <c>errorUrl</c>.</summary>
	[JsonPropertyName("errorUrl")]
	public string? ErrorUrl { get; init; }

	/// <summary>The label of <see cref="ErrorUrl"/>.</summary>
	[JsonPropertyName("errorUrlLabel")]
	public string? ErrorUrlLabel { get; init; }

	/// <summary>Whether to verify certificates when making attribute queries.</summary>
	[JsonPropertyName("sslVerifyServerCert")]
	public bool? SslVerifyServerCert { get; init; }

	/// <summary>The SSL/TLS versions used for attribute queries.</summary>
	[JsonPropertyName("sslVersions")]
	public string? SslVersions { get; init; }

	/// <summary>The path of the CA certificate.</summary>
	[JsonPropertyName("caCertFile")]
	public string? CaCertFile { get; init; }

	/// <summary>The location of the service provider's private key.</summary>
	[JsonPropertyName("sslKeysfile")]
	public string? SslKeysFile { get; init; }

	/// <summary>The password of <see cref="SslKeysFile"/>. A secret.</summary>
	[JsonPropertyName("sslKeysfilePassword")]
	public string? SslKeysFilePassword { get; init; }
}
