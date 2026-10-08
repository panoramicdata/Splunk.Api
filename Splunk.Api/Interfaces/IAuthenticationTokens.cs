using Refit;
using Splunk.Api.Models;
using Splunk.Api.Models.Access;

namespace Splunk.Api.Interfaces;

/// <summary>
/// Authentication (bearer) tokens (<c>authorization/tokens</c>), sent as <c>Authorization: Bearer {token}</c>. Token
/// authentication must be enabled on the instance. Managing other users' tokens requires <c>edit_tokens_all</c>;
/// one's own, <c>edit_tokens_own</c>.
/// </summary>
public interface IAuthenticationTokens
{
	/// <summary>Lists tokens (<c>GET authorization/tokens</c>).</summary>
	/// <param name="options">The user, token and status to filter by, paging and filtering; or <see langword="null"/>.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>The tokens, named by their identifier. The token values themselves are not returned.</returns>
	[Get("services/authorization/tokens")]
	Task<SplunkFeed<AuthenticationToken>> ListAsync([Query] AuthenticationTokenListOptions? options, CancellationToken cancellationToken);

	/// <summary>Creates a token (<c>POST authorization/tokens</c>).</summary>
	/// <param name="request">The user, audience and validity period.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>
	/// A feed with one entry holding the token's identifier and value. The value is a secret and is returned only here.
	/// </returns>
	/// <remarks>The reference describes this POST as changing token status; a live Splunk 10.6 creates a token.</remarks>
	[Post("services/authorization/tokens")]
	Task<SplunkFeed<CreatedAuthenticationToken>> CreateAsync([Body] AuthenticationTokenCreateRequest request, CancellationToken cancellationToken);

	/// <summary>Enables or disables a user's tokens (<c>POST authorization/tokens/{name}</c>).</summary>
	/// <param name="userName">The user whose tokens to change.</param>
	/// <param name="request">The token and its new status.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>An empty feed with an <c>INFO</c> message such as <c>Token(s) updated to status: disabled.</c></returns>
	/// <remarks>
	/// The reference describes this POST as creating a token with <c>audience</c>, <c>expires_on</c> and
	/// <c>not_before</c>; a live Splunk 10.6 rejects those arguments and requires <c>status</c> (with <c>id</c>).
	/// </remarks>
	[Post("services/authorization/tokens/{userName}")]
	Task<SplunkFeed<AuthenticationToken>> UpdateStatusAsync(string userName, [Body] AuthenticationTokenStatusRequest request, CancellationToken cancellationToken);

	/// <summary>Deletes a user's token, or all of them (<c>DELETE authorization/tokens/{name}</c>).</summary>
	/// <param name="userName">The user whose token to delete.</param>
	/// <param name="tokenId">
	/// The token's identifier (<c>id</c>). <see langword="null"/> deletes <b>every</b> token the user has.
	/// </param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A task that completes when the token is deleted. Splunk reports success even for an unknown id.</returns>
	[Delete("services/authorization/tokens/{userName}")]
	Task DeleteAsync(string userName, [AliasAs("id")] string? tokenId, CancellationToken cancellationToken);
}
