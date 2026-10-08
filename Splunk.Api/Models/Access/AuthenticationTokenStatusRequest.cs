using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Access;

/// <summary>Enables or disables a user's tokens (<c>POST authorization/tokens/{name}</c>).</summary>
public sealed class AuthenticationTokenStatusRequest : SplunkFormRequest
{
	/// <summary>The new status.</summary>
	[JsonPropertyName("status")]
	public required TokenStatus Status { get; init; }

	/// <summary>The token's identifier. <see langword="null"/> changes <b>every</b> token the user has.</summary>
	[JsonPropertyName("id")]
	public string? TokenId { get; init; }
}
