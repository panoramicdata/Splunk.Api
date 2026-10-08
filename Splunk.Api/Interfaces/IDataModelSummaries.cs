using Refit;
using Splunk.Api.Models;
using Splunk.Api.Models.Knowledge;

namespace Splunk.Api.Interfaces;

/// <summary>Data model acceleration summaries (<c>admin/summarization</c>).</summary>
/// <remarks>Access is role-based. Only accelerated data models have a summary.</remarks>
public interface IDataModelSummaries
{
	/// <summary>Lists the acceleration summaries of every accelerated data model (<c>GET admin/summarization</c>).</summary>
	/// <param name="options">Paging and filtering, and <c>by_tstats</c>; <see langword="null"/> for Splunk's defaults.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with one entry per summary, named <c>tstats:DM_{app}_{model}</c>.</returns>
	[Get("services/admin/summarization")]
	Task<SplunkFeed<DataModelSummary>> ListAsync([Query] DataModelSummaryListOptions? options, CancellationToken cancellationToken);

	/// <summary>Gets the acceleration summary of one data model (<c>GET admin/summarization/tstats:DM_{app}_{data_model_ID}</c>).</summary>
	/// <param name="app">The app the data model belongs to, for example <c>search</c>.</param>
	/// <param name="dataModelId">The data model name (id).</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with one entry. Splunk answers 404 when the model is not accelerated.</returns>
	[Get("services/admin/summarization/tstats:DM_{app}_{dataModelId}")]
	Task<SplunkFeed<DataModelSummary>> GetAsync(string app, string dataModelId, CancellationToken cancellationToken);
}
