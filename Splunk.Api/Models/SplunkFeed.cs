using System.Text.Json.Serialization;

namespace Splunk.Api.Models;

/// <summary>
/// The envelope Splunk wraps around most REST responses (<c>output_mode=json</c>): a list of entries plus paging,
/// messages and generator details.
/// </summary>
/// <typeparam name="T">The type of each entry's <c>content</c>.</typeparam>
public sealed class SplunkFeed<T>
{
	/// <summary>The feed title, typically the endpoint name.</summary>
	[JsonPropertyName("title")]
	public string? Title { get; init; }

	/// <summary>The URL of the endpoint that produced the feed.</summary>
	[JsonPropertyName("origin")]
	public string? Origin { get; init; }

	/// <summary>When the feed was generated.</summary>
	[JsonPropertyName("updated")]
	public DateTimeOffset? Updated { get; init; }

	/// <summary>The Splunk build and version that produced the feed.</summary>
	[JsonPropertyName("generator")]
	public SplunkGenerator? Generator { get; init; }

	/// <summary>Links to related operations, keyed by relation (for example <c>create</c> or <c>_reload</c>).</summary>
	[JsonPropertyName("links")]
	public IReadOnlyDictionary<string, string> Links { get; init; } = new Dictionary<string, string>();

	/// <summary>The entries, in the order Splunk returned them.</summary>
	[JsonPropertyName("entry")]
	public IReadOnlyList<SplunkEntry<T>> Entries { get; init; } = [];

	/// <summary>Paging details: the total number of entries and the page returned.</summary>
	[JsonPropertyName("paging")]
	public SplunkPaging? Paging { get; init; }

	/// <summary>Informational, warning or error messages returned with the feed.</summary>
	[JsonPropertyName("messages")]
	public IReadOnlyList<SplunkMessage> Messages { get; init; } = [];
}
