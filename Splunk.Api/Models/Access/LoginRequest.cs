using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Access;

/// <summary>Logs in (<c>POST auth/login</c>).</summary>
public sealed class LoginRequest : SplunkFormRequest
{
	/// <summary>The user name.</summary>
	[JsonPropertyName("username")]
	public required string Username { get; init; }

	/// <summary>The user's password. A secret.</summary>
	[JsonPropertyName("password")]
	public required string Password { get; init; }

	/// <summary>The RSA multifactor passcode (PIN followed by token code); required only for RSA multifactor users.</summary>
	[JsonPropertyName("passcode")]
	public string? Passcode { get; init; }

	/// <summary>
	/// Whether Splunk should also set a session cookie (<c>cookie</c>), when <c>allowCookieAuth</c> is enabled in
	/// <c>server.conf</c>. The response headers are not exposed by <see cref="Interfaces.ISessions.LoginAsync"/>.
	/// </summary>
	[JsonPropertyName("cookie")]
	public bool? Cookie { get; init; }
}
