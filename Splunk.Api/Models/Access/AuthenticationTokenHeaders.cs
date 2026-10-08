using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Access;

/// <summary>The JWT headers of an authentication token.</summary>
public sealed class AuthenticationTokenHeaders
{
	/// <summary>The signing algorithm (<c>alg</c>), for example <c>HS512</c>.</summary>
	[JsonPropertyName("alg")]
	public string? Algorithm { get; init; }

	/// <summary>The signing key identifier (<c>kid</c>), for example <c>splunk.secret</c>.</summary>
	[JsonPropertyName("kid")]
	public string? KeyId { get; init; }

	/// <summary>The token type (<c>ttyp</c>): <c>static</c> or <c>ephemeral</c>.</summary>
	[JsonPropertyName("ttyp")]
	public string? TokenType { get; init; }

	/// <summary>The token format version (<c>ver</c>), for example <c>v2</c>.</summary>
	[JsonPropertyName("ver")]
	public string? Version { get; init; }
}
