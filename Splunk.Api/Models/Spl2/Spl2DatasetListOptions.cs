using Refit;

namespace Splunk.Api.Models.Spl2;

/// <summary>Query parameters for <c>GET orchestrator/v1/datasets</c>.</summary>
public sealed class Spl2DatasetListOptions
{
	/// <summary>Only datasets of this kind (<c>kind</c>), for example <c>index</c>, <c>lookup</c>, <c>savedsearch</c> or <c>view</c>.</summary>
	[AliasAs("kind")]
	public string? Kind { get; init; }

	/// <summary>A filter expression (<c>filter</c>).</summary>
	[AliasAs("filter")]
	public string? Filter { get; init; }

	/// <summary>The properties to order by, comma-separated, each optionally followed by <c>asc</c> or <c>desc</c> (<c>orderBy</c>).</summary>
	[AliasAs("orderBy")]
	public string? OrderBy { get; init; }

	/// <summary>The number of datasets to skip (<c>offset</c>).</summary>
	[AliasAs("offset")]
	public int? Offset { get; init; }

	/// <summary>The page size, 1 to 1000 (<c>pageSize</c>); Splunk's default is 1000.</summary>
	[AliasAs("pageSize")]
	public int? PageSize { get; init; }

	/// <summary>The token of the page to read (<c>paginationToken</c>), from <see cref="Spl2DatasetList.NextPaginationToken"/>.</summary>
	[AliasAs("paginationToken")]
	public string? PaginationToken { get; init; }

	/// <summary>Only datasets of this federated connection (<c>connection_id</c>).</summary>
	[AliasAs("connection_id")]
	public string? ConnectionId { get; init; }

	/// <summary>Only datasets of this data source (<c>dataSource</c>).</summary>
	[AliasAs("dataSource")]
	public string? DataSource { get; init; }

	/// <summary>Only datasets with these capabilities (<c>supported_capabilities</c>).</summary>
	[AliasAs("supported_capabilities")]
	public string? SupportedCapabilities { get; init; }

	/// <summary>Only datasets with this visibility (<c>visibility</c>).</summary>
	[AliasAs("visibility")]
	public string? Visibility { get; init; }
}
