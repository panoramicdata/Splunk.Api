using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Access;

/// <summary>A Duo multifactor authentication configuration (<c>admin/Duo-MFA</c>).</summary>
/// <remarks>Splunk returns the keys encrypted (<c>$1$...</c>), not in clear text.</remarks>
public sealed class DuoMfaConfiguration : SplunkContent
{
	/// <summary>The Duo API host name Splunk calls, for example <c>api-xxxxxxxx.duosecurity.com</c>.</summary>
	[JsonPropertyName("apiHostname")]
	public string? ApiHostname { get; init; }

	/// <summary>The Splunk application-specific secret key (encrypted).</summary>
	[JsonPropertyName("appSecretKey")]
	public string? AppSecretKey { get; init; }

	/// <summary>The cipher suite used to call Duo.</summary>
	[JsonPropertyName("cipherSuite")]
	public string? CipherSuite { get; init; }

	/// <summary>The ECDH curves used to call Duo.</summary>
	[JsonPropertyName("ecdhCurves")]
	public string? EcdhCurves { get; init; }

	/// <summary>Whether Splunk lets users in without Duo when the Duo service is unavailable.</summary>
	[JsonPropertyName("failOpen")]
	public bool? FailOpen { get; init; }

	/// <summary>The Duo integration key for Splunk (encrypted).</summary>
	[JsonPropertyName("integrationKey")]
	public string? IntegrationKey { get; init; }

	/// <summary>The secret key shared between Splunk and Duo (encrypted).</summary>
	[JsonPropertyName("secretKey")]
	public string? SecretKey { get; init; }

	/// <summary>The alternate name Duo's certificate must have.</summary>
	[JsonPropertyName("sslAltNameToCheck")]
	public string? SslAltNameToCheck { get; init; }

	/// <summary>The common name Duo's certificate must have.</summary>
	[JsonPropertyName("sslCommonNameToCheck")]
	public string? SslCommonNameToCheck { get; init; }

	/// <summary>The path of the CA certificate used to verify Duo's certificate.</summary>
	[JsonPropertyName("sslRootCAPath")]
	public string? SslRootCAPath { get; init; }

	/// <summary>Whether Duo's server certificate is verified.</summary>
	[JsonPropertyName("sslVerifyServerCert")]
	public bool? SslVerifyServerCert { get; init; }

	/// <summary>The SSL/TLS versions used to call Duo.</summary>
	[JsonPropertyName("sslVersions")]
	public string? SslVersions { get; init; }

	/// <summary>The connection timeout, in seconds, after which Duo is declared unavailable.</summary>
	[JsonPropertyName("timeout")]
	public int? Timeout { get; init; }

	/// <summary>Whether client-side SSL compression is enabled.</summary>
	[JsonPropertyName("useClientSSLCompression")]
	public bool? UseClientSslCompression { get; init; }
}
