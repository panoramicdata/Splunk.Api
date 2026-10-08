using Refit;
using Splunk.Api.Models;
using Splunk.Api.Models.FederatedSearch;

namespace Splunk.Api.Interfaces;

/// <summary>Federated provider definitions (<c>data/federated/provider</c>).</summary>
/// <remarks>
/// Providers ignore the user and app context: their stanzas always go to <c>etc/system/local/federated.conf</c>.
/// Reading needs <c>list_federated_providers</c> or <c>edit_federated_providers</c> (or read access to an index of the
/// provider); changing needs <c>edit_federated_providers</c>, and deleting also the admin role. Splunk Enterprise allows
/// only <see cref="FederatedProviderType.Splunk"/> providers; Amazon S3 providers are Splunk Cloud Platform only.
/// </remarks>
public interface IFederatedProviders
{
	/// <summary>Lists the federated providers (<c>GET data/federated/provider</c>).</summary>
	/// <param name="options">Paging, filtering and sorting, or <see langword="null"/>.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with one entry per provider.</returns>
	[Get("services/data/federated/provider")]
	Task<SplunkFeed<FederatedProvider>> ListAsync([Query] ListOptions? options, CancellationToken cancellationToken);

	/// <summary>Creates a federated provider (<c>POST data/federated/provider</c>).</summary>
	/// <remarks>
	/// Splunk tries to reach <see cref="FederatedProviderCreateRequest.HostPort"/> while creating a Splunk provider, so
	/// the call can take as long as a connection timeout (about 20 seconds for an unreachable host) and still succeed,
	/// with <see cref="FederatedProvider.ConnectivityStatus"/> <c>unknown</c>.
	/// </remarks>
	/// <param name="request">The new provider.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with the new provider.</returns>
	[Post("services/data/federated/provider")]
	Task<SplunkFeed<FederatedProvider>> CreateAsync([Body] FederatedProviderCreateRequest request, CancellationToken cancellationToken);

	/// <summary>
	/// Turns off every federated provider, or every provider of one type, in one call
	/// (<c>POST data/federated/provider/turnOffProvidersInBatch</c>).
	/// </summary>
	/// <remarks>
	/// This affects providers other users defined, and their federated indexes stop being searchable. Turn each provider
	/// back on with <see cref="EnableAsync"/>.
	/// </remarks>
	/// <param name="request">The provider type to turn off, or none for all.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>The feed Splunk answers with.</returns>
	[Post("services/data/federated/provider/turnOffProvidersInBatch")]
	Task<SplunkFeed<FederatedProvider>> DisableAllAsync([Body] FederatedProviderBatchDisableRequest request, CancellationToken cancellationToken);

	/// <summary>Gets a federated provider (<c>GET data/federated/provider/{name}</c>).</summary>
	/// <param name="name">The provider name.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with the provider.</returns>
	[Get("services/data/federated/provider/{name}")]
	Task<SplunkFeed<FederatedProvider>> GetAsync(string name, CancellationToken cancellationToken);

	/// <summary>Changes a federated provider (<c>POST data/federated/provider/{name}</c>).</summary>
	/// <param name="name">The provider name.</param>
	/// <param name="request">The settings to change.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with the updated provider.</returns>
	[Post("services/data/federated/provider/{name}")]
	Task<SplunkFeed<FederatedProvider>> UpdateAsync(string name, [Body] FederatedProviderUpdateRequest request, CancellationToken cancellationToken);

	/// <summary>Deletes a federated provider (<c>DELETE data/federated/provider/{name}</c>).</summary>
	/// <param name="name">The provider name.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A task that completes when the provider is deleted.</returns>
	[Delete("services/data/federated/provider/{name}")]
	Task DeleteAsync(string name, CancellationToken cancellationToken);

	/// <summary>
	/// Turns a federated provider off, so its federated indexes are not searchable
	/// (<c>POST data/federated/provider/{name}/disable</c>).
	/// </summary>
	/// <param name="name">The provider name.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with the provider.</returns>
	[Post("services/data/federated/provider/{name}/disable")]
	Task<SplunkFeed<FederatedProvider>> DisableAsync(string name, CancellationToken cancellationToken);

	/// <summary>Turns a federated provider back on (<c>POST data/federated/provider/{name}/enable</c>).</summary>
	/// <param name="name">The provider name.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with the provider.</returns>
	[Post("services/data/federated/provider/{name}/enable")]
	Task<SplunkFeed<FederatedProvider>> EnableAsync(string name, CancellationToken cancellationToken);
}
