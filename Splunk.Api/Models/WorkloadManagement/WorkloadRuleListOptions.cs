using Refit;

namespace Splunk.Api.Models.WorkloadManagement;

/// <summary>Paging and the rule type for <c>GET workloads/rules</c>.</summary>
public sealed class WorkloadRuleListOptions : ListOptions
{
	/// <summary><c>search_filter</c> for admission rules only, or <c>all</c> (<c>workload_rule_type</c>).</summary>
	[AliasAs("workload_rule_type")]
	public string? WorkloadRuleType { get; init; }
}
