using Refit;

namespace Splunk.Api.Models.Knowledge;

/// <summary>Options for listing data model acceleration summaries (<c>GET admin/summarization</c>).</summary>
public sealed class DataModelSummaryListOptions : ListOptions
{
	/// <summary>When <see langword="true"/>, lists the <c>tstats</c> (data model) summaries, as the reference's example does (<c>by_tstats</c>).</summary>
	[AliasAs("by_tstats")]
	public bool? ByTstats { get; init; }
}
