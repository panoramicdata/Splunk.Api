using Refit;
using Splunk.Api.Models;
using Splunk.Api.Models.WorkloadManagement;

namespace Splunk.Api.Interfaces;

/// <summary>
/// Turning workload management on and off, and its Linux cgroup setup (<c>workloads/config</c>). Needs
/// <c>edit_workload_pools</c>. Not on Splunk Cloud.
/// </summary>
public interface IWorkloadConfig
{
	/// <summary>Enables workload management (<c>POST workloads/config/enable</c>).</summary>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A task that completes when workload management is enabled.</returns>
	/// <remarks>Fails unless the <see cref="GetPreflightChecksAsync"/> checks pass.</remarks>
	[Post("services/workloads/config/enable")]
	Task EnableAsync(CancellationToken cancellationToken);

	/// <summary>Disables workload management (<c>POST workloads/config/disable</c>).</summary>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A task that completes when workload management is disabled.</returns>
	[Post("services/workloads/config/disable")]
	Task DisableAsync(CancellationToken cancellationToken);

	/// <summary>Gets the name of Splunk's parent cgroup (<c>GET workloads/config/get-base-dirname</c>).</summary>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with one entry, <c>workload-pool-base-path</c>.</returns>
	[Get("services/workloads/config/get-base-dirname")]
	Task<SplunkFeed<WorkloadBaseDirectory>> GetBaseDirectoryAsync(CancellationToken cancellationToken);

	/// <summary>Runs the Linux preflight checks for workload management (<c>GET workloads/config/preflight-checks</c>).</summary>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with one entry, <c>workload-management-preflight-checks</c>.</returns>
	[Get("services/workloads/config/preflight-checks")]
	Task<SplunkFeed<WorkloadPreflightChecks>> GetPreflightChecksAsync(CancellationToken cancellationToken);

	/// <summary>Sets the name of Splunk's parent cgroup (<c>POST workloads/config/set-base-dirname</c>).</summary>
	/// <param name="request">The directory name.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A task that completes when the name is set.</returns>
	[Post("services/workloads/config/set-base-dirname")]
	Task SetBaseDirectoryAsync([Body] WorkloadBaseDirectoryRequest request, CancellationToken cancellationToken);
}
