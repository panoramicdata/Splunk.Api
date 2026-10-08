using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Access;

/// <summary>Creates an OAuth 2.0 identity provider configuration (<c>POST authentication/providers/oauth2</c>).</summary>
public sealed class OAuth2ProviderCreateRequest : SplunkFormRequest
{
	/// <summary>The configuration name.</summary>
	[JsonPropertyName("name")]
	public required string Name { get; init; }

	/// <summary>The identity provider's public keys endpoint.</summary>
	[JsonPropertyName("jwks_uri")]
	public required string JwksUri { get; init; }

	/// <summary>The expected token audience.</summary>
	[JsonPropertyName("audience")]
	public required string Audience { get; init; }

	/// <summary>The JWT claim that contains the client's groups.</summary>
	[JsonPropertyName("groupsClaim")]
	public required string GroupsClaim { get; init; }

	/// <summary>The token issuer URL.</summary>
	[JsonPropertyName("issuer")]
	public required string Issuer { get; init; }

	/// <summary>The JWT claim that contains the client ID.</summary>
	[JsonPropertyName("clientIdClaim")]
	public required string ClientIdClaim { get; init; }

	/// <summary>The JWT claim that contains a human-readable client name.</summary>
	[JsonPropertyName("clientFullNameClaim")]
	public string? ClientFullNameClaim { get; init; }
}
