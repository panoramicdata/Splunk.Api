using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Search;

/// <summary>Controls a search job (<c>POST search/jobs/{search_id}/control</c>).</summary>
public sealed class SearchJobControlRequest : SplunkFormRequest
{
	/// <summary>The action to take (<c>action</c>).</summary>
	[JsonPropertyName("action")]
	public required SearchJobAction Action { get; init; }

	/// <summary>The new time to live in seconds, for <see cref="SearchJobAction.SetTtl"/> (<c>ttl</c>).</summary>
	[JsonPropertyName("ttl")]
	public int? Ttl { get; init; }

	/// <summary>The new priority, 0 to 10, for <see cref="SearchJobAction.SetPriority"/> (<c>priority</c>).</summary>
	[JsonPropertyName("priority")]
	public int? Priority { get; init; }

	/// <summary>The workload pool, for <see cref="SearchJobAction.SetWorkloadPool"/> (<c>workload_pool</c>).</summary>
	[JsonPropertyName("workload_pool")]
	public string? WorkloadPool { get; init; }
}
