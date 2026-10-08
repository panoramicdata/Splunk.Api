using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Access;

/// <summary>An active session (<c>authentication/httpauth-tokens</c>).</summary>
public sealed class SessionInfo : SplunkContent
{
	/// <summary>The session key; Splunk masks it (<c>******</c>).</summary>
	[JsonPropertyName("authString")]
	public string? AuthString { get; init; }

	/// <summary>The search job the session was created for; empty for a login session.</summary>
	[JsonPropertyName("searchId")]
	public string? SearchId { get; init; }

	/// <summary>When the session was last used, as Splunk formats it (for example <c>Thu Oct  8 13:48:34 2026</c>).</summary>
	[JsonPropertyName("timeAccessed")]
	public string? TimeAccessed { get; init; }

	/// <summary>The user the session belongs to.</summary>
	[JsonPropertyName("userName")]
	public string? UserName { get; init; }
}
