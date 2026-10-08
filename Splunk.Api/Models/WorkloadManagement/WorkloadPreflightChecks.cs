using System.Text.Json;
using System.Text.Json.Serialization;

namespace Splunk.Api.Models.WorkloadManagement;

/// <summary>The Linux preflight checks for workload management (<c>workloads/config/preflight-checks</c>).</summary>
public sealed class WorkloadPreflightChecks : SplunkContent
{
	/// <summary>The overall result.</summary>
	[JsonPropertyName("general")]
	public WorkloadPreflightSummary? General { get; init; }

	/// <summary>
	/// Each check, keyed by its identifier (for example <c>cgroup_version</c> or <c>platform_type</c>); read from the
	/// content's object-valued keys, which vary with the Splunk version.
	/// </summary>
	[JsonIgnore]
	public IReadOnlyDictionary<string, WorkloadPreflightCheck> Checks
		=> AdditionalProperties
			.Where(p => p.Value.ValueKind == JsonValueKind.Object)
			.ToDictionary(p => p.Key, p => p.Value.Deserialize<WorkloadPreflightCheck>(SplunkJson.Options)!, StringComparer.Ordinal);
}
