using Refit;
using Splunk.Api.Models;
using Splunk.Api.Models.Introspection;

namespace Splunk.Api.Interfaces;

/// <summary>System introspection: indexer state and search framework statistics (<c>server/introspection</c>).</summary>
public interface IIntrospection
{
	/// <summary>Lists the introspection resources (<c>GET server/introspection</c>).</summary>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with one entry per resource, for example <c>indexer</c>, <c>kvstore</c>, <c>queues</c>, <c>search</c>.</returns>
	[Get("services/server/introspection")]
	Task<SplunkFeed<SplunkDynamicContent>> ListAsync(CancellationToken cancellationToken);

	/// <summary>Gets the indexer's state (<c>GET server/introspection/indexer</c>).</summary>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with one entry, <c>indexer</c>.</returns>
	[Get("services/server/introspection/indexer")]
	Task<SplunkFeed<IndexerStatus>> GetIndexerAsync(CancellationToken cancellationToken);

	/// <summary>Lists the timings of every search dispatch task (<c>GET server/introspection/search/dispatch</c>).</summary>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with one entry per task.</returns>
	[Get("services/server/introspection/search/dispatch")]
	Task<SplunkFeed<SearchDispatchTiming>> ListSearchDispatchAsync(CancellationToken cancellationToken);

	/// <summary>Gets the time taken to reap obsolete knowledge bundles (<c>GET server/introspection/search/dispatch/Bundle_Directory_Reaper</c>).</summary>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with one entry.</returns>
	[Get("services/server/introspection/search/dispatch/Bundle_Directory_Reaper")]
	Task<SplunkFeed<SearchDispatchTiming>> GetBundleDirectoryReaperAsync(CancellationToken cancellationToken);

	/// <summary>Gets the time taken to compute user search quotas (<c>GET server/introspection/search/dispatch/Compute_User_Search_Quota</c>).</summary>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with one entry.</returns>
	[Get("services/server/introspection/search/dispatch/Compute_User_Search_Quota")]
	Task<SplunkFeed<SearchDispatchTiming>> GetComputeUserSearchQuotaAsync(CancellationToken cancellationToken);

	/// <summary>Gets the time taken to reap stale dispatch artifacts (<c>GET server/introspection/search/dispatch/Dispatch_Directory_Reaper</c>).</summary>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with one entry.</returns>
	[Get("services/server/introspection/search/dispatch/Dispatch_Directory_Reaper")]
	Task<SplunkFeed<SearchDispatchTiming>> GetDispatchDirectoryReaperAsync(CancellationToken cancellationToken);

	/// <summary>Gets the time searches spend preprocessing before they start (<c>GET server/introspection/search/dispatch/Search_StartUp_Time</c>).</summary>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with one entry.</returns>
	[Get("services/server/introspection/search/dispatch/Search_StartUp_Time")]
	Task<SplunkFeed<SearchDispatchTiming>> GetSearchStartUpTimeAsync(CancellationToken cancellationToken);

	/// <summary>Gets knowledge bundle replication metrics (<c>GET server/introspection/search/distributed</c>).</summary>
	/// <param name="options">Paging, filtering and sorting, or <see langword="null"/> for Splunk's defaults.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with the entries <c>per_searchhead_metrics</c> and <c>window_metrics</c>.</returns>
	[Get("services/server/introspection/search/distributed")]
	Task<SplunkFeed<BundleReplicationMetrics>> GetSearchDistributedAsync([Query] ListOptions? options, CancellationToken cancellationToken);

	/// <summary>Gets the latest scheduled search priority scores (<c>GET server/introspection/search/saved</c>).</summary>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with one entry per scheduled search; empty when nothing is scheduled.</returns>
	[Get("services/server/introspection/search/saved")]
	Task<SplunkFeed<SplunkDynamicContent>> GetSearchSavedAsync(CancellationToken cancellationToken);
}
