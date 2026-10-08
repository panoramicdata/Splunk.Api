using System.Text.Json.Serialization;

namespace Splunk.Api.Models.WorkloadManagement;

/// <summary>A workload rule or search admission rule (<c>workloads/rules</c>).</summary>
public sealed class WorkloadRule : SplunkContent
{
	/// <summary>The condition, for example <c>app=search AND role=power</c> (<c>predicate</c>).</summary>
	[JsonPropertyName("predicate")]
	public string? Predicate { get; init; }

	/// <summary>The action: <c>move</c>, <c>alert</c> or <c>abort</c> for workload rules, <c>filter</c> for admission rules (<c>action</c>).</summary>
	[JsonPropertyName("action")]
	public string? Action { get; init; }

	/// <summary>The pool searches are placed in or moved to (<c>workload_pool</c>).</summary>
	[JsonPropertyName("workload_pool")]
	public string? WorkloadPool { get; init; }

	/// <summary>The rule's position in evaluation order (<c>order</c>).</summary>
	[JsonPropertyName("order")]
	public int? Order { get; init; }

	/// <summary>The message shown to users when an admission rule filters a search (<c>user_message</c>).</summary>
	[JsonPropertyName("user_message")]
	public string? UserMessage { get; init; }
}
