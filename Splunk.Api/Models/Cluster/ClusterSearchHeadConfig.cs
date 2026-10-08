using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Cluster;

/// <summary>A cluster a search head belongs to (<c>cluster/searchhead/searchheadconfig</c>).</summary>
public sealed class ClusterSearchHeadConfig : SplunkContent
{
	/// <summary>The cluster manager's URI.</summary>
	[JsonPropertyName("manager_uri")]
	public string? ManagerUri { get; init; }

	/// <summary>The shared cluster secret, masked by Splunk.</summary>
	[JsonPropertyName("secret")]
	public string? Secret { get; init; }
}
