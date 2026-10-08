using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Access;

/// <summary>Changes a stored credential's password (<c>POST storage/passwords/{name}</c>).</summary>
public sealed class StoredPasswordUpdateRequest : SplunkFormRequest
{
	/// <summary>The new password, encrypted by Splunk. A secret.</summary>
	[JsonPropertyName("password")]
	public required string Password { get; init; }
}
