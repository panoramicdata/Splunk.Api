using Refit;
using Splunk.Api.Models;
using Splunk.Api.Models.WorkloadManagement;

namespace Splunk.Api.Interfaces;

/// <summary>Workload management policies (<c>workloads/policy</c>). Not on Splunk Cloud.</summary>
public interface IWorkloadPolicy
{
	/// <summary>Gets whether search admission rules are enforced (<c>GET workloads/policy/search_admission_control</c>).</summary>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with one entry, <c>search_admission_control</c>.</returns>
	/// <remarks>Needs <c>list_workload_policy</c>. The reference's URL block says <c>search/workloads/policy/...</c>; the real path has no <c>search/</c>.</remarks>
	[Get("services/workloads/policy/search_admission_control")]
	Task<SplunkFeed<SearchAdmissionControl>> GetSearchAdmissionControlAsync(CancellationToken cancellationToken);

	/// <summary>Turns search admission rules on or off (<c>POST workloads/policy/search_admission_control</c>).</summary>
	/// <param name="request">Whether to enforce admission rules.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with the updated policy.</returns>
	/// <remarks>Needs <c>edit_workload_policy</c>.</remarks>
	[Post("services/workloads/policy/search_admission_control")]
	Task<SplunkFeed<SearchAdmissionControl>> UpdateSearchAdmissionControlAsync([Body] SearchAdmissionControlUpdateRequest request, CancellationToken cancellationToken);
}
