using Refit;
using Splunk.Api.Models;
using Splunk.Api.Models.Licensing;

namespace Splunk.Api.Interfaces;

/// <summary>License stacks (<c>licenser/stacks</c>): licenses of the same type, whose quotas add up.</summary>
public interface ILicenseStacks
{
	/// <summary>Lists the license stacks (<c>GET licenser/stacks</c>).</summary>
	/// <param name="options">Paging and filtering, or <see langword="null"/> for Splunk's defaults.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>The stacks.</returns>
	[Get("services/licenser/stacks")]
	Task<SplunkFeed<LicenseStackInfo>> ListAsync([Query] ListOptions? options, CancellationToken cancellationToken);

	/// <summary>Gets one license stack (<c>GET licenser/stacks/{name}</c>).</summary>
	/// <param name="name">The stack ID, for example <c>enterprise</c>.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with the one stack.</returns>
	[Get("services/licenser/stacks/{name}")]
	Task<SplunkFeed<LicenseStackInfo>> GetAsync(string name, CancellationToken cancellationToken);
}
