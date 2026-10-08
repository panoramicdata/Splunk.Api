using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Cluster;

/// <summary>A site of a multisite indexer cluster (<c>cluster/manager/sites</c>).</summary>
public sealed class ClusterSite : SplunkContent
{
	/// <summary>The site's peers, by GUID.</summary>
	[JsonPropertyName("peers")]
	public IReadOnlyDictionary<string, ClusterSitePeer> Peers { get; init; } = new Dictionary<string, ClusterSitePeer>();
}
