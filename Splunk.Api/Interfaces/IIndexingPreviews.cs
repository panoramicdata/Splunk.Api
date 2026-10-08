using Refit;
using Splunk.Api.Models;
using Splunk.Api.Models.Inputs;

namespace Splunk.Api.Interfaces;

/// <summary>
/// Data preview jobs, which show how a file would be indexed (<c>indexing/preview</c>). Requires the
/// <c>edit_monitor</c> or <c>edit_upload_and_index</c> capability.
/// </summary>
public interface IIndexingPreviews
{
	/// <summary>Lists the data preview jobs (<c>GET indexing/preview</c>).</summary>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with one entry per job, named by its ID.</returns>
	[Get("services/indexing/preview")]
	Task<SplunkFeed<IndexingPreview>> ListAsync(CancellationToken cancellationToken);

	/// <summary>
	/// Starts a data preview job for a file on the Splunk server (<c>POST indexing/preview</c>). Splunk answers with no
	/// entries and the new job's ID as the text of the first message (<see cref="SplunkFeed{T}.Messages"/>).
	/// </summary>
	/// <param name="request">The file, and any <c>props.conf</c> settings to try.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed whose first message holds the job ID.</returns>
	[Post("services/indexing/preview")]
	Task<SplunkFeed<IndexingPreview>> CreateAsync([Body] IndexingPreviewCreateRequest request, CancellationToken cancellationToken);

	/// <summary>Gets the <c>props.conf</c> settings a data preview job uses (<c>GET indexing/preview/{job_id}</c>).</summary>
	/// <param name="jobId">The job's ID.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with the job's settings.</returns>
	[Get("services/indexing/preview/{jobId}")]
	Task<SplunkFeed<IndexingPreview>> GetAsync(string jobId, CancellationToken cancellationToken);
}
