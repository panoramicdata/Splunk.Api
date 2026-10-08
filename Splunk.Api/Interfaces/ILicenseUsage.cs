using Refit;
using Splunk.Api.Models;
using Splunk.Api.Models.Licensing;

namespace Splunk.Api.Interfaces;

/// <summary>License usage (<c>licenser/usage</c>).</summary>
public interface ILicenseUsage
{
	/// <summary>Gets today's license usage, as of the last minute, since midnight server time (<c>GET licenser/usage</c>).</summary>
	/// <param name="options">Paging and filtering, or <see langword="null"/> for Splunk's defaults.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with one entry, <c>license_usage</c>.</returns>
	[Get("services/licenser/usage")]
	Task<SplunkFeed<LicenseUsage>> GetAsync([Query] ListOptions? options, CancellationToken cancellationToken);
}
