using Refit;

namespace Splunk.Api.Models.Introspection;

/// <summary>Paging and the summary kinds for <c>GET data/summaries</c>.</summary>
public sealed class SummaryListOptions : ListOptions
{
	/// <summary>Whether to include report acceleration summaries (<c>report_acceleration</c>).</summary>
	[AliasAs("report_acceleration")]
	public bool? ReportAcceleration { get; init; }

	/// <summary>Whether to include data model acceleration summaries (<c>data_model_acceleration</c>).</summary>
	[AliasAs("data_model_acceleration")]
	public bool? DataModelAcceleration { get; init; }
}
