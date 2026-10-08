using Refit;
using Splunk.Api.Models;
using Splunk.Api.Models.WorkloadManagement;

namespace Splunk.Api.Interfaces;

/// <summary>
/// Workload pools (<c>workloads/pools</c>). Workload management needs Linux; reading needs <c>list_workload_pools</c>,
/// creating <c>edit_workload_pools</c>. Not on Splunk Cloud.
/// </summary>
public interface IWorkloadPools
{
	/// <summary>Lists the workload pools (<c>GET workloads/pools</c>).</summary>
	/// <param name="options">Paging, filtering and sorting, or <see langword="null"/> for Splunk's defaults.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with one entry per pool.</returns>
	[Get("services/workloads/pools")]
	Task<SplunkFeed<WorkloadPool>> ListAsync([Query] ListOptions? options, CancellationToken cancellationToken);

	/// <summary>Creates a workload pool (<c>POST workloads/pools</c>).</summary>
	/// <param name="request">The pool.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with the new pool.</returns>
	[Post("services/workloads/pools")]
	Task<SplunkFeed<WorkloadPool>> CreateAsync([Body] WorkloadPoolCreateRequest request, CancellationToken cancellationToken);
}
