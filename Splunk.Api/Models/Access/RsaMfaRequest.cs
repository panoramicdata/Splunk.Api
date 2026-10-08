using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Access;

/// <summary>Creates or edits the RSA multifactor authentication configuration (<c>POST admin/Rsa-MFA</c>).</summary>
public sealed class RsaMfaRequest : SplunkFormRequest
{
	/// <summary>The configuration stanza name.</summary>
	[JsonPropertyName("name")]
	public required string Name { get; init; }

	/// <summary>The URL of the RSA Authentication Manager REST endpoint.</summary>
	[JsonPropertyName("authManagerUrl")]
	public required string AuthManagerUrl { get; init; }

	/// <summary>The access key Splunk uses with RSA Authentication Manager.</summary>
	[JsonPropertyName("accessKey")]
	public required string AccessKey { get; init; }

	/// <summary>The agent name created on RSA Authentication Manager.</summary>
	[JsonPropertyName("clientId")]
	public required string ClientId { get; init; }

	/// <summary>The SSL certificate chain of the RSA server.</summary>
	[JsonPropertyName("caCertBundlePayload")]
	public required string CaCertBundlePayload { get; init; }

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

	/// <summary>Whether the RSA certificate files are replicated across a search head cluster.</summary>
	[JsonPropertyName("replicateCertificates")]
	public bool? ReplicateCertificates { get; init; }
}
