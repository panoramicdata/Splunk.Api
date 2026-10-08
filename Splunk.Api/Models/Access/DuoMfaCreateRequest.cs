using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Access;

/// <summary>Creates a Duo multifactor authentication configuration (<c>POST admin/Duo-MFA</c>).</summary>
public sealed class DuoMfaCreateRequest : DuoMfaSettings
{
	/// <summary>The configuration stanza name.</summary>
	[JsonPropertyName("name")]
	public required string Name { get; init; }

	/// <summary>The Duo integration key for Splunk (20 characters).</summary>
	[JsonPropertyName("integrationKey")]
	public required string IntegrationKey { get; init; }

	/// <summary>The secret key shared between Splunk and Duo.</summary>
	[JsonPropertyName("secretKey")]
	public required string SecretKey { get; init; }

	/// <summary>The Duo API host name Splunk calls.</summary>
	[JsonPropertyName("apiHostname")]
	public required string ApiHostname { get; init; }
}
