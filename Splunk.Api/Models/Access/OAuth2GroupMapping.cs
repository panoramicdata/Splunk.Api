using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Access;

/// <summary>
/// A mapping from an identity provider group (the entry name) to Splunk roles for an OAuth 2.0 configuration
/// (<c>admin/oauth2-groups</c>).
/// </summary>
public sealed class OAuth2GroupMapping : SplunkContent
{
	/// <summary>The OAuth configuration the mapping belongs to.</summary>
	[JsonPropertyName("config")]
	public string? Config { get; init; }

	/// <summary>The Splunk roles assigned to the group.</summary>
	[JsonPropertyName("roles")]
	public IReadOnlyList<string> Roles { get; init; } = [];
}
