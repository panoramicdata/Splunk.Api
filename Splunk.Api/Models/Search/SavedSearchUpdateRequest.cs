using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Search;

/// <summary>Changes a saved search (<c>POST saved/searches/{name}</c>); only the properties set are changed.</summary>
public sealed class SavedSearchUpdateRequest : SavedSearchSettings
{
	/// <summary>The new search, in SPL (<c>search</c>).</summary>
	[JsonPropertyName("search")]
	public string? Search { get; init; }
}
