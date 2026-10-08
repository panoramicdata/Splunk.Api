using System.Text.Json.Serialization;

namespace Splunk.Api.Models.FederatedSearch;

/// <summary>Which federated providers to turn off at once (<c>POST data/federated/provider/turnOffProvidersInBatch</c>).</summary>
public sealed class FederatedProviderBatchDisableRequest : SplunkFormRequest
{
	/// <summary>Turns off only providers of this type; <see langword="null"/> turns off every provider.</summary>
	[JsonPropertyName("type")]
	public FederatedProviderType? Type { get; init; }
}
