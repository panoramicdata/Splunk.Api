using Refit;
using Splunk.Api.Models;
using Splunk.Api.Models.Introspection;

namespace Splunk.Api.Interfaces;

/// <summary>Current host and Splunk process resource use (<c>server/status/resource-usage</c>).</summary>
public interface IResourceUsage
{
	/// <summary>Lists the resource usage resources (<c>GET server/status/resource-usage</c>).</summary>
	/// <param name="options">Paging, filtering and sorting, or <see langword="null"/> for Splunk's defaults.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with the entries <c>hostwide</c>, <c>iostats</c>, <c>iowait</c> and <c>splunk-processes</c>.</returns>
	[Get("services/server/status/resource-usage")]
	Task<SplunkFeed<SplunkDynamicContent>> ListAsync([Query] ListOptions? options, CancellationToken cancellationToken);

	/// <summary>Gets host-wide CPU, memory and paging use (<c>GET server/status/resource-usage/hostwide</c>).</summary>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with one entry, <c>result</c>.</returns>
	[Get("services/server/status/resource-usage/hostwide")]
	Task<SplunkFeed<HostResourceUsage>> GetHostwideAsync(CancellationToken cancellationToken);

	/// <summary>Lists the latest disk I/O statistics per device (<c>GET server/status/resource-usage/iostats</c>).</summary>
	/// <param name="options">Paging, filtering and sorting, or <see langword="null"/> for Splunk's defaults.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with one entry per device.</returns>
	[Get("services/server/status/resource-usage/iostats")]
	Task<SplunkFeed<DiskIoStatistics>> ListIoStatsAsync([Query] ListOptions? options, CancellationToken cancellationToken);

	/// <summary>Lists the resource use of each Splunk process (<c>GET server/status/resource-usage/splunk-processes</c>).</summary>
	/// <param name="options">Paging, filtering and sorting, or <see langword="null"/> for Splunk's defaults.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with one entry per process.</returns>
	[Get("services/server/status/resource-usage/splunk-processes")]
	Task<SplunkFeed<SplunkProcessUsage>> ListSplunkProcessesAsync([Query] ListOptions? options, CancellationToken cancellationToken);
}
