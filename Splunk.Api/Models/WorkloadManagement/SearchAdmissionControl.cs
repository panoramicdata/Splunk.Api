using System.Text.Json.Serialization;

namespace Splunk.Api.Models.WorkloadManagement;

/// <summary>The search admission control policy (<c>workloads/policy/search_admission_control</c>).</summary>
public sealed class SearchAdmissionControl : SplunkContent
{
	/// <summary>Whether admission rules are enforced (<c>admission_rules_enabled</c>).</summary>
	[JsonPropertyName("admission_rules_enabled")]
	public bool? AdmissionRulesEnabled { get; init; }
}
