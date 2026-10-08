using Refit;

namespace Splunk.Api.Models.MetricsCatalog;

/// <summary>Query parameters for the metrics catalog lists, besides paging and filtering.</summary>
public class MetricCatalogOptions : ListOptions
{
	/// <summary>The earliest time of the data to look at (<c>earliest</c>); Splunk's default is <c>-1d</c>.</summary>
	[AliasAs("earliest")]
	public string? Earliest { get; init; }

	/// <summary>The latest time of the data to look at (<c>latest</c>); Splunk's default is <c>now</c>.</summary>
	[AliasAs("latest")]
	public string? Latest { get; init; }

	/// <summary>A filter on metric fields (<c>filter</c>), for example <c>index=my_metrics</c> or <c>host=web01</c>.</summary>
	[AliasAs("filter")]
	public string? Filter { get; init; }
}
