using System.Text.Json.Serialization;

namespace Splunk.Api.Models.WorkloadManagement;

/// <summary>
/// Creates or changes a workload rule or search admission rule (<c>POST workloads/rules</c>). Undocumented settings such as
/// <c>schedule</c>, <c>start_time</c> and <c>end_time</c> go in <see cref="SplunkFormRequest.AdditionalParameters"/>.
/// </summary>
public sealed class WorkloadRuleCreateRequest : SplunkFormRequest
{
	/// <summary>The rule name (<c>name</c>).</summary>
	[JsonPropertyName("name")]
	public required string Name { get; init; }

	/// <summary>
	/// The condition (<c>predicate</c>): <c>type=value</c> terms joined with <c>AND</c>, <c>OR</c>, <c>NOT</c> and
	/// parentheses. Types: <c>app</c>, <c>role</c>, <c>index</c>, <c>user</c>, <c>search_type</c>, <c>search_mode</c>,
	/// <c>search_time_range</c>, <c>runtime</c>.
	/// </summary>
	[JsonPropertyName("predicate")]
	public required string Predicate { get; init; }

	/// <summary>The action: <c>move</c>, <c>alert</c> or <c>abort</c> for workload rules, <c>filter</c> for admission rules (<c>action</c>).</summary>
	[JsonPropertyName("action")]
	public required string Action { get; init; }

	/// <summary>The target pool, required when <see cref="Action"/> is <c>move</c> (<c>workload_pool</c>).</summary>
	[JsonPropertyName("workload_pool")]
	public string? WorkloadPool { get; init; }

	/// <summary>The rule's position in evaluation order; existing rules only (<c>order</c>).</summary>
	[JsonPropertyName("order")]
	public int? Order { get; init; }

	/// <summary><c>search_filter</c> for a search admission rule (<c>workload_rule_type</c>).</summary>
	[JsonPropertyName("workload_rule_type")]
	public string? WorkloadRuleType { get; init; }
}
