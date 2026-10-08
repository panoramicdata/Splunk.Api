using System.Text.Json.Serialization;

namespace Splunk.Api.Models.WorkloadManagement;

/// <summary>Creates a workload pool (<c>POST workloads/pools</c>).</summary>
public sealed class WorkloadPoolCreateRequest : WorkloadWeightsRequest
{
	/// <summary>The pool name (<c>name</c>).</summary>
	[JsonPropertyName("name")]
	public required string Name { get; init; }

	/// <summary>The category: <c>search</c>, <c>ingest</c> or <c>misc</c> (<c>category</c>).</summary>
	[JsonPropertyName("category")]
	public required string Category { get; init; }

	/// <summary>Makes this its category's default pool (<c>default_category_pool</c>).</summary>
	[JsonPropertyName("default_category_pool")]
	public bool? DefaultCategoryPool { get; init; }
}
