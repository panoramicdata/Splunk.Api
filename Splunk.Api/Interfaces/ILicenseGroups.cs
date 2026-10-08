using Refit;
using Splunk.Api.Models;
using Splunk.Api.Models.Licensing;

namespace Splunk.Api.Interfaces;

/// <summary>
/// License groups (<c>licenser/groups</c>): <c>Enterprise</c>, <c>Forwarder</c>, <c>Free</c>, <c>Trial</c> and so on.
/// Exactly one is active.
/// </summary>
public interface ILicenseGroups
{
	/// <summary>Lists the license groups (<c>GET licenser/groups</c>).</summary>
	/// <param name="options">Paging and filtering, or <see langword="null"/> for Splunk's defaults.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>The groups.</returns>
	[Get("services/licenser/groups")]
	Task<SplunkFeed<LicenseGroup>> ListAsync([Query] ListOptions? options, CancellationToken cancellationToken);

	/// <summary>Gets one license group (<c>GET licenser/groups/{name}</c>).</summary>
	/// <param name="name">The group name, for example <c>Enterprise</c>.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with the one group.</returns>
	[Get("services/licenser/groups/{name}")]
	Task<SplunkFeed<LicenseGroup>> GetAsync(string name, CancellationToken cancellationToken);

	/// <summary>Activates a license group, deactivating the previously active one (<c>POST licenser/groups/{name}</c>).</summary>
	/// <param name="name">The group name.</param>
	/// <param name="request">The activation.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>The group.</returns>
	/// <remarks>Changing the active group changes the whole instance's licensing and usually requires a restart.</remarks>
	[Post("services/licenser/groups/{name}")]
	Task<SplunkFeed<LicenseGroup>> UpdateAsync(string name, [Body] LicenseGroupUpdateRequest request, CancellationToken cancellationToken);
}
