using System.Text.Json.Serialization;

namespace Splunk.Api.Models.WorkloadManagement;

/// <summary>Turns search admission rules on or off (<c>POST workloads/policy/search_admission_control</c>).</summary>
public sealed class SearchAdmissionControlUpdateRequest : SplunkFormRequest
{
	/// <summary>Whether admission rules are enforced (<c>admission_rules_enabled</c>).</summary>
	[JsonPropertyName("admission_rules_enabled")]
	public required bool AdmissionRulesEnabled { get; init; }
}
