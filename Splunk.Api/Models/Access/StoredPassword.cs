using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Access;

/// <summary>
/// A stored credential (<c>storage/passwords</c>); the entry name is <c>{realm}:{username}:</c>.
/// </summary>
/// <remarks>
/// <b>Security:</b> Splunk returns the password in clear text (<see cref="ClearPassword"/>) on every read. Treat each
/// instance as a secret: never log, serialize or persist it, and keep it in memory no longer than needed.
/// </remarks>
public sealed class StoredPassword : SplunkContent
{
	/// <summary>The user name the credential belongs to.</summary>
	[JsonPropertyName("username")]
	public string? Username { get; init; }

	/// <summary>The realm the credential is valid in; empty when none was given.</summary>
	[JsonPropertyName("realm")]
	public string? Realm { get; init; }

	/// <summary>
	/// The password <b>in clear text</b>, as Splunk returns it on reads (not on create or update). A secret: never log or
	/// persist it.
	/// </summary>
	[JsonPropertyName("clear_password")]
	public string? ClearPassword { get; init; }

	/// <summary>The password as Splunk stores it, encrypted with the instance's <c>splunk.secret</c>.</summary>
	[JsonPropertyName("encr_password")]
	public string? EncryptedPassword { get; init; }

	/// <summary>The password mask, always <c>********</c>.</summary>
	[JsonPropertyName("password")]
	public string? Password { get; init; }
}
