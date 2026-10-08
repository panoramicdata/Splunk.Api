using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Access;

/// <summary>Changes a user (<c>POST authentication/users/{name}</c>).</summary>
/// <remarks>The reference marks <see cref="Password"/> required; a live Splunk 10.6 does not.</remarks>
public sealed class UserUpdateRequest : UserSettings
{
	/// <summary>A new login password. A secret.</summary>
	[JsonPropertyName("password")]
	public string? Password { get; init; }

	/// <summary>The current password; needed only when users change their own <see cref="Password"/>. A secret.</summary>
	[JsonPropertyName("oldpassword")]
	public string? OldPassword { get; init; }
}
