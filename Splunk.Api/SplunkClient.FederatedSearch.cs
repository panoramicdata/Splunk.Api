using Splunk.Api.Interfaces;

namespace Splunk.Api;

public sealed partial class SplunkClient
{
	/// <summary>General federated search settings (<c>data/federated/settings/general</c>).</summary>
	public IFederatedSearchSettings FederatedSearchSettings => field ??= For<IFederatedSearchSettings>();

	/// <summary>Federated providers (<c>data/federated/provider</c>).</summary>
	public IFederatedProviders FederatedProviders => field ??= For<IFederatedProviders>();

	/// <summary>Federated indexes (<c>data/federated/index</c>).</summary>
	public IFederatedIndexes FederatedIndexes => field ??= For<IFederatedIndexes>();
}
