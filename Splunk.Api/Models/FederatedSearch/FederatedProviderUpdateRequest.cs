using System.Text.Json.Serialization;

namespace Splunk.Api.Models.FederatedSearch;

/// <summary>Changes to a federated provider (<c>POST data/federated/provider/{name}</c>). Only the properties set are sent; set at least one.</summary>
public sealed class FederatedProviderUpdateRequest : FederatedProviderSettings
{
	/// <summary>The federated indexes the provider may serve when index-based provider filtering is on.</summary>
	[JsonPropertyName("fedSrchIndexesAllowed")]
	public string? FederatedIndexesAllowed { get; init; }

	/// <summary>Whether to use the app context of the local search rather than <see cref="AppContext"/>.</summary>
	[JsonPropertyName("useAppContextFromSearch")]
	public bool? UseAppContextFromSearch { get; init; }
}
