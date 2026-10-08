using Refit;

namespace Splunk.Api.Models.Access;

/// <summary>Filters for listing authentication tokens (<c>GET authorization/tokens</c>).</summary>
public sealed class AuthenticationTokenListOptions : ListOptions
{
	/// <summary>Only this user's tokens (<c>username</c>).</summary>
	[AliasAs("username")]
	public string? Username { get; init; }

	/// <summary>Only the token with this identifier (<c>id</c>).</summary>
	[AliasAs("id")]
	public string? TokenId { get; init; }

	/// <summary>Only tokens with this status (<c>status</c>).</summary>
	[AliasAs("status")]
	public TokenStatus? Status { get; init; }
}
