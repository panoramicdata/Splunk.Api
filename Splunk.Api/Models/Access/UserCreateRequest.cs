using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Access;

/// <summary>Creates a user (<c>POST authentication/users</c>).</summary>
public sealed class UserCreateRequest : UserSettings
{
	/// <summary>The unique login name.</summary>
	[JsonPropertyName("name")]
	public required string Name { get; init; }

	/// <summary>The login password. A secret.</summary>
	/// <remarks>The reference marks this optional; a live Splunk 10.6 requires it.</remarks>
	[JsonPropertyName("password")]
	public required string Password { get; init; }

	/// <summary>Whether to create a role <c>user-{name}</c> for the user and assign it.</summary>
	[JsonPropertyName("createrole")]
	public bool? CreateRole { get; init; }
}
