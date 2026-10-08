using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Spl2;

/// <summary>A page of SPL2 datasets (<c>GET orchestrator/v1/datasets</c>).</summary>
public sealed class Spl2DatasetList
{
	/// <summary>The datasets.</summary>
	[JsonPropertyName("results")]
	public IReadOnlyList<Spl2Dataset> Results { get; init; } = [];

	/// <summary>The total number of matching datasets.</summary>
	[JsonPropertyName("totalCount")]
	public int? TotalCount { get; init; }

	/// <summary>The relative URL of the next page, or <see langword="null"/> on the last page.</summary>
	[JsonPropertyName("nextLink")]
	public string? NextLink { get; init; }

	/// <summary>
	/// The <c>paginationToken</c> of the next page (from <see cref="NextLink"/>), to pass in
	/// <see cref="Spl2DatasetListOptions.PaginationToken"/>; <see langword="null"/> on the last page.
	/// </summary>
	[JsonIgnore]
	public string? NextPaginationToken
		=> NextLink?.Split('?', 2) is [_, var query]
			? query.Split('&').Select(p => p.Split('=', 2)).Where(p => p is ["paginationToken", _]).Select(p => Uri.UnescapeDataString(p[1])).FirstOrDefault()
			: null;
}
