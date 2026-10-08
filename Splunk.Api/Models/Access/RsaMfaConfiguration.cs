using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Access;

/// <summary>An RSA Authentication Manager multifactor configuration (<c>admin/Rsa-MFA</c>).</summary>
public sealed class RsaMfaConfiguration : SplunkContent
{
	/// <summary>The URL of the RSA Authentication Manager REST endpoint.</summary>
	[JsonPropertyName("authManagerUrl")]
	public string? AuthManagerUrl { get; init; }

	/// <summary>The access key Splunk uses with RSA Authentication Manager; Splunk hides it in responses.</summary>
	[JsonPropertyName("accessKey")]
	public string? AccessKey { get; init; }

	/// <summary>The agent name created on RSA Authentication Manager.</summary>
	[JsonPropertyName("clientId")]
	public string? ClientId { get; init; }

	/// <summary>Whether users can log in when the authentication server is unavailable.</summary>
	[JsonPropertyName("failOpen")]
	public bool? FailOpen { get; init; }

	/// <summary>The connection timeout, in seconds, for the outbound HTTPS connection.</summary>
	[JsonPropertyName("timeout")]
	public int? Timeout { get; init; }

	/// <summary>The message shown to a user whose login fails.</summary>
	[JsonPropertyName("messageOnError")]
	public string? MessageOnError { get; init; }

	/// <summary>Whether REST API calls are also authenticated with RSA.</summary>
	[JsonPropertyName("enableMfaAuthRest")]
	public bool? EnableMfaAuthRest { get; init; }

	/// <summary>The SSL certificate chain of the RSA server.</summary>
	[JsonPropertyName("caCertBundlePayload")]
	public string? CaCertBundlePayload { get; init; }

	/// <summary>Whether the RSA certificate files are replicated across a search head cluster.</summary>
	[JsonPropertyName("replicateCertificates")]
	public bool? ReplicateCertificates { get; init; }
}
