using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Access;

/// <summary>Stores a credential (<c>POST storage/passwords</c>).</summary>
public sealed class StoredPasswordCreateRequest : SplunkFormRequest
{
	/// <summary>The user name the credential belongs to.</summary>
	[JsonPropertyName("name")]
	public required string Name { get; init; }

	/// <summary>The password to store, encrypted by Splunk. A secret.</summary>
	[JsonPropertyName("password")]
	public required string Password { get; init; }

	/// <summary>The realm the credential is valid in, for example an app-specific service name.</summary>
	[JsonPropertyName("realm")]
	public string? Realm { get; init; }
}
