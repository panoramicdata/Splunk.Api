using System.Text.Json.Serialization;

namespace Splunk.Api.Models.WorkloadManagement;

/// <summary>One workload management preflight check (<c>workloads/config/preflight-checks</c>).</summary>
public sealed class WorkloadPreflightCheck
{
	/// <summary>What the check verifies, for example <c>Operating System</c>.</summary>
	[JsonPropertyName("title")]
	public string? Title { get; init; }

	/// <summary>Whether the check passed (<c>preflight_check_status</c>).</summary>
	[JsonPropertyName("preflight_check_status")]
	public bool Passed { get; init; }

	/// <summary>How to fix a failure.</summary>
	[JsonPropertyName("mitigation")]
	public string? Mitigation { get; init; }
}
