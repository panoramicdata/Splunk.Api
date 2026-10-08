using System.Text.Json;
using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Search;

/// <summary>
/// A page of a job's results or events (<c>search/v2/jobs/{search_id}/results</c>, <c>events</c>,
/// <c>results_preview</c>), or a oneshot search's results. This is Splunk's results shape, not the feed envelope.
/// </summary>
public sealed class SearchResults
{
	/// <summary>Whether these are preview results of a job still running.</summary>
	[JsonPropertyName("preview")]
	public bool Preview { get; init; }

	/// <summary>The offset of the first result in this page.</summary>
	[JsonPropertyName("init_offset")]
	public int InitOffset { get; init; }

	/// <summary>With a post-process search, the number of results the post-process search produced.</summary>
	[JsonPropertyName("post_process_count")]
	public int? PostProcessCount { get; init; }

	/// <summary>Messages about the search, such as warnings.</summary>
	[JsonPropertyName("messages")]
	public IReadOnlyList<SplunkMessage> Messages { get; init; } = [];

	/// <summary>The fields in the results, in column order. Absent (empty) when there are no results.</summary>
	[JsonPropertyName("fields")]
	public IReadOnlyList<SearchField> Fields { get; init; } = [];

	/// <summary>The results or events.</summary>
	[JsonPropertyName("results")]
	public IReadOnlyList<SearchResult> Results { get; init; } = [];

	/// <summary>Highlighting information, keyed by result; usually empty.</summary>
	[JsonPropertyName("highlighted")]
	public IReadOnlyDictionary<string, JsonElement> Highlighted { get; init; } = new Dictionary<string, JsonElement>();
}
