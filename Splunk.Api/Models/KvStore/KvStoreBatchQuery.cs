using System.Text.Json.Serialization;

namespace Splunk.Api.Models.KvStore;

/// <summary>One query of a batch find (<c>POST storage/collections/data/{collection}/batch_find</c>).</summary>
public sealed class KvStoreBatchQuery
{
	/// <summary>
	/// The filter: any value that serializes to a JSON object, such as an anonymous object (<c>new { status = "open" }</c>),
	/// a dictionary or a <see cref="System.Text.Json.JsonElement"/>. <see langword="null"/> matches every document.
	/// </summary>
	[JsonPropertyName("query")]
	public object? Query { get; init; }

	/// <summary>The fields to return.</summary>
	[JsonPropertyName("fields")]
	public IReadOnlyList<string>? Fields { get; init; }

	/// <summary>The sort order: one object per key, for example <c>{"n": -1}</c> (<c>1</c> ascending, <c>-1</c> descending).</summary>
	[JsonPropertyName("sort")]
	public IReadOnlyList<IReadOnlyDictionary<string, int>>? Sort { get; init; }

	/// <summary>The maximum number of documents to return.</summary>
	[JsonPropertyName("limit")]
	public int? Limit { get; init; }

	/// <summary>The number of documents to skip.</summary>
	[JsonPropertyName("skip")]
	public int? Skip { get; init; }

	/// <summary>Whether to include documents of user <c>nobody</c> when reading in a user's namespace.</summary>
	[JsonPropertyName("shared")]
	public bool? Shared { get; init; }
}
