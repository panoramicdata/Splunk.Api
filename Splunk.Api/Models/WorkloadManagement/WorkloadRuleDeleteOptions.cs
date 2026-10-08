using Refit;

namespace Splunk.Api.Models.WorkloadManagement;

/// <summary>The rule type for <c>DELETE workloads/rules/{name}</c>.</summary>
public sealed class WorkloadRuleDeleteOptions
{
	/// <summary><c>search_filter</c> to delete a search admission rule (<c>workload_rule_type</c>).</summary>
	[AliasAs("workload_rule_type")]
	public string? WorkloadRuleType { get; init; }
}
