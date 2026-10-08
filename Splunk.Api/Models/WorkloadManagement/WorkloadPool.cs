using System.Text.Json.Serialization;

namespace Splunk.Api.Models.WorkloadManagement;

/// <summary>A workload pool (<c>workloads/pools</c>).</summary>
public sealed class WorkloadPool : SplunkContent
{
	/// <summary>The pool's category: <c>search</c>, <c>ingest</c> or <c>misc</c> (<c>category</c>).</summary>
	[JsonPropertyName("category")]
	public string? Category { get; init; }

	/// <summary>Whether this is its category's default pool (<c>default_category_pool</c>).</summary>
	[JsonPropertyName("default_category_pool")]
	public bool? DefaultCategoryPool { get; init; }

	/// <summary>The pool's CPU weight within its category (<c>cpu_weight</c>).</summary>
	[JsonPropertyName("cpu_weight")]
	public int? CpuWeight { get; init; }

	/// <summary>The share of CPU allocated to the pool, in percent (<c>cpu_allocated_percent</c>).</summary>
	[JsonPropertyName("cpu_allocated_percent")]
	public double? CpuAllocatedPercent { get; init; }

	/// <summary>The cgroup CPU shares (<c>cpu_shares</c>).</summary>
	[JsonPropertyName("cpu_shares")]
	public long? CpuShares { get; init; }

	/// <summary>The pool's memory weight within its category (<c>mem_weight</c>).</summary>
	[JsonPropertyName("mem_weight")]
	public int? MemoryWeight { get; init; }

	/// <summary>The share of memory allocated to the pool, in percent (<c>mem_allocated_percent</c>).</summary>
	[JsonPropertyName("mem_allocated_percent")]
	public double? MemoryAllocatedPercent { get; init; }

	/// <summary>The cgroup memory limit (<c>mem_limit</c>).</summary>
	[JsonPropertyName("mem_limit")]
	public string? MemoryLimit { get; init; }
}
