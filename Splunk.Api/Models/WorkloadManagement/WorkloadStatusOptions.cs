using Refit;

namespace Splunk.Api.Models.WorkloadManagement;

/// <summary>Options for <c>GET workloads/status</c>.</summary>
public sealed class WorkloadStatusOptions
{
	/// <summary>Whether to include advanced (per-pool) details (<c>advanced</c>).</summary>
	[AliasAs("advanced")]
	public bool? Advanced { get; init; }
}
