using Refit;
using Splunk.Api.Models;
using Splunk.Api.Models.Access;

namespace Splunk.Api.Interfaces;

/// <summary>
/// LDAP group to role mappings (<c>admin/LDAP-groups</c>). Requires the <c>change_authentication</c> capability. Splunk
/// Enterprise only.
/// </summary>
public interface ILdapGroups
{
	/// <summary>Lists LDAP groups and the roles mapped to them (<c>GET admin/LDAP-groups</c>).</summary>
	/// <param name="options">The strategy and group to filter by, paging and filtering; or <see langword="null"/>.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>The groups. With no LDAP strategy enabled, the feed is empty and carries a <c>WARN</c> message.</returns>
	[Get("services/admin/LDAP-groups")]
	Task<SplunkFeed<LdapGroup>> ListAsync([Query] LdapGroupListOptions? options, CancellationToken cancellationToken);

	/// <summary>Maps an LDAP group to roles (<c>POST admin/LDAP-groups</c>).</summary>
	/// <param name="request">The strategy, group and roles.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>The group's mapping.</returns>
	/// <remarks>
	/// The reference lists this POST on the collection with <c>strategy</c> and <c>LDAPgroup</c> parameters, but its
	/// example posts to <c>admin/LDAP-groups/{strategy},{group}</c>, and a live Splunk 10.6 reports that this handler
	/// supports only <c>list</c> and <c>edit</c>.
	/// </remarks>
	[Post("services/admin/LDAP-groups")]
	Task<SplunkFeed<LdapGroup>> UpdateAsync([Body] LdapGroupUpdateRequest request, CancellationToken cancellationToken);
}
