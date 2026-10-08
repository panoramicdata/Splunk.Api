using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Inputs;

/// <summary>An active connection to a TCP or UDP input (<c>data/inputs/tcp/raw/{name}/connections</c> and similar).</summary>
public sealed class InputConnection : SplunkContent
{
	/// <summary>The sender's address and port.</summary>
	[JsonPropertyName("connection")]
	public string? Connection { get; init; }

	/// <summary>The sender's DNS or server name (<c>servername</c>).</summary>
	[JsonPropertyName("servername")]
	public string? ServerName { get; init; }

	/// <summary>The input status group, <c>listenerports</c>.</summary>
	[JsonPropertyName("group")]
	public string? Group { get; init; }
}
