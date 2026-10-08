using Refit;
using Splunk.Api.Models.Access;

namespace Splunk.Api.Interfaces;

/// <summary>OAuth 2.0 token exchange (<c>oauth2/v1/token</c>).</summary>
public interface IOAuth2Tokens
{
	/// <summary>
	/// Exchanges an identity provider's JWT for a Splunk access token, using the client credentials grant with a JWT bearer
	/// client assertion (<c>POST oauth2/v1/token</c>).
	/// </summary>
	/// <param name="request">The client ID and the identity provider's JWT.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>The Splunk access token, in the standard OAuth 2.0 token response shape.</returns>
	/// <remarks>
	/// <para>
	/// The reference lists this endpoint with the others under <c>services/</c>, where a live Splunk 10.6 answers 404 (or
	/// 401 without credentials). It is served at the management port's root, <c>/oauth2/v1/token</c>, which is where this
	/// method sends it; a client namespace does not apply.
	/// </para>
	/// <para>
	/// The endpoint is public: Splunk validates the client assertion, not this client's credentials. Its errors are
	/// OAuth-shaped (<c>{"error": "invalid_request", "error_description": "..."}</c>), so
	/// the <see cref="SplunkApiException"/> message carries only the HTTP status.
	/// </para>
	/// </remarks>
	[Post("oauth2/v1/token")]
	Task<OAuth2TokenResponse> ExchangeAsync([Body] OAuth2TokenExchangeRequest request, CancellationToken cancellationToken);
}
