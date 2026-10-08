using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Search;

/// <summary>Lifts an alert's throttling (<c>POST saved/searches/{name}/acknowledge</c>).</summary>
public sealed class SavedSearchAcknowledgeRequest : SplunkFormRequest
{
	/// <summary>The suppression key to acknowledge (<c>key</c>), from <see cref="SavedSearchSuppression.SuppressionKey"/>; <see langword="null"/> for all.</summary>
	[JsonPropertyName("key")]
	public string? Key { get; init; }
}
