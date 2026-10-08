using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Access;

/// <summary>
/// Maps an external group or user to Splunk roles (<c>POST admin/ProxySSO-groups</c>,
/// <c>admin/ProxySSO-user-role-map</c>, <c>admin/SAML-groups</c> or <c>admin/SAML-user-role-map</c>).
/// </summary>
public sealed class RoleMappingCreateRequest : SplunkFormRequest
{
	/// <summary>The external group or user name.</summary>
	[JsonPropertyName("name")]
	public required string Name { get; init; }

	/// <summary>The Splunk roles to map, sent once per role.</summary>
	[JsonPropertyName("roles")]
	public required IReadOnlyList<string> Roles { get; init; }
}
