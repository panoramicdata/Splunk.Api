using System.Text.Json;
using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Search;

/// <summary>How Splunk parsed a search (<c>search/v2/parser</c>): its phases and commands.</summary>
public sealed class SearchParseResult
{
	/// <summary>The search after normalization (macro expansion and optimization unless parsing only).</summary>
	[JsonPropertyName("normalizedSearch")]
	public string? NormalizedSearch { get; init; }

	/// <summary>The search sent to the search peers.</summary>
	[JsonPropertyName("remoteSearch")]
	public string? RemoteSearch { get; init; }

	/// <summary>Whether the remote search returns events in time order.</summary>
	[JsonPropertyName("remoteTimeOrdered")]
	public bool RemoteTimeOrdered { get; init; }

	/// <summary>The part of the search before the first transforming command.</summary>
	[JsonPropertyName("eventsSearch")]
	public string? EventsSearch { get; init; }

	/// <summary>Whether the events are in time order.</summary>
	[JsonPropertyName("eventsTimeOrdered")]
	public bool EventsTimeOrdered { get; init; }

	/// <summary>Whether the events are streamed.</summary>
	[JsonPropertyName("eventsStreaming")]
	public bool EventsStreaming { get; init; }

	/// <summary>The transforming (reporting) part of the search, if any.</summary>
	[JsonPropertyName("reportsSearch")]
	public string? ReportsSearch { get; init; }

	/// <summary>Whether the whole search is streaming (has no transforming command).</summary>
	[JsonPropertyName("isStreamingSearch")]
	public bool IsStreamingSearch { get; init; }

	/// <summary>Whether the search can be summarized (accelerated).</summary>
	[JsonPropertyName("canSummarize")]
	public bool CanSummarize { get; init; }

	/// <summary>The search's commands, in pipeline order.</summary>
	[JsonPropertyName("commands")]
	public IReadOnlyList<SearchParsedCommand> Commands { get; init; } = [];

	/// <summary>Any other properties Splunk returned.</summary>
	[JsonExtensionData]
	public IDictionary<string, JsonElement> AdditionalProperties { get; init; } = new Dictionary<string, JsonElement>();
}
