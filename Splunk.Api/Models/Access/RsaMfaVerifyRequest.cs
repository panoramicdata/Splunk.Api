using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Access;

/// <summary>Verifies an RSA configuration (<c>POST admin/Rsa-MFA-config-verify/{rsa-stanza-name}</c>).</summary>
public sealed class RsaMfaVerifyRequest : SplunkFormRequest
{
	/// <summary>The RSA user name.</summary>
	[JsonPropertyName("username")]
	public string? Username { get; init; }

	/// <summary>The RSA passcode: the user's PIN followed by the token code. A secret.</summary>
	[JsonPropertyName("passcode")]
	public string? Passcode { get; init; }
}
