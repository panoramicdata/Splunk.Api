using Refit;
using Splunk.Api.Models;
using Splunk.Api.Models.Knowledge;

namespace Splunk.Api.Interfaces;

/// <summary>The global banner shown across Splunk Web (<c>data/ui/global-banner</c>).</summary>
/// <remarks>Reading is unrestricted; writing requires the <c>edit_global_banner</c> capability.</remarks>
public interface IGlobalBanners
{
	/// <summary>Gets the global banner (<c>GET data/ui/global-banner</c>).</summary>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with the banner entry, <c>BANNER_MESSAGE_SINGLETON</c>.</returns>
	[Get("services/data/ui/global-banner")]
	Task<SplunkFeed<GlobalBanner>> ListAsync(CancellationToken cancellationToken);

	/// <summary>Writes the global banner (<c>POST data/ui/global-banner</c>).</summary>
	/// <param name="request">The banner name (normally <see cref="GlobalBannerCreateRequest.SingletonName"/>), text, visibility, colour and link.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with the banner.</returns>
	/// <remarks>The reference's field names lack the <c>global_banner.</c> prefix Splunk 10.6 requires; see <see cref="GlobalBannerCreateRequest"/>.</remarks>
	[Post("services/data/ui/global-banner")]
	Task<SplunkFeed<GlobalBanner>> CreateAsync([Body] GlobalBannerCreateRequest request, CancellationToken cancellationToken);
}
