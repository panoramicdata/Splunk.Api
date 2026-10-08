using Splunk.Api.Serialization;
using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Introspection;

/// <summary>An index (<c>data/indexes</c>). Every other indexes.conf setting is in <see cref="SplunkContent.AdditionalProperties"/>.</summary>
public class SplunkIndex : SplunkContent
{
	/// <summary>The kind of data the index holds.</summary>
	[JsonPropertyName("datatype")]
	public IndexDataType DataType { get; init; }

	/// <summary>The hot and warm bucket path, as configured (<c>homePath</c>).</summary>
	[JsonPropertyName("homePath")]
	public string? HomePath { get; init; }

	/// <summary>The hot and warm bucket path with variables expanded (<c>homePath_expanded</c>).</summary>
	[JsonPropertyName("homePath_expanded")]
	public string? HomePathExpanded { get; init; }

	/// <summary>The cold bucket path, as configured (<c>coldPath</c>).</summary>
	[JsonPropertyName("coldPath")]
	public string? ColdPath { get; init; }

	/// <summary>The cold bucket path with variables expanded (<c>coldPath_expanded</c>).</summary>
	[JsonPropertyName("coldPath_expanded")]
	public string? ColdPathExpanded { get; init; }

	/// <summary>The thawed bucket path, as configured (<c>thawedPath</c>).</summary>
	[JsonPropertyName("thawedPath")]
	public string? ThawedPath { get; init; }

	/// <summary>The thawed bucket path with variables expanded (<c>thawedPath_expanded</c>).</summary>
	[JsonPropertyName("thawedPath_expanded")]
	public string? ThawedPathExpanded { get; init; }

	/// <summary>The data model summary path (<c>tstatsHomePath</c>).</summary>
	[JsonPropertyName("tstatsHomePath")]
	public string? TstatsHomePath { get; init; }

	/// <summary>Where frozen buckets are archived (<c>coldToFrozenDir</c>).</summary>
	[JsonPropertyName("coldToFrozenDir")]
	public string? ColdToFrozenDir { get; init; }

	/// <summary>The script run on frozen buckets (<c>coldToFrozenScript</c>).</summary>
	[JsonPropertyName("coldToFrozenScript")]
	public string? ColdToFrozenScript { get; init; }

	/// <summary>The current size of the index in megabytes (<c>currentDBSizeMB</c>).</summary>
	[JsonPropertyName("currentDBSizeMB")]
	public long CurrentDBSizeMB { get; init; }

	/// <summary>The maximum size of the index in megabytes (<c>maxTotalDataSizeMB</c>).</summary>
	[JsonPropertyName("maxTotalDataSizeMB")]
	public long? MaxTotalDataSizeMB { get; init; }

	/// <summary>The number of events in the index (<c>totalEventCount</c>).</summary>
	[JsonPropertyName("totalEventCount")]
	public long TotalEventCount { get; init; }

	/// <summary>The age in seconds after which data is frozen (<c>frozenTimePeriodInSecs</c>).</summary>
	[JsonPropertyName("frozenTimePeriodInSecs")]
	public long? FrozenTimePeriodInSecs { get; init; }

	/// <summary>The maximum hot bucket size: megabytes, <c>auto</c> or <c>auto_high_volume</c> (<c>maxDataSize</c>).</summary>
	[JsonPropertyName("maxDataSize")]
	public string? MaxDataSize { get; init; }

	/// <summary>The maximum number of hot buckets, a number or <c>auto</c> (<c>maxHotBuckets</c>).</summary>
	[JsonPropertyName("maxHotBuckets")]
	public string? MaxHotBuckets { get; init; }

	/// <summary>The maximum number of warm buckets (<c>maxWarmDBCount</c>).</summary>
	[JsonPropertyName("maxWarmDBCount")]
	public int? MaxWarmDBCount { get; init; }

	/// <summary>The maximum time span of a hot bucket in seconds (<c>maxHotSpanSecs</c>).</summary>
	[JsonPropertyName("maxHotSpanSecs")]
	public long? MaxHotSpanSecs { get; init; }

	/// <summary>The time in seconds after which an idle hot bucket rolls (<c>maxHotIdleSecs</c>).</summary>
	[JsonPropertyName("maxHotIdleSecs")]
	public long? MaxHotIdleSecs { get; init; }

	/// <summary>The earliest event time, or empty for an empty index (<c>minTime</c>).</summary>
	[JsonPropertyName("minTime")]
	public string? MinTime { get; init; }

	/// <summary>The latest event time, or empty for an empty index (<c>maxTime</c>).</summary>
	[JsonPropertyName("maxTime")]
	public string? MaxTime { get; init; }

	/// <summary>Whether this is an internal index such as <c>_internal</c> (<c>isInternal</c>).</summary>
	[JsonPropertyName("isInternal")]
	public bool IsInternal { get; init; }

	/// <summary>Whether the index is ready for use (<c>isReady</c>).</summary>
	[JsonPropertyName("isReady")]
	public bool IsReady { get; init; }

	/// <summary>Whether this is a virtual index (<c>isVirtual</c>).</summary>
	[JsonPropertyName("isVirtual")]
	public bool IsVirtual { get; init; }

	/// <summary>When the index was last initialised (<c>lastInitTime</c>).</summary>
	[JsonPropertyName("lastInitTime")]
	[JsonConverter(typeof(EpochSecondsConverter))]
	public DateTimeOffset? LastInitTime { get; init; }

	/// <summary>Whether raw data is compressed (<c>compressRawdata</c>).</summary>
	[JsonPropertyName("compressRawdata")]
	public bool? CompressRawdata { get; init; }

	/// <summary>Whether buckets are repaired online after a crash (<c>enableOnlineBucketRepair</c>).</summary>
	[JsonPropertyName("enableOnlineBucketRepair")]
	public bool? EnableOnlineBucketRepair { get; init; }

	/// <summary>Whether data integrity control is on (<c>enableDataIntegrityControl</c>).</summary>
	[JsonPropertyName("enableDataIntegrityControl")]
	public bool? EnableDataIntegrityControl { get; init; }

	/// <summary>The replication factor in an indexer cluster: <c>0</c> or <c>auto</c> (<c>repFactor</c>).</summary>
	[JsonPropertyName("repFactor")]
	public string? RepFactor { get; init; }

	/// <summary>The default database directory name (<c>defaultDatabase</c>).</summary>
	[JsonPropertyName("defaultDatabase")]
	public string? DefaultDatabase { get; init; }

	/// <summary>The journal compression, for example <c>zstd</c> (<c>journalCompression</c>).</summary>
	[JsonPropertyName("journalCompression")]
	public string? JournalCompression { get; init; }
}
