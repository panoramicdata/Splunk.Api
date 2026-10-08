using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Deployment;

/// <summary>The knowledge bundle replication settings of a search head (<c>search/distributed/bundle/replication/config</c>).</summary>
public sealed class BundleReplicationConfig : SplunkContent
{
	/// <summary>The lookup size, in bytes, above which a warning is logged.</summary>
	[JsonPropertyName("concerningReplicatedFileSize")]
	public long? ConcerningReplicatedFileSize { get; init; }

	/// <summary>The connection timeout to indexers, in seconds.</summary>
	[JsonPropertyName("connectionTimeout")]
	public int? ConnectionTimeout { get; init; }

	/// <summary>The maximum bundle size, in bytes.</summary>
	[JsonPropertyName("maxBundleSize")]
	public long? MaxBundleSize { get; init; }

	/// <summary>The receive timeout, in seconds.</summary>
	[JsonPropertyName("receiveTimeout")]
	public int? ReceiveTimeout { get; init; }

	/// <summary>How often, in seconds, the replication thread checks whether replication is needed.</summary>
	[JsonPropertyName("replicationPeriod")]
	public int? ReplicationPeriod { get; init; }

	/// <summary>The replication policy, for example <c>classic</c>, <c>cascading</c> or <c>rfs</c>.</summary>
	[JsonPropertyName("replicationPolicy")]
	public string? ReplicationPolicy { get; init; }

	/// <summary>The number of replication threads.</summary>
	[JsonPropertyName("replicationThreads")]
	public int? ReplicationThreads { get; init; }

	/// <summary>The send timeout, in seconds.</summary>
	[JsonPropertyName("sendTimeout")]
	public int? SendTimeout { get; init; }

	/// <summary>How many replication cycles are kept in memory for <c>cycles</c>.</summary>
	[JsonPropertyName("statusQueueSize")]
	public int? StatusQueueSize { get; init; }
}
