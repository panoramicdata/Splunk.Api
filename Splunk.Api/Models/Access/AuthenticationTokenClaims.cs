using Splunk.Api.Serialization;
using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Access;

/// <summary>The JWT claims of an authentication token.</summary>
public sealed class AuthenticationTokenClaims
{
	/// <summary>The user the token authenticates (<c>sub</c>).</summary>
	[JsonPropertyName("sub")]
	public string? Subject { get; init; }

	/// <summary>The token's purpose (<c>aud</c>).</summary>
	[JsonPropertyName("aud")]
	public string? Audience { get; init; }

	/// <summary>Who issued the token (<c>iss</c>), for example <c>admin from splunk01</c>.</summary>
	[JsonPropertyName("iss")]
	public string? Issuer { get; init; }

	/// <summary>The identity provider (<c>idp</c>), for example <c>Splunk</c>.</summary>
	[JsonPropertyName("idp")]
	public string? IdentityProvider { get; init; }

	/// <summary>When the token was issued (<c>iat</c>).</summary>
	[JsonPropertyName("iat")]
	[JsonConverter(typeof(EpochSecondsConverter))]
	public DateTimeOffset? IssuedAt { get; init; }

	/// <summary>When the token expires (<c>exp</c>); <see langword="null"/> if it never does.</summary>
	[JsonPropertyName("exp")]
	[JsonConverter(typeof(EpochSecondsConverter))]
	public DateTimeOffset? ExpiresAt { get; init; }

	/// <summary>When the token becomes valid (<c>nbr</c>).</summary>
	[JsonPropertyName("nbr")]
	[JsonConverter(typeof(EpochSecondsConverter))]
	public DateTimeOffset? NotBefore { get; init; }

	/// <summary>The roles the token is limited to; <c>*</c> for all of the user's roles.</summary>
	[JsonPropertyName("roles")]
	public IReadOnlyList<string> Roles { get; init; } = [];
}
