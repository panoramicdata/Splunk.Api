using Refit;

namespace Splunk.Api.Models.Access;

/// <summary>Filters for listing LDAP groups (<c>GET admin/LDAP-groups</c>).</summary>
public sealed class LdapGroupListOptions : ListOptions
{
	/// <summary>The LDAP strategy name (<c>strategy</c>).</summary>
	[AliasAs("strategy")]
	public string? Strategy { get; init; }

	/// <summary>The LDAP group name (<c>LDAPgroup</c>).</summary>
	[AliasAs("LDAPgroup")]
	public string? LdapGroup { get; init; }
}
