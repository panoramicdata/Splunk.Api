using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Search;

/// <summary>
/// One line of a JSON export stream (<c>search/v2/jobs/export</c>):
/// <c>{"preview":false,"offset":0,"lastrow":true,"result":{...}}</c>. Read a stream with
/// <see cref="SearchExportReader.ReadAsync"/>.
/// </summary>
public sealed class SearchExportRecord
{
	/// <summary>Whether this is a preview result of a transforming search that has not finished; final results are not.</summary>
	[JsonPropertyName("preview")]
	public bool Preview { get; init; }

	/// <summary>The result's offset.</summary>
	[JsonPropertyName("offset")]
	public long? Offset { get; init; }

	/// <summary>Whether this is the last row (of the final results, or of a preview).</summary>
	[JsonPropertyName("lastrow")]
	public bool LastRow { get; init; }

	/// <summary>The result, or <see langword="null"/> for a line without one (such as the last row of a search with no results).</summary>
	[JsonPropertyName("result")]
	public SearchResult? Result { get; init; }

	/// <summary>Messages about the search sent in the stream, if any.</summary>
	[JsonPropertyName("messages")]
	public IReadOnlyList<SplunkMessage> Messages { get; init; } = [];
}
