using System.Text.Json.Serialization;

namespace Splunk.Api.Models.WorkloadManagement;

/// <summary>A workload category: <c>search</c>, <c>ingest</c> or <c>misc</c> (<c>workloads/categories</c>).</summary>
public sealed class WorkloadCategory : SplunkContent
{
	/// <summary>The category's CPU weight (<c>cpu_weight</c>).</summary>
	[JsonPropertyName("cpu_weight")]
	public int? CpuWeight { get; init; }

	/// <summary>The sum of every category's CPU weight (<c>cpu_weight_sum</c>).</summary>
	[JsonPropertyName("cpu_weight_sum")]
	public int? CpuWeightSum { get; init; }

	/// <summary>The share of CPU allocated to the category, in percent (<c>cpu_allocated_percent</c>).</summary>
	[JsonPropertyName("cpu_allocated_percent")]
	public double? CpuAllocatedPercent { get; init; }

	/// <summary>The cgroup CPU shares (<c>cpu_shares</c>); 0 while workload management is disabled.</summary>
	[JsonPropertyName("cpu_shares")]
	public long? CpuShares { get; init; }

	/// <summary>The category's memory weight (<c>mem_weight</c>).</summary>
	[JsonPropertyName("mem_weight")]
	public int? MemoryWeight { get; init; }

	/// <summary>The share of memory allocated to the category, in percent (<c>mem_allocated_percent</c>).</summary>
	[JsonPropertyName("mem_allocated_percent")]
	public double? MemoryAllocatedPercent { get; init; }

	/// <summary>The cgroup memory limit (<c>mem_limit</c>); <c>0</c> while workload management is disabled.</summary>
	[JsonPropertyName("mem_limit")]
	public string? MemoryLimit { get; init; }
}
