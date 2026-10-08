using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Access;

/// <summary>Changes an OAuth 2.0 identity provider configuration (<c>POST authentication/providers/oauth2/{name}</c>).</summary>
public sealed class OAuth2ProviderUpdateRequest : SplunkFormRequest
{
	/// <summary>The identity provider's public keys endpoint.</summary>
	[JsonPropertyName("jwks_uri")]
	public string? JwksUri { get; init; }

	/// <summary>The expected token audience.</summary>
	[JsonPropertyName("audience")]
	public string? Audience { get; init; }

	/// <summary>The JWT claim that contains the client's groups.</summary>
	[JsonPropertyName("groupsClaim")]
	public string? GroupsClaim { get; init; }

	/// <summary>The JWT claim that contains the client ID.</summary>
	[JsonPropertyName("clientIdClaim")]
	public string? ClientIdClaim { get; init; }

	/// <summary>The JWT claim that contains a human-readable client name.</summary>
	[JsonPropertyName("clientFullNameClaim")]
	public string? ClientFullNameClaim { get; init; }

	/// <summary>Whether the configuration is disabled.</summary>
	[JsonPropertyName("disabled")]
	public bool? Disabled { get; init; }
}
