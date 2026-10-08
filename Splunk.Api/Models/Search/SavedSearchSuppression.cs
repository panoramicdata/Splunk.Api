using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Search;

/// <summary>An alert's throttling state (<c>saved/searches/{name}/suppress</c>).</summary>
public sealed class SavedSearchSuppression : SplunkContent
{
	/// <summary>Whether the alert is being throttled now.</summary>
	[JsonPropertyName("suppressed")]
	public bool Suppressed { get; init; }

	/// <summary>The suppression key: the owner, app and name, and the throttled field values.</summary>
	[JsonPropertyName("suppressionKey")]
	public string? SuppressionKey { get; init; }

	/// <summary>When the throttling ends, while suppressed.</summary>
	[JsonPropertyName("expiration")]
	public string? Expiration { get; init; }
}
