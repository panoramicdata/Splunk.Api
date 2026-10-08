using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Knowledge;

/// <summary>The settings of an event type, shared by <see cref="EventTypeCreateRequest"/> and <see cref="EventTypeUpdateRequest"/>.</summary>
/// <remarks>
/// The deprecated <c>tags</c> field is not modelled: tag an event type with <c>search/tags</c> (field <c>eventtype</c>), or
/// send <c>tags</c> through <see cref="SplunkFormRequest.AdditionalParameters"/>.
/// </remarks>
public abstract class EventTypeSettings : SplunkFormRequest
{
	/// <summary>The search terms that define the event type, for example <c>index=web status&gt;=500</c>.</summary>
	[JsonPropertyName("search")]
	public required string Search { get; init; }

	/// <summary>The description.</summary>
	[JsonPropertyName("description")]
	public string? Description { get; init; }

	/// <summary>The display priority, 1 (highest, the default) to 10, among matching event types.</summary>
	[JsonPropertyName("priority")]
	public int? Priority { get; init; }

	/// <summary>The highlight colour, for example <c>et_red</c>, or <c>none</c>.</summary>
	[JsonPropertyName("color")]
	public string? Color { get; init; }

	/// <summary>Whether the event type is disabled.</summary>
	[JsonPropertyName("disabled")]
	public bool? Disabled { get; init; }
}
