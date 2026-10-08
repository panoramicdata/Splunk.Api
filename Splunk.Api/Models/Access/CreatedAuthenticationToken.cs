using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Access;

/// <summary>A newly created authentication token (<c>POST authorization/tokens</c>).</summary>
public sealed class CreatedAuthenticationToken : SplunkContent
{
	/// <summary>The token's identifier, used to update or delete it.</summary>
	[JsonPropertyName("id")]
	public string? TokenId { get; init; }

	/// <summary>
	/// The token, sent as <c>Authorization: Bearer {token}</c>. A secret that Splunk returns only once: store it securely
	/// and never log it.
	/// </summary>
	[JsonPropertyName("token")]
	public string? Token { get; init; }
}
