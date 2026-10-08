using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Search;

/// <summary>Creates a saved search, report or alert (<c>POST saved/searches</c>).</summary>
public sealed class SavedSearchCreateRequest : SavedSearchSettings
{
	/// <summary>The name, unique in its app and owner context (<c>name</c>).</summary>
	[JsonPropertyName("name")]
	public required string Name { get; init; }

	/// <summary>The search, in SPL (<c>search</c>).</summary>
	[JsonPropertyName("search")]
	public required string Search { get; init; }
}
