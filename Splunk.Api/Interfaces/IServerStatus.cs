using Refit;
using Splunk.Api.Models;
using Splunk.Api.Models.Introspection;

namespace Splunk.Api.Interfaces;

/// <summary>Server status: dispatch artifacts, fishbucket, file integrity, limits and disk space (<c>server/status</c>).</summary>
public interface IServerStatus
{
	/// <summary>Lists the status resources (<c>GET server/status</c>).</summary>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with one entry per resource, for example <c>dispatch-artifacts</c> or <c>resource-usage</c>.</returns>
	[Get("services/server/status")]
	Task<SplunkFeed<SplunkDynamicContent>> ListAsync(CancellationToken cancellationToken);

	/// <summary>Gets counts and sizes of search job artifacts (<c>GET server/status/dispatch-artifacts</c>).</summary>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with one entry, <c>result</c>.</returns>
	[Get("services/server/status/dispatch-artifacts")]
	Task<SplunkFeed<DispatchArtifactsStatus>> GetDispatchArtifactsAsync(CancellationToken cancellationToken);

	/// <summary>Gets the fishbucket's size (<c>GET server/status/fishbucket</c>).</summary>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with one entry, <c>result</c>.</returns>
	[Get("services/server/status/fishbucket")]
	Task<SplunkFeed<FishbucketStatus>> GetFishbucketAsync(CancellationToken cancellationToken);

	/// <summary>Checks installed files against the installation manifest (<c>GET server/status/installed-file-integrity</c>).</summary>
	/// <param name="options">Whether to re-run the check and which files to report, or <see langword="null"/>.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with one entry, <c>file-integrity</c>.</returns>
	[Get("services/server/status/installed-file-integrity")]
	Task<SplunkFeed<FileIntegrityStatus>> GetInstalledFileIntegrityAsync([Query] FileIntegrityOptions? options, CancellationToken cancellationToken);

	/// <summary>Gets the search concurrency limits of a standalone instance (<c>GET server/status/limits/search-concurrency</c>).</summary>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with one entry, <c>search-concurrency</c>.</returns>
	[Get("services/server/status/limits/search-concurrency")]
	Task<SplunkFeed<SearchConcurrencyLimits>> GetSearchConcurrencyLimitsAsync(CancellationToken cancellationToken);

	/// <summary>Lists the disk space of file systems holding Splunk data (<c>GET server/status/partitions-space</c>).</summary>
	/// <param name="options">Paging, filtering and sorting, or <see langword="null"/> for Splunk's defaults.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with one entry per file system.</returns>
	[Get("services/server/status/partitions-space")]
	Task<SplunkFeed<PartitionSpace>> ListPartitionsSpaceAsync([Query] ListOptions? options, CancellationToken cancellationToken);
}
