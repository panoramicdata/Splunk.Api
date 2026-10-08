using Refit;

namespace Splunk.Api.Models.Access;

/// <summary>Filters for listing LDAP strategies (<c>GET authentication/providers/LDAP</c>).</summary>
public sealed class LdapStrategyListOptions : ListOptions
{
	/// <summary>The strategy name (<c>strategy</c>).</summary>
	[AliasAs("strategy")]
	public string? Strategy { get; init; }
}
