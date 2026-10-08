using System.Text.Json;
using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Search;

/// <summary>A field (column) of <see cref="SearchResults"/>.</summary>
public sealed class SearchField
{
	/// <summary>The field name.</summary>
	[JsonPropertyName("name")]
	public string Name { get; init; } = string.Empty;

	/// <summary>The field type Splunk inferred, for example <c>str</c> or <c>num</c>, when it reports one.</summary>
	[JsonPropertyName("type")]
	public string? Type { get; init; }

	/// <summary>For a <c>by</c> field of a transforming command, its position among the group-by fields.</summary>
	[JsonPropertyName("groupby_rank")]
	public int? GroupByRank { get; init; }

	/// <summary>
	/// Other properties, such as the <c>summary.count</c>, <c>summary.dc</c> and <c>summary.numcount</c> statistics added
	/// by <c>add_summary_to_metadata</c>.
	/// </summary>
	[JsonExtensionData]
	public IDictionary<string, JsonElement> AdditionalProperties { get; init; } = new Dictionary<string, JsonElement>();
}
