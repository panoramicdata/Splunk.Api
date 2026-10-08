using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Access;

/// <summary>The response to <c>POST auth/login</c>: a session key, not a feed.</summary>
public sealed class LoginResponse
{
	/// <summary>
	/// The session key, sent on later requests as <c>Authorization: Splunk {key}</c>. A secret that grants the user's
	/// access until the session times out (by default after an hour without use): never log or persist it.
	/// </summary>
	[JsonPropertyName("sessionKey")]
	public string? SessionKey { get; init; }

	/// <summary>A message from Splunk; usually empty.</summary>
	[JsonPropertyName("message")]
	public string? Message { get; init; }

	/// <summary>A code from Splunk; usually empty.</summary>
	[JsonPropertyName("code")]
	public string? Code { get; init; }
}
