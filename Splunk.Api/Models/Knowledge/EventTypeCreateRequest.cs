using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Knowledge;

/// <summary>Creates an event type (<c>POST saved/eventtypes</c>).</summary>
public sealed class EventTypeCreateRequest : EventTypeSettings
{
	/// <summary>The event type name.</summary>
	[JsonPropertyName("name")]
	public required string Name { get; init; }
}
