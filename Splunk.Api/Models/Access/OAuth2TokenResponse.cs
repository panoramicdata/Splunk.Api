using System.Text.Json;
using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Access;

/// <summary>A Splunk access token issued for an identity provider's token (<c>POST oauth2/v1/token</c>).</summary>
/// <remarks>The standard OAuth 2.0 token response fields are modelled; any others are in <see cref="AdditionalProperties"/>.</remarks>
public sealed class OAuth2TokenResponse
{
	/// <summary>The Splunk access token, sent as <c>Authorization: Bearer {token}</c>. A secret: never log it.</summary>
	[JsonPropertyName("access_token")]
	public string? AccessToken { get; init; }

	/// <summary>The token type, typically <c>Bearer</c>.</summary>
	[JsonPropertyName("token_type")]
	public string? TokenType { get; init; }

	/// <summary>How long, in seconds, the token is valid.</summary>
	[JsonPropertyName("expires_in")]
	public long? ExpiresIn { get; init; }

	/// <summary>The token's scope, if Splunk returns one.</summary>
	[JsonPropertyName("scope")]
	public string? Scope { get; init; }

	/// <summary>Any other fields Splunk returned.</summary>
	[JsonExtensionData]
	public IDictionary<string, JsonElement> AdditionalProperties { get; init; } = new Dictionary<string, JsonElement>();
}
