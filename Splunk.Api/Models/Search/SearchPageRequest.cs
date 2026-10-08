using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Search;

/// <summary>
/// The form parameters for reading a job's results or events with POST, which (unlike GET in the v2 API) also takes a
/// post-process <see cref="Search"/>.
/// </summary>
public abstract class SearchPageRequest : SplunkFormRequest
{
	/// <summary>A post-process search applied to the rows before they are returned (<c>search</c>), for example <c>stats count by host</c>.</summary>
	[JsonPropertyName("search")]
	public string? Search { get; init; }

	/// <summary>The most rows to return (<c>count</c>); Splunk's default is 100, and 0 returns every available row.</summary>
	[JsonPropertyName("count")]
	public int? Count { get; init; }

	/// <summary>The index of the first row (<c>offset</c>).</summary>
	[JsonPropertyName("offset")]
	public int? Offset { get; init; }

	/// <summary>The fields to return (<c>f</c>, repeated); every field by default.</summary>
	[JsonPropertyName("f")]
	public IReadOnlyList<string>? Fields { get; init; }
}
