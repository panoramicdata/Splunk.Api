using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Cluster;

/// <summary>A bucket fixup priority level, from highest to lowest priority.</summary>
public enum ClusterFixupLevel
{
	/// <summary>A value this library does not recognise.</summary>
	Unknown = 0,

	/// <summary>Corrupted buckets.</summary>
	[JsonStringEnumMemberName("corruption")]
	Corruption,

	/// <summary>Hot buckets that need rolling or a committed size.</summary>
	[JsonStringEnumMemberName("streaming")]
	Streaming,

	/// <summary>Buckets without at least two raw data copies.</summary>
	[JsonStringEnumMemberName("data_safety")]
	DataSafety,

	/// <summary>Buckets without a primary copy.</summary>
	[JsonStringEnumMemberName("generation")]
	Generation,

	/// <summary>Buckets without the replication factor number of copies.</summary>
	[JsonStringEnumMemberName("replication_factor")]
	ReplicationFactor,

	/// <summary>Buckets without the search factor number of searchable copies.</summary>
	[JsonStringEnumMemberName("search_factor")]
	SearchFactor,

	/// <summary>Buckets whose delete files must be synchronised across peers.</summary>
	[JsonStringEnumMemberName("checksum_sync")]
	ChecksumSync
}
