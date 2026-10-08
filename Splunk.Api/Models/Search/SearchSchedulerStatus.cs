using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Search;

/// <summary>Whether the search scheduler runs scheduled searches (<c>search/scheduler</c>).</summary>
public sealed class SearchSchedulerStatus : SplunkContent
{
	/// <summary>Whether scheduled saved searches are disabled.</summary>
	[JsonPropertyName("saved_searches_disabled")]
	public bool SavedSearchesDisabled { get; init; }
}
