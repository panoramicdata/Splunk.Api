using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Access;

/// <summary>Maps an identity provider group to Splunk roles (<c>POST admin/oauth2-groups</c>).</summary>
public sealed class OAuth2GroupCreateRequest : SplunkFormRequest
{
	/// <summary>The group name in the identity provider.</summary>
	[JsonPropertyName("name")]
	public required string Name { get; init; }

	/// <summary>The OAuth configuration the mapping belongs to.</summary>
	[JsonPropertyName("config")]
	public required string Config { get; init; }

	/// <summary>The Splunk roles to assign, sent once per role.</summary>
	[JsonPropertyName("roles")]
	public required IReadOnlyList<string> Roles { get; init; }
}
