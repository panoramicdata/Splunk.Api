using Refit;
using Splunk.Api.Models;
using Splunk.Api.Models.Licensing;

namespace Splunk.Api.Interfaces;

/// <summary>This instance's license state as a license peer (<c>licenser/localpeer</c>).</summary>
public interface ILicenseLocalPeer
{
	/// <summary>Gets this instance's license state (<c>GET licenser/localpeer</c>).</summary>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with one entry, <c>license</c>.</returns>
	[Get("services/licenser/localpeer")]
	Task<SplunkFeed<LocalLicensePeer>> GetAsync(CancellationToken cancellationToken);
}
