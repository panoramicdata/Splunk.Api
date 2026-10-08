using Refit;
using Splunk.Api.Models;
using Splunk.Api.Models.WorkloadManagement;

namespace Splunk.Api.Interfaces;

/// <summary>Workload management status (<c>workloads/status</c>). Needs <c>list_workload_pools</c>. Not on Splunk Cloud.</summary>
public interface IWorkloadStatus
{
	/// <summary>Gets the status of workload management and admission control (<c>GET workloads/status</c>).</summary>
	/// <param name="options">Whether to include advanced details, or <see langword="null"/>.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with the entries <c>admission-control-status</c> and <c>workload-management-status</c>.</returns>
	[Get("services/workloads/status")]
	Task<SplunkFeed<WorkloadStatus>> GetAsync([Query] WorkloadStatusOptions? options, CancellationToken cancellationToken);
}
