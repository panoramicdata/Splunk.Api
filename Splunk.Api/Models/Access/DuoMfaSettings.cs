using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Access;

/// <summary>The optional settings of a Duo configuration, shared by <see cref="DuoMfaCreateRequest"/> and <see cref="DuoMfaUpdateRequest"/>.</summary>
public abstract class DuoMfaSettings : SplunkFormRequest
{
	/// <summary>The Splunk application-specific secret key: random hexadecimal, at least 40 characters.</summary>
	[JsonPropertyName("appSecretKey")]
	public string? AppSecretKey { get; init; }

	/// <summary>Whether to let users in without Duo when the Duo service is unavailable. Splunk's default is <see langword="false"/>.</summary>
	[JsonPropertyName("failOpen")]
	public bool? FailOpen { get; init; }

	/// <summary>The connection timeout, in seconds, after which Duo is declared unavailable. Splunk's default is 15.</summary>
	[JsonPropertyName("timeout")]
	public int? Timeout { get; init; }

	/// <summary>The SSL/TLS versions used to call Duo. Splunk's default is splunkd's <c>sslVersions</c>.</summary>
	[JsonPropertyName("sslVersions")]
	public string? SslVersions { get; init; }

	/// <summary>The cipher suite used to call Duo. Splunk's default is splunkd's <c>cipherSuite</c>.</summary>
	[JsonPropertyName("cipherSuite")]
	public string? CipherSuite { get; init; }

	/// <summary>The ECDH curves used to call Duo. Splunk's default is splunkd's <c>ecdhCurves</c>.</summary>
	[JsonPropertyName("ecdhCurves")]
	public string? EcdhCurves { get; init; }

	/// <summary>Whether to verify Duo's server certificate; set <see cref="SslRootCAPath"/> too.</summary>
	[JsonPropertyName("sslVerifyServerCert")]
	public bool? SslVerifyServerCert { get; init; }

	/// <summary>The path of the CA certificate used to verify Duo's certificate.</summary>
	[JsonPropertyName("sslRootCAPath")]
	public string? SslRootCAPath { get; init; }

	/// <summary>The common name Duo's certificate must have.</summary>
	[JsonPropertyName("sslCommonNameToCheck")]
	public string? SslCommonNameToCheck { get; init; }

	/// <summary>The alternate name Duo's certificate must have.</summary>
	[JsonPropertyName("sslAltNameToCheck")]
	public string? SslAltNameToCheck { get; init; }

	/// <summary>Whether to enable client-side SSL compression.</summary>
	[JsonPropertyName("useClientSSLCompression")]
	public bool? UseClientSslCompression { get; init; }

	/// <summary>Whether REST API calls are also authenticated with Duo.</summary>
	[JsonPropertyName("enableMfaAuthRest")]
	public bool? EnableMfaAuthRest { get; init; }
}
