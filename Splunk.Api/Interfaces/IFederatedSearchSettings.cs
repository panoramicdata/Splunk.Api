using Refit;
using Splunk.Api.Models;
using Splunk.Api.Models.FederatedSearch;

namespace Splunk.Api.Interfaces;

/// <summary>The general settings of Federated Search for Splunk (<c>data/federated/settings/general</c>).</summary>
/// <remarks>
/// Reading needs <c>list_federated_provider</c> or <c>edit_federated_provider</c>; changing needs
/// <c>edit_federated_provider</c>. The settings do not apply to Federated Search for Amazon S3.
/// </remarks>
public interface IFederatedSearchSettings
{
	/// <summary>Gets the general federated search settings (<c>GET data/federated/settings/general</c>).</summary>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with one entry, <c>general</c>.</returns>
	[Get("services/data/federated/settings/general")]
	Task<SplunkFeed<FederatedSearchSettings>> GetAsync(CancellationToken cancellationToken);

	/// <summary>Changes the general federated search settings (<c>POST data/federated/settings/general</c>).</summary>
	/// <remarks>
	/// A change of <see cref="FederatedSearchSettingsUpdateRequest.TransparentMode"/> takes effect only after
	/// <c>configs/conf-federated/_reload</c>.
	/// </remarks>
	/// <param name="request">The settings to change.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with the updated <c>general</c> entry.</returns>
	[Post("services/data/federated/settings/general")]
	Task<SplunkFeed<FederatedSearchSettings>> UpdateAsync([Body] FederatedSearchSettingsUpdateRequest request, CancellationToken cancellationToken);
}
