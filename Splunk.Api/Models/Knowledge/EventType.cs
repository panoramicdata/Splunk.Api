using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Knowledge;

/// <summary>An event type (<c>saved/eventtypes</c>).</summary>
public sealed class EventType : SplunkContent
{
	/// <summary>The search that defines the event type.</summary>
	[JsonPropertyName("search")]
	public string? Search { get; init; }

	/// <summary>The description.</summary>
	[JsonPropertyName("description")]
	public string? Description { get; init; }

	/// <summary>The display priority, 1 (highest) to 10, among matching event types.</summary>
	[JsonPropertyName("priority")]
	public int? Priority { get; init; }

	/// <summary>The colour events of this type are highlighted with, or <c>none</c>.</summary>
	[JsonPropertyName("color")]
	public string? Color { get; init; }

	/// <summary>The tags of the event type. Deprecated by Splunk: tag event types through <c>search/tags</c> instead.</summary>
	[JsonPropertyName("tags")]
	public IReadOnlyList<string> Tags { get; init => field = value ?? []; } = [];
}
