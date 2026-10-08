using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Cluster;

/// <summary>A new cluster generation for a search head (<c>POST cluster/manager/generation</c>).</summary>
public sealed class ClusterGenerationCreateRequest : SplunkFormRequest
{
	/// <summary>The URI of the search head.</summary>
	[JsonPropertyName("name")]
	public required string Name { get; init; }

	/// <summary>How often, in seconds, the search head polls the manager for generation information (default 60).</summary>
	[JsonPropertyName("generation_poll_interval")]
	public int? GenerationPollInterval { get; init; }

	/// <summary>The search head's server name.</summary>
	[JsonPropertyName("label")]
	public string? Label { get; init; }

	/// <summary>The search head's management port.</summary>
	[JsonPropertyName("mgmt_port")]
	public string? ManagementPort { get; init; }

	/// <summary>The address the search head is reachable on.</summary>
	[JsonPropertyName("register_search_address")]
	public string? RegisterSearchAddress { get; init; }
}
