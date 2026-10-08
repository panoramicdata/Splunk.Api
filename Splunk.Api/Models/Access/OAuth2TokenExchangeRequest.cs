using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Access;

/// <summary>Exchanges an identity provider's JWT for a Splunk access token (<c>POST oauth2/v1/token</c>).</summary>
public sealed class OAuth2TokenExchangeRequest : SplunkFormRequest
{
	/// <summary>The grant type; Splunk supports only <c>client_credentials</c>, the default.</summary>
	[JsonPropertyName("grant_type")]
	public string GrantType { get; init; } = "client_credentials";

	/// <summary>The OAuth application's client ID; it must match the corresponding claim in the identity provider's token.</summary>
	[JsonPropertyName("client_id")]
	public required string ClientId { get; init; }

	/// <summary>
	/// The client assertion type; Splunk supports only <c>urn:ietf:params:oauth:client-assertion-type:jwt-bearer</c>, the
	/// default.
	/// </summary>
	[JsonPropertyName("client_assertion_type")]
	public string ClientAssertionType { get; init; } = "urn:ietf:params:oauth:client-assertion-type:jwt-bearer";

	/// <summary>The JWT access token obtained from the identity provider. A secret.</summary>
	[JsonPropertyName("client_assertion")]
	public required string ClientAssertion { get; init; }
}
