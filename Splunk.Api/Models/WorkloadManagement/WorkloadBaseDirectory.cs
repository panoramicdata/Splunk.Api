using System.Text.Json.Serialization;

namespace Splunk.Api.Models.WorkloadManagement;

/// <summary>The name of Splunk's parent cgroup (<c>workloads/config/get-base-dirname</c>).</summary>
public sealed class WorkloadBaseDirectory : SplunkContent
{
	/// <summary>The cgroup directory name, for example <c>splunk</c> (<c>workload_pool_base_dir_name</c>).</summary>
	[JsonPropertyName("workload_pool_base_dir_name")]
	public string? Name { get; init; }
}
