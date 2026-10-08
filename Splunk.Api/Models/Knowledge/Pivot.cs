using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Knowledge;

/// <summary>The searches that run a pivot over a data model (<c>datamodel/pivot/{name}</c>).</summary>
/// <remarks>The content also carries the data model's acceleration and dataset settings, kept in <see cref="SplunkContent.AdditionalProperties"/>.</remarks>
public sealed class Pivot : SplunkContent
{
	/// <summary>The pivot as a <c>| pivot</c> search command (<c>pivot_search</c>).</summary>
	[JsonPropertyName("pivot_search")]
	public string? PivotSearch { get; init; }

	/// <summary>The pivot as JSON (<c>pivot_json</c>).</summary>
	[JsonPropertyName("pivot_json")]
	public string? PivotJson { get; init; }

	/// <summary>The search that runs the pivot report.</summary>
	[JsonPropertyName("search")]
	public string? Search { get; init; }

	/// <summary>The search to open the pivot in the Search app (<c>open_in_search</c>).</summary>
	[JsonPropertyName("open_in_search")]
	public string? OpenInSearch { get; init; }

	/// <summary>The search for drilldown from the pivot (<c>drilldown_search</c>).</summary>
	[JsonPropertyName("drilldown_search")]
	public string? DrilldownSearch { get; init; }

	/// <summary>The <c>tstats</c> search that uses acceleration summaries, or empty when the model is not accelerated (<c>tstats_search</c>).</summary>
	[JsonPropertyName("tstats_search")]
	public string? TstatsSearch { get; init; }
}
