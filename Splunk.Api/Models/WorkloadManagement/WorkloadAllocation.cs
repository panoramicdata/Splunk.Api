using System.Text.Json.Serialization;

namespace Splunk.Api.Models.WorkloadManagement;

/// <summary>
/// The CPU and memory allocation Splunk reports for a workload category (<see cref="WorkloadCategory"/>) and for a
/// workload pool (<see cref="WorkloadPool"/>).
/// </summary>
public abstract class WorkloadAllocation : SplunkContent
{
	/// <summary>The CPU weight (<c>cpu_weight</c>); a pool's weight is relative to the other pools of its category.</summary>
	[JsonPropertyName("cpu_weight")]
	public int? CpuWeight { get; init; }

	/// <summary>The share of CPU allocated, in percent (<c>cpu_allocated_percent</c>).</summary>
	[JsonPropertyName("cpu_allocated_percent")]
	public double? CpuAllocatedPercent { get; init; }

	/// <summary>The cgroup CPU shares (<c>cpu_shares</c>); 0 while workload management is disabled.</summary>
	[JsonPropertyName("cpu_shares")]
	public long? CpuShares { get; init; }

	/// <summary>The memory weight (<c>mem_weight</c>); a pool's weight is relative to the other pools of its category.</summary>
	[JsonPropertyName("mem_weight")]
	public int? MemoryWeight { get; init; }

	/// <summary>The share of memory allocated, in percent (<c>mem_allocated_percent</c>).</summary>
	[JsonPropertyName("mem_allocated_percent")]
	public double? MemoryAllocatedPercent { get; init; }

	/// <summary>The cgroup memory limit (<c>mem_limit</c>); <c>0</c> while workload management is disabled.</summary>
	[JsonPropertyName("mem_limit")]
	public string? MemoryLimit { get; init; }
}
