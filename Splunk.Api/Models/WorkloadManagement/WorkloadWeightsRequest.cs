using System.Text.Json.Serialization;

namespace Splunk.Api.Models.WorkloadManagement;

/// <summary>Changes a workload category's resource weights (<c>POST workloads/categories/{name}</c>).</summary>
public class WorkloadWeightsRequest : SplunkFormRequest
{
	/// <summary>The CPU weight, 1 to 100 (<c>cpu_weight</c>).</summary>
	[JsonPropertyName("cpu_weight")]
	public int? CpuWeight { get; init; }

	/// <summary>The memory weight, 1 to 100 (<c>mem_weight</c>).</summary>
	[JsonPropertyName("mem_weight")]
	public int? MemoryWeight { get; init; }
}
