using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Search;

/// <summary>Search auto-complete suggestions (<c>search/typeahead</c>).</summary>
public sealed class TypeaheadResponse
{
	/// <summary>The suggestions, most likely first.</summary>
	[JsonPropertyName("results")]
	public IReadOnlyList<TypeaheadSuggestion> Results { get; init; } = [];
}
