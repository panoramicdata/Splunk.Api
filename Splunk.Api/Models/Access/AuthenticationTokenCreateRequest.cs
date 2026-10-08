using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Access;

/// <summary>Creates an authentication token (<c>POST authorization/tokens</c>).</summary>
public sealed class AuthenticationTokenCreateRequest : SplunkFormRequest
{
	/// <summary>The user the token authenticates as (up to 1024 characters).</summary>
	[JsonPropertyName("name")]
	public required string Name { get; init; }

	/// <summary>The token's purpose (up to 256 characters).</summary>
	[JsonPropertyName("audience")]
	public required string Audience { get; init; }

	/// <summary>
	/// When the token expires: an absolute time such as <c>2027-02-09T07:35:00+07:00</c> or a relative one such as
	/// <c>+90d</c>. It cannot be in the past. Omit for the instance's default.
	/// </summary>
	[JsonPropertyName("expires_on")]
	public string? ExpiresOn { get; init; }

	/// <summary>When the token becomes valid, absolute or relative; not after <see cref="ExpiresOn"/>.</summary>
	[JsonPropertyName("not_before")]
	public string? NotBefore { get; init; }

	/// <summary>The token's initial status. Splunk's default is enabled.</summary>
	[JsonPropertyName("status")]
	public TokenStatus? Status { get; init; }
}
