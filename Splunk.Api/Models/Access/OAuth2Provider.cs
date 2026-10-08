using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Access;

/// <summary>An OAuth 2.0 identity provider configuration (<c>authentication/providers/oauth2</c>).</summary>
public sealed class OAuth2Provider : SplunkContent
{
	/// <summary>The identity provider's public keys endpoint (<c>jwks_uri</c>).</summary>
	[JsonPropertyName("jwks_uri")]
	public string? JwksUri { get; init; }

	/// <summary>The expected token audience (<c>aud</c> claim).</summary>
	[JsonPropertyName("audience")]
	public string? Audience { get; init; }

	/// <summary>The JWT claim that contains the client's groups.</summary>
	[JsonPropertyName("groupsClaim")]
	public string? GroupsClaim { get; init; }

	/// <summary>The token issuer URL.</summary>
	[JsonPropertyName("issuer")]
	public string? Issuer { get; init; }

	/// <summary>The JWT claim that contains the client ID.</summary>
	[JsonPropertyName("clientIdClaim")]
	public string? ClientIdClaim { get; init; }

	/// <summary>The JWT claim that contains a human-readable client name.</summary>
	[JsonPropertyName("clientFullNameClaim")]
	public string? ClientFullNameClaim { get; init; }
}
