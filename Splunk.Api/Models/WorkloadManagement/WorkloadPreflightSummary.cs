using System.Text.Json.Serialization;

namespace Splunk.Api.Models.WorkloadManagement;

/// <summary>The overall result of the workload management preflight checks (<c>general</c>).</summary>
public sealed class WorkloadPreflightSummary
{
	/// <summary>Whether every check passed (<c>preflight_checks_status</c>).</summary>
	[JsonPropertyName("preflight_checks_status")]
	public bool Passed { get; init; }

	/// <summary>Whether splunkd runs under systemd (<c>systemd_present</c>).</summary>
	[JsonPropertyName("systemd_present")]
	public bool SystemdPresent { get; init; }
}
