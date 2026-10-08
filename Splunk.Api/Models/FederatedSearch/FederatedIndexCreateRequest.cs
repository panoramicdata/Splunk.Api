using System.Text.Json.Serialization;

namespace Splunk.Api.Models.FederatedSearch;

/// <summary>A new federated index (<c>POST data/federated/index</c>).</summary>
public sealed class FederatedIndexCreateRequest : FederatedIndexSettings
{
	/// <summary>The index name; <c>federated:&lt;name&gt;</c> for Federated Search for Splunk (lowercase letters, digits, <c>_</c> and <c>-</c>).</summary>
	[JsonPropertyName("name")]
	public required string Name { get; init; }

	/// <summary>The federated provider that holds the dataset.</summary>
	[JsonPropertyName("federated.provider")]
	public required string Provider { get; init; }

	/// <summary>The remote dataset, as <c>&lt;prefix&gt;:&lt;name&gt;</c>, for example <c>index:main</c>.</summary>
	[JsonPropertyName("federated.dataset")]
	public required string Dataset { get; init; }
}
