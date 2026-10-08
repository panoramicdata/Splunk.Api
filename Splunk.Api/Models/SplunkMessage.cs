using System.Text.Json.Serialization;

namespace Splunk.Api.Models;

/// <summary>A message Splunk returned with a response or an error.</summary>
public sealed class SplunkMessage
{
	/// <summary>The message type, for example <c>ERROR</c>, <c>WARN</c>, <c>INFO</c> or <c>DEBUG</c>.</summary>
	[JsonPropertyName("type")]
	public string Type { get; init; } = string.Empty;

	/// <summary>The message text.</summary>
	[JsonPropertyName("text")]
	public string Text { get; init; } = string.Empty;

	/// <inheritdoc />
	public override string ToString() => $"{Type}: {Text}";
}
