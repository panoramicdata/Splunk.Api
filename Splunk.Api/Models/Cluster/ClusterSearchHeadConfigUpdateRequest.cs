using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Cluster;

/// <summary>Changes a search head's cluster configuration (<c>POST cluster/searchhead/searchheadconfig/{name}</c>).</summary>
public sealed class ClusterSearchHeadConfigUpdateRequest : SplunkFormRequest
{
	/// <summary>The cluster manager's URI.</summary>
	[JsonPropertyName("manager_uri")]
	public string? ManagerUri { get; init; }

	/// <summary>The shared cluster secret (<c>pass4SymmKey</c>).</summary>
	[JsonPropertyName("secret")]
	public string? Secret { get; init; }
}
