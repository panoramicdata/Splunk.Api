using Refit;
using Splunk.Api.Models;
using Splunk.Api.Models.Inputs;

namespace Splunk.Api.Interfaces;

/// <summary>The ingestion pipeline sets of an indexer (<c>server/pipelinesets</c>). Requires the <c>list_pipeline_sets</c> capability.</summary>
public interface IPipelineSets
{
	/// <summary>Lists the pipeline sets and how busy each was in the last period (<c>GET server/pipelinesets</c>).</summary>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with one entry per pipeline set, for example <c>ingest_pipe_0</c>.</returns>
	[Get("services/server/pipelinesets")]
	Task<SplunkFeed<PipelineSet>> ListAsync(CancellationToken cancellationToken);
}
