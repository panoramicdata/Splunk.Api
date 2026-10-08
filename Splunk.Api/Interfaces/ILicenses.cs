using Refit;
using Splunk.Api.Models;
using Splunk.Api.Models.Licensing;

namespace Splunk.Api.Interfaces;

/// <summary>The licenses installed on this instance (<c>licenser/licenses</c>).</summary>
public interface ILicenses
{
	/// <summary>Lists the installed licenses (<c>GET licenser/licenses</c>).</summary>
	/// <param name="options">Paging and filtering, or <see langword="null"/> for Splunk's defaults.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>The licenses, named by their hash.</returns>
	[Get("services/licenser/licenses")]
	Task<SplunkFeed<License>> ListAsync([Query] ListOptions? options, CancellationToken cancellationToken);

	/// <summary>Adds a license (<c>POST licenser/licenses</c>).</summary>
	/// <param name="request">The license file path on the server, or the license XML.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>The added license.</returns>
	[Post("services/licenser/licenses")]
	Task<SplunkFeed<License>> AddAsync([Body] LicenseAddRequest request, CancellationToken cancellationToken);

	/// <summary>Gets one license (<c>GET licenser/licenses/{name}</c>).</summary>
	/// <param name="name">The license hash.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with the one license.</returns>
	[Get("services/licenser/licenses/{name}")]
	Task<SplunkFeed<License>> GetAsync(string name, CancellationToken cancellationToken);

	/// <summary>Deletes a license (<c>DELETE licenser/licenses/{name}</c>).</summary>
	/// <param name="name">The license hash.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A task that completes when the license is deleted.</returns>
	[Delete("services/licenser/licenses/{name}")]
	Task DeleteAsync(string name, CancellationToken cancellationToken);
}
