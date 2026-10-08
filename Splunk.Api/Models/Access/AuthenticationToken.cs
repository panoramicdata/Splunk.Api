using Splunk.Api.Serialization;
using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Access;

/// <summary>An authentication token (<c>authorization/tokens</c>); the entry name is the token's identifier.</summary>
/// <remarks>The token value itself is returned only when the token is created (<see cref="CreatedAuthenticationToken"/>).</remarks>
public sealed class AuthenticationToken : SplunkContent
{
	/// <summary>The token's claims: its user, audience, issuer and validity period.</summary>
	[JsonPropertyName("claims")]
	public AuthenticationTokenClaims? Claims { get; init; }

	/// <summary>The token's JWT headers.</summary>
	[JsonPropertyName("headers")]
	public AuthenticationTokenHeaders? Headers { get; init; }

	/// <summary>Whether the token is enabled.</summary>
	[JsonPropertyName("status")]
	public TokenStatus Status { get; init; }

	/// <summary>When the token was last used; <see langword="null"/> if never.</summary>
	[JsonPropertyName("lastUsed")]
	[JsonConverter(typeof(EpochSecondsConverter))]
	public DateTimeOffset? LastUsed { get; init; }

	/// <summary>The IP address the token was last used from; empty if never.</summary>
	[JsonPropertyName("lastUsedIp")]
	public string? LastUsedIp { get; init; }
}
