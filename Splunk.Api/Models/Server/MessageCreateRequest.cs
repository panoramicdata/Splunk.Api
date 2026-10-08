using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Server;

/// <summary>Creates a persistent system message (<c>POST messages</c>).</summary>
public sealed class MessageCreateRequest : SplunkFormRequest
{
	/// <summary>The message identifier (<c>name</c>).</summary>
	[JsonPropertyName("name")]
	public required string Name { get; init; }

	/// <summary>The message text (<c>value</c>).</summary>
	[JsonPropertyName("value")]
	public required string Value { get; init; }

	/// <summary>The severity (<c>severity</c>); Splunk's default is <c>warn</c>.</summary>
	[JsonPropertyName("severity")]
	public MessageSeverity? Severity { get; init; }

	/// <summary>Capabilities a user needs to see the message (<c>capability</c>, repeated).</summary>
	[JsonPropertyName("capability")]
	public IEnumerable<string>? Capabilities { get; init; }

	/// <summary>Roles that see the message (<c>role</c>, repeated).</summary>
	[JsonPropertyName("role")]
	public IEnumerable<string>? Roles { get; init; }
}
