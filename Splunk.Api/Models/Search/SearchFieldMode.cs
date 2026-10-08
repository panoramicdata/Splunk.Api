using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Search;

/// <summary>One of a field's most frequent values, in a <see cref="SearchFieldSummary"/>.</summary>
public sealed class SearchFieldMode
{
	/// <summary>The value.</summary>
	[JsonPropertyName("value")]
	public string? Value { get; init; }

	/// <summary>The number of events with the value.</summary>
	[JsonPropertyName("count")]
	public long Count { get; init; }

	/// <summary>Whether the count is exact.</summary>
	[JsonPropertyName("is_exact")]
	public bool IsExact { get; init; }
}
