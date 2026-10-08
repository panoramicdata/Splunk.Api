using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Access;

/// <summary>
/// The Splunk roles mapped to an external group or user (the entry name), as returned by the ProxySSO and SAML mapping
/// endpoints (<c>admin/ProxySSO-groups</c>, <c>admin/ProxySSO-user-role-map</c>, <c>admin/SAML-groups</c> and
/// <c>admin/SAML-user-role-map</c>).
/// </summary>
public sealed class RoleMapping : SplunkContent
{
	/// <summary>The mapped roles.</summary>
	[JsonPropertyName("roles")]
	public IReadOnlyList<string> Roles { get; init; } = [];
}
