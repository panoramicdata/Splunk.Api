using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Cluster;

/// <summary>The latest search head cluster configuration agreed by Raft consensus (<c>shcluster/member/consensus</c>).</summary>
public sealed class ShClusterConsensus : SplunkContent
{
	/// <summary>The configuration ID.</summary>
	[JsonPropertyName("configuration_id")]
	public long? ConfigurationId { get; init; }

	/// <summary>The members, comma-separated, each as <c>scheme://host:port</c>.</summary>
	[JsonPropertyName("servers_list")]
	public string? ServersList { get; init; }
}
