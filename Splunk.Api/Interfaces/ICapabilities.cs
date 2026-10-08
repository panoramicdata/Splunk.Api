using Refit;
using Splunk.Api.Models;
using Splunk.Api.Models.Access;

namespace Splunk.Api.Interfaces;

/// <summary>Capabilities (<c>authorization/capabilities</c> and <c>authorization/grantable_capabilities</c>).</summary>
public interface ICapabilities
{
	/// <summary>Lists every capability defined on the system (<c>GET authorization/capabilities</c>).</summary>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with one entry, <c>capabilities</c>, listing them all.</returns>
	[Get("services/authorization/capabilities")]
	Task<SplunkFeed<CapabilityList>> ListAsync(CancellationToken cancellationToken);

	/// <summary>Lists the capabilities the current user can grant (<c>GET authorization/grantable_capabilities</c>).</summary>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>
	/// A feed with one entry, <c>capabilities</c>: every capability for a user with <c>edit_roles</c>, otherwise those
	/// the user's <c>grantableRoles</c> allow.
	/// </returns>
	[Get("services/authorization/grantable_capabilities")]
	Task<SplunkFeed<CapabilityList>> ListGrantableAsync(CancellationToken cancellationToken);
}
