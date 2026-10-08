using System.Text.Json.Serialization;

namespace Splunk.Api.Models.WorkloadManagement;

/// <summary>A workload category: <c>search</c>, <c>ingest</c> or <c>misc</c> (<c>workloads/categories</c>).</summary>
public sealed class WorkloadCategory : WorkloadAllocation
{
	/// <summary>The sum of every category's CPU weight (<c>cpu_weight_sum</c>).</summary>
	[JsonPropertyName("cpu_weight_sum")]
	public int? CpuWeightSum { get; init; }
}
