using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Search;

/// <summary>One auto-complete suggestion.</summary>
public sealed class TypeaheadSuggestion
{
	/// <summary>The suggested text, for example <c>index="_internal"</c>.</summary>
	[JsonPropertyName("content")]
	public string Content { get; init; } = string.Empty;

	/// <summary>How many events the term occurs in, where Splunk knows.</summary>
	[JsonPropertyName("count")]
	public long Count { get; init; }

	/// <summary>Whether the suggestion is a field/value operator term rather than a plain keyword.</summary>
	[JsonPropertyName("operator")]
	public bool Operator { get; init; }
}
