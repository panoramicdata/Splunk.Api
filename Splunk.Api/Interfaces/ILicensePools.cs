using Refit;
using Splunk.Api.Models;
using Splunk.Api.Models.Licensing;

namespace Splunk.Api.Interfaces;

/// <summary>License pools (<c>licenser/pools</c>): shares of a license stack's quota allocated to license peers.</summary>
public interface ILicensePools
{
	/// <summary>Lists the license pools (<c>GET licenser/pools</c>).</summary>
	/// <param name="options">Paging and filtering, or <see langword="null"/> for Splunk's defaults.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>The pools.</returns>
	[Get("services/licenser/pools")]
	Task<SplunkFeed<LicensePool>> ListAsync([Query] ListOptions? options, CancellationToken cancellationToken);

	/// <summary>Creates a license pool (<c>POST licenser/pools</c>).</summary>
	/// <param name="request">The pool.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>The created pool.</returns>
	[Post("services/licenser/pools")]
	Task<SplunkFeed<LicensePool>> CreateAsync([Body] LicensePoolCreateRequest request, CancellationToken cancellationToken);

	/// <summary>Gets one license pool (<c>GET licenser/pools/{name}</c>).</summary>
	/// <param name="name">The pool name.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with the one pool.</returns>
	[Get("services/licenser/pools/{name}")]
	Task<SplunkFeed<LicensePool>> GetAsync(string name, CancellationToken cancellationToken);

	/// <summary>Updates a license pool (<c>POST licenser/pools/{name}</c>).</summary>
	/// <param name="name">The pool name.</param>
	/// <param name="request">The settings to change.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>The updated pool.</returns>
	[Post("services/licenser/pools/{name}")]
	Task<SplunkFeed<LicensePool>> UpdateAsync(string name, [Body] LicensePoolUpdateRequest request, CancellationToken cancellationToken);

	/// <summary>Deletes a license pool (<c>DELETE licenser/pools/{name}</c>).</summary>
	/// <param name="name">The pool name.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A task that completes when the pool is deleted.</returns>
	[Delete("services/licenser/pools/{name}")]
	Task DeleteAsync(string name, CancellationToken cancellationToken);
}
