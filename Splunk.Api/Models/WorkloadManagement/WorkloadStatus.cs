using System.Text.Json;
using System.Text.Json.Serialization;

namespace Splunk.Api.Models.WorkloadManagement;

/// <summary>
/// One part of the workload management status (<c>workloads/status</c>): the <c>workload-management-status</c> entry has
/// <see cref="General"/> and <see cref="WorkloadRules"/>, the <c>admission-control-status</c> entry has
/// <see cref="Enabled"/> and <see cref="SearchFilterRules"/>.
/// </summary>
public sealed class WorkloadStatus : SplunkContent
{
	/// <summary>General status (<c>general</c>).</summary>
	[JsonPropertyName("general")]
	public WorkloadStatusGeneral? General { get; init; }

	/// <summary>Whether admission control is enabled (<c>enabled</c>).</summary>
	[JsonPropertyName("enabled")]
	public bool? Enabled { get; init; }

	/// <summary>The workload rules in force (<c>workload-rules</c>); <see langword="null"/> when there are none.</summary>
	[JsonPropertyName("workload-rules")]
	public JsonElement? WorkloadRules { get; init; }

	/// <summary>The admission rules in force (<c>search-filter-rules</c>); <see langword="null"/> when there are none.</summary>
	[JsonPropertyName("search-filter-rules")]
	public JsonElement? SearchFilterRules { get; init; }
}
