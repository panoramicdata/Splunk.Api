using System.Text.Json.Serialization;

namespace Splunk.Api.Models.WorkloadManagement;

/// <summary>A workload pool (<c>workloads/pools</c>).</summary>
public sealed class WorkloadPool : WorkloadAllocation
{
	/// <summary>The pool's category: <c>search</c>, <c>ingest</c> or <c>misc</c> (<c>category</c>).</summary>
	[JsonPropertyName("category")]
	public string? Category { get; init; }

	/// <summary>Whether this is its category's default pool (<c>default_category_pool</c>).</summary>
	[JsonPropertyName("default_category_pool")]
	public bool? DefaultCategoryPool { get; init; }
}
