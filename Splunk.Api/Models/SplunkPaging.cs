using System.Text.Json.Serialization;

namespace Splunk.Api.Models;

/// <summary>Paging details of a <see cref="SplunkFeed{T}"/>.</summary>
public sealed class SplunkPaging
{
	/// <summary>The total number of entries available.</summary>
	[JsonPropertyName("total")]
	public int Total { get; init; }

	/// <summary>The page size requested (<c>count</c>); 0 means all.</summary>
	[JsonPropertyName("perPage")]
	public int PerPage { get; init; }

	/// <summary>The index of the first entry returned (<c>offset</c>).</summary>
	[JsonPropertyName("offset")]
	public int Offset { get; init; }
}
