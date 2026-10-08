using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Inputs;

/// <summary>An HTTP Event Collector token (<c>data/inputs/http/{name}</c>). The entry name is <c>http://{name}</c>.</summary>
public sealed class HecToken : InputContent
{
	/// <summary>The token value senders put in <c>Authorization: Splunk {token}</c>.</summary>
	[JsonPropertyName("token")]
	public string? Token { get; init; }

	/// <summary>The indexes events sent with this token may name; empty means any index the token's owner may write.</summary>
	[JsonPropertyName("indexes")]
	public IReadOnlyList<string> Indexes { get; init; } = [];

	/// <summary>Whether indexer acknowledgement is on (<c>useACK</c>): senders must then name a channel.</summary>
	[JsonPropertyName("useACK")]
	public bool? UseAck { get; init; }

	/// <summary>A description of the token.</summary>
	[JsonPropertyName("description")]
	public string? Description { get; init; }
}
