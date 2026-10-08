using System.Text.Json.Serialization;

namespace Splunk.Api.Models.FederatedSearch;

/// <summary>Changes to a federated index (<c>POST data/federated/index/{name}</c>). Only the properties set are sent.</summary>
public sealed class FederatedIndexUpdateRequest : FederatedIndexSettings
{
	/// <summary>The remote dataset, as <c>&lt;prefix&gt;:&lt;name&gt;</c>.</summary>
	[JsonPropertyName("federated.dataset")]
	public string? Dataset { get; init; }
}
