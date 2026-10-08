using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Access;

/// <summary>Maps an LDAP group to Splunk roles (<c>POST admin/LDAP-groups</c>).</summary>
public sealed class LdapGroupUpdateRequest : SplunkFormRequest
{
	/// <summary>The LDAP strategy name.</summary>
	[JsonPropertyName("strategy")]
	public required string Strategy { get; init; }

	/// <summary>The LDAP group name.</summary>
	[JsonPropertyName("LDAPgroup")]
	public required string LdapGroup { get; init; }

	/// <summary>The roles to map to the group, sent once per role.</summary>
	[JsonPropertyName("roles")]
	public IReadOnlyList<string>? Roles { get; init; }
}
