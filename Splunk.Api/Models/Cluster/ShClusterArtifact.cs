using Splunk.Api.Serialization;
using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Cluster;

/// <summary>A search artifact managed by the search head cluster captain (<c>shcluster/captain/artifacts</c>).</summary>
/// <remarks>Only scheduled search results are artifacts; ad hoc searches are not listed.</remarks>
public sealed class ShClusterArtifact : SplunkContent
{
	/// <summary>The artifact size, in bytes.</summary>
	[JsonPropertyName("artifact_size")]
	public long? ArtifactSize { get; init; }

	/// <summary>The GUID of the member that ran the search.</summary>
	[JsonPropertyName("origin_guid")]
	public string? OriginGuid { get; init; }

	/// <summary>The replicas, by member GUID.</summary>
	[JsonPropertyName("peers")]
	public IReadOnlyDictionary<string, ShClusterArtifactReplica> Peers { get; init; } = new Dictionary<string, ShClusterArtifactReplica>();

	/// <summary>When deferred fixup of the artifact may start.</summary>
	[JsonPropertyName("service_after_time")]
	[JsonConverter(typeof(EpochSecondsConverter))]
	public DateTimeOffset? ServiceAfterTime { get; init; }

	/// <summary>The saved search name, where reported.</summary>
	[JsonPropertyName("label")]
	public string? Label { get; init; }

	/// <summary>The user the search ran as, where reported.</summary>
	[JsonPropertyName("user")]
	public string? User { get; init; }

	/// <summary>The artifact permissions as Splunk formats them, where reported.</summary>
	[JsonPropertyName("perms")]
	public string? Permissions { get; init; }
}
