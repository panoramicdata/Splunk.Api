using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Search;

/// <summary>A reply that carries only messages, such as <c>{"messages":[{"type":"INFO","text":"Search job cancelled."}]}</c>.</summary>
public sealed class SearchMessagesResponse
{
	/// <summary>The messages, in order.</summary>
	[JsonPropertyName("messages")]
	public IReadOnlyList<SplunkMessage> Messages { get; init; } = [];
}
