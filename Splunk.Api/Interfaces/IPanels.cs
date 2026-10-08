using Refit;
using Splunk.Api.Models;
using Splunk.Api.Models.Knowledge;

namespace Splunk.Api.Interfaces;

/// <summary>Prebuilt dashboard panels (<c>data/ui/panels</c>).</summary>
/// <remarks>Panels belong to an app: use a namespace (<see cref="SplunkClient.InNamespace(string, string)"/>), as the reference's <c>servicesNS/{user}/{app}</c> URL does.</remarks>
public interface IPanels
{
	/// <summary>Lists prebuilt panels (<c>GET data/ui/panels</c>).</summary>
	/// <param name="options">Paging and filtering; <see langword="null"/> for Splunk's defaults.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with one entry per panel.</returns>
	[Get("services/data/ui/panels")]
	Task<SplunkFeed<Panel>> ListAsync([Query] ListOptions? options, CancellationToken cancellationToken);

	/// <summary>Creates a prebuilt panel (<c>POST data/ui/panels</c>).</summary>
	/// <param name="request">The panel name and Simple XML source.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with the new panel.</returns>
	[Post("services/data/ui/panels")]
	Task<SplunkFeed<Panel>> CreateAsync([Body] PanelCreateRequest request, CancellationToken cancellationToken);
}
