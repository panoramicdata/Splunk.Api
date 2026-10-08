using System.Text.Json.Serialization;

namespace Splunk.Api.Models.WorkloadManagement;

/// <summary>Sets the name of Splunk's parent cgroup (<c>POST workloads/config/set-base-dirname</c>).</summary>
public sealed class WorkloadBaseDirectoryRequest : SplunkFormRequest
{
	/// <summary>The cgroup directory name (<c>workload_pool_base_dir_name</c>).</summary>
	[JsonPropertyName("workload_pool_base_dir_name")]
	public required string Name { get; init; }
}
