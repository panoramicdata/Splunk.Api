using System.Text.Json.Serialization;

namespace Splunk.Api.Models.FederatedSearch;

/// <summary>A new federated provider (<c>POST data/federated/provider</c>).</summary>
/// <remarks>A Federated Search for Splunk provider (<see cref="FederatedProviderType.Splunk"/>, the only type Splunk Enterprise allows) also needs <see cref="Mode"/>, <see cref="FederatedProviderSettings.HostPort"/>, <see cref="FederatedProviderSettings.ServiceAccount"/> and <see cref="FederatedProviderSettings.Password"/>; an Amazon S3 provider needs the <c>Aws*</c> properties and <see cref="Database"/>.</remarks>
public sealed class FederatedProviderCreateRequest : FederatedProviderSettings
{
	/// <summary>The provider name.</summary>
	[JsonPropertyName("name")]
	public required string Name { get; init; }

	/// <summary>The provider type.</summary>
	[JsonPropertyName("type")]
	public required FederatedProviderType Type { get; init; }

	/// <summary>The search mode (Federated Search for Splunk providers).</summary>
	[JsonPropertyName("mode")]
	public FederatedProviderMode? Mode { get; init; }

	/// <summary>The AWS Glue Data Catalog database (Amazon S3 providers).</summary>
	[JsonPropertyName("database")]
	public string? Database { get; init; }
}
