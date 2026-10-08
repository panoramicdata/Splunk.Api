using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Access;

/// <summary>Changes a Duo multifactor authentication configuration (<c>POST admin/Duo-MFA/{name}</c>).</summary>
public sealed class DuoMfaUpdateRequest : DuoMfaSettings
{
	/// <summary>The Duo integration key for Splunk (20 characters).</summary>
	[JsonPropertyName("integrationKey")]
	public string? IntegrationKey { get; init; }

	/// <summary>The secret key shared between Splunk and Duo.</summary>
	[JsonPropertyName("secretKey")]
	public string? SecretKey { get; init; }

	/// <summary>The Duo API host name Splunk calls.</summary>
	[JsonPropertyName("apiHostname")]
	public string? ApiHostname { get; init; }
}
