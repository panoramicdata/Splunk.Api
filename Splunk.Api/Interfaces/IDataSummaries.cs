using Refit;
using Splunk.Api.Models;
using Splunk.Api.Models.Introspection;

namespace Splunk.Api.Interfaces;

/// <summary>Disk usage of report and data model acceleration summaries on an indexer (<c>data/summaries</c>).</summary>
public interface IDataSummaries
{
	/// <summary>Lists the summaries (<c>GET data/summaries</c>).</summary>
	/// <param name="options">Paging and the summary kinds, or <see langword="null"/> for Splunk's defaults.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with one entry per summary; its properties are in <see cref="SplunkContent.AdditionalProperties"/>.</returns>
	[Get("services/data/summaries")]
	Task<SplunkFeed<SplunkDynamicContent>> ListAsync([Query] SummaryListOptions? options, CancellationToken cancellationToken);

	/// <summary>Gets one summary (<c>GET data/summaries/{name}</c>).</summary>
	/// <param name="name">The summary name.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with the summary.</returns>
	[Get("services/data/summaries/{name}")]
	Task<SplunkFeed<SplunkDynamicContent>> GetAsync(string name, CancellationToken cancellationToken);
}
