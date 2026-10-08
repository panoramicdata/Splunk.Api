using Refit;
using Splunk.Api.Models;
using Splunk.Api.Models.WorkloadManagement;

namespace Splunk.Api.Interfaces;

/// <summary>
/// Workload rules and search admission rules (<c>workloads/rules</c>). Reading needs <c>list_workload_rules</c>, changing
/// <c>edit_workload_rules</c>. Not on Splunk Cloud.
/// </summary>
public interface IWorkloadRules
{
	/// <summary>Lists the rules (<c>GET workloads/rules</c>).</summary>
	/// <param name="options">The rule type and paging, or <see langword="null"/> for workload rules with Splunk's defaults.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with one entry per rule.</returns>
	[Get("services/workloads/rules")]
	Task<SplunkFeed<WorkloadRule>> ListAsync([Query] WorkloadRuleListOptions? options, CancellationToken cancellationToken);

	/// <summary>Creates a rule, or changes an existing rule's order (<c>POST workloads/rules</c>).</summary>
	/// <param name="request">The rule.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with the rule.</returns>
	[Post("services/workloads/rules")]
	Task<SplunkFeed<WorkloadRule>> CreateAsync([Body] WorkloadRuleCreateRequest request, CancellationToken cancellationToken);

	/// <summary>Deletes a rule (<c>DELETE workloads/rules/{name}</c>).</summary>
	/// <param name="name">The rule name.</param>
	/// <param name="options">The rule type (<c>search_filter</c> for an admission rule), or <see langword="null"/> for a workload rule.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A task that completes when the rule is deleted.</returns>
	/// <remarks>
	/// The reference lists this as <c>DELETE workloads/rules</c>; Splunk 10.6 answers that with "Cannot perform action
	/// DELETE without a target name", so the rule is addressed by name in the path.
	/// </remarks>
	[Delete("services/workloads/rules/{name}")]
	Task DeleteAsync(string name, [Query] WorkloadRuleDeleteOptions? options, CancellationToken cancellationToken);
}
