using Splunk.Api.Serialization;
using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Server;

/// <summary>
/// A system message, as shown in Splunk Web's Messages menu (<c>messages</c>). The content also holds a key named after the
/// message whose value is the message text; it is in <see cref="SplunkContent.AdditionalProperties"/>.
/// </summary>
public sealed class ServerMessage : SplunkContent
{
	/// <summary>The message text.</summary>
	[JsonPropertyName("message")]
	public string? Message { get; init; }

	/// <summary>An alternative form of the message text, often empty.</summary>
	[JsonPropertyName("message_alternate")]
	public string? MessageAlternate { get; init; }

	/// <summary>The severity.</summary>
	[JsonPropertyName("severity")]
	public MessageSeverity Severity { get; init; }

	/// <summary>The server that raised the message.</summary>
	[JsonPropertyName("server")]
	public string? Server { get; init; }

	/// <summary>A help link or topic, often empty.</summary>
	[JsonPropertyName("help")]
	public string? Help { get; init; }

	/// <summary>The capabilities a user needs to see the message.</summary>
	[JsonPropertyName("capabilities")]
	public IReadOnlyList<string> Capabilities { get; init; } = [];

	/// <summary>The roles that see the message.</summary>
	[JsonPropertyName("roles")]
	public IReadOnlyList<string> Roles { get; init; } = [];

	/// <summary>When the message was created (<c>timeCreated_epochSecs</c>).</summary>
	[JsonPropertyName("timeCreated_epochSecs")]
	[JsonConverter(typeof(EpochSecondsConverter))]
	public DateTimeOffset? TimeCreated { get; init; }

	/// <summary>When the message was created, as Splunk formats it (<c>timeCreated_iso</c>).</summary>
	[JsonPropertyName("timeCreated_iso")]
	public DateTimeOffset? TimeCreatedIso { get; init; }
}
