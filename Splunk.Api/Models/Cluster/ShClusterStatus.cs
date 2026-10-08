using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Cluster;

/// <summary>Search head cluster health, checked before a rolling upgrade or restart (<c>shcluster/status</c>).</summary>
public sealed class ShClusterStatus : SplunkContent
{
	/// <summary>The captain.</summary>
	[JsonPropertyName("captain")]
	public ShClusterStatusCaptain? Captain { get; init; }

	/// <summary>The members, by GUID.</summary>
	[JsonPropertyName("peers")]
	public IReadOnlyDictionary<string, ShClusterStatusMember> Peers { get; init; } = new Dictionary<string, ShClusterStatusMember>();
}
