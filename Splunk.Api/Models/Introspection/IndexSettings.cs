using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Introspection;

/// <summary>
/// The indexes.conf settings shared by creating and updating an index. Settings not modelled here (for example
/// <c>metric.*</c> or <c>tsidxWritingLevel</c>) go in <see cref="SplunkFormRequest.AdditionalParameters"/>.
/// </summary>
public abstract class IndexSettings : SplunkFormRequest
{
	/// <summary>The block signature size; 0 disables block signing (<c>blockSignSize</c>).</summary>
	[JsonPropertyName("blockSignSize")]
	public int? BlockSignSize { get; init; }

	/// <summary>Memory hint for bucket rebuilds, for example <c>auto</c> or <c>5MB</c> (<c>bucketRebuildMemoryHint</c>).</summary>
	[JsonPropertyName("bucketRebuildMemoryHint")]
	public string? BucketRebuildMemoryHint { get; init; }

	/// <summary>Where frozen buckets are archived; takes precedence over the script (<c>coldToFrozenDir</c>).</summary>
	[JsonPropertyName("coldToFrozenDir")]
	public string? ColdToFrozenDir { get; init; }

	/// <summary>The script run on buckets as they freeze (<c>coldToFrozenScript</c>).</summary>
	[JsonPropertyName("coldToFrozenScript")]
	public string? ColdToFrozenScript { get; init; }

	/// <summary>Whether buckets are repaired online after a crash (<c>enableOnlineBucketRepair</c>).</summary>
	[JsonPropertyName("enableOnlineBucketRepair")]
	public bool? EnableOnlineBucketRepair { get; init; }

	/// <summary>The age in seconds after which data is frozen; default 188697600 (<c>frozenTimePeriodInSecs</c>).</summary>
	[JsonPropertyName("frozenTimePeriodInSecs")]
	public long? FrozenTimePeriodInSecs { get; init; }

	/// <summary>The maximum bucket age for bloom filter backfill, for example <c>30d</c> (<c>maxBloomBackfillBucketAge</c>).</summary>
	[JsonPropertyName("maxBloomBackfillBucketAge")]
	public string? MaxBloomBackfillBucketAge { get; init; }

	/// <summary>The number of concurrent optimize processes per hot bucket (<c>maxConcurrentOptimizes</c>).</summary>
	[JsonPropertyName("maxConcurrentOptimizes")]
	public int? MaxConcurrentOptimizes { get; init; }

	/// <summary>The maximum hot bucket size: megabytes, <c>auto</c> or <c>auto_high_volume</c> (<c>maxDataSize</c>).</summary>
	[JsonPropertyName("maxDataSize")]
	public string? MaxDataSize { get; init; }

	/// <summary>The maximum number of hot buckets, a number or <c>auto</c> (<c>maxHotBuckets</c>).</summary>
	[JsonPropertyName("maxHotBuckets")]
	public string? MaxHotBuckets { get; init; }

	/// <summary>The time in seconds after which an idle hot bucket rolls; 0 disables (<c>maxHotIdleSecs</c>).</summary>
	[JsonPropertyName("maxHotIdleSecs")]
	public long? MaxHotIdleSecs { get; init; }

	/// <summary>The maximum time span of a hot bucket in seconds (<c>maxHotSpanSecs</c>).</summary>
	[JsonPropertyName("maxHotSpanSecs")]
	public long? MaxHotSpanSecs { get; init; }

	/// <summary>The memory in megabytes for indexing (<c>maxMemMB</c>).</summary>
	[JsonPropertyName("maxMemMB")]
	public int? MaxMemMB { get; init; }

	/// <summary>The maximum number of unique metadata entries; 0 is unlimited (<c>maxMetaEntries</c>).</summary>
	[JsonPropertyName("maxMetaEntries")]
	public long? MaxMetaEntries { get; init; }

	/// <summary>Seconds a hot bucket may stay unreplicated without acknowledgements (<c>maxTimeUnreplicatedNoAcks</c>).</summary>
	[JsonPropertyName("maxTimeUnreplicatedNoAcks")]
	public int? MaxTimeUnreplicatedNoAcks { get; init; }

	/// <summary>Seconds a hot bucket may stay unreplicated with acknowledgements (<c>maxTimeUnreplicatedWithAcks</c>).</summary>
	[JsonPropertyName("maxTimeUnreplicatedWithAcks")]
	public int? MaxTimeUnreplicatedWithAcks { get; init; }

	/// <summary>The maximum size of the index in megabytes; default 500000 (<c>maxTotalDataSizeMB</c>).</summary>
	[JsonPropertyName("maxTotalDataSizeMB")]
	public long? MaxTotalDataSizeMB { get; init; }

	/// <summary>The maximum number of warm buckets (<c>maxWarmDBCount</c>).</summary>
	[JsonPropertyName("maxWarmDBCount")]
	public int? MaxWarmDBCount { get; init; }

	/// <summary>How often raw data is synced to disk: seconds or <c>disable</c> (<c>minRawFileSyncSecs</c>).</summary>
	[JsonPropertyName("minRawFileSyncSecs")]
	public string? MinRawFileSyncSecs { get; init; }

	/// <summary>The minimum size of the streaming group queue (<c>minStreamGroupQueueSize</c>).</summary>
	[JsonPropertyName("minStreamGroupQueueSize")]
	public int? MinStreamGroupQueueSize { get; init; }

	/// <summary>Seconds between partial metadata syncs; 0 disables (<c>partialServiceMetaPeriod</c>).</summary>
	[JsonPropertyName("partialServiceMetaPeriod")]
	public int? PartialServiceMetaPeriod { get; init; }

	/// <summary>Seconds between checks of launched child processes (<c>processTrackerServiceInterval</c>).</summary>
	[JsonPropertyName("processTrackerServiceInterval")]
	public int? ProcessTrackerServiceInterval { get; init; }

	/// <summary>Events newer than now plus this many seconds are quarantined (<c>quarantineFutureSecs</c>).</summary>
	[JsonPropertyName("quarantineFutureSecs")]
	public long? QuarantineFutureSecs { get; init; }

	/// <summary>Events older than now minus this many seconds are quarantined (<c>quarantinePastSecs</c>).</summary>
	[JsonPropertyName("quarantinePastSecs")]
	public long? QuarantinePastSecs { get; init; }

	/// <summary>The target uncompressed size of raw data slices in bytes (<c>rawChunkSizeBytes</c>).</summary>
	[JsonPropertyName("rawChunkSizeBytes")]
	public int? RawChunkSizeBytes { get; init; }

	/// <summary>The replication factor in an indexer cluster: <c>0</c> or <c>auto</c> (<c>repFactor</c>).</summary>
	[JsonPropertyName("repFactor")]
	public string? RepFactor { get; init; }

	/// <summary>Seconds between checks for bucket roll and freeze (<c>rotatePeriodInSecs</c>).</summary>
	[JsonPropertyName("rotatePeriodInSecs")]
	public int? RotatePeriodInSecs { get; init; }

	/// <summary>Seconds between metadata syncs (<c>serviceMetaPeriod</c>).</summary>
	[JsonPropertyName("serviceMetaPeriod")]
	public int? ServiceMetaPeriod { get; init; }

	/// <summary>Whether metadata writes are synced to disk (<c>syncMeta</c>).</summary>
	[JsonPropertyName("syncMeta")]
	public bool? SyncMeta { get; init; }

	/// <summary>Seconds between checks of the index's disk usage (<c>throttleCheckPeriod</c>).</summary>
	[JsonPropertyName("throttleCheckPeriod")]
	public int? ThrottleCheckPeriod { get; init; }

	/// <summary>The data model summary path (<c>tstatsHomePath</c>).</summary>
	[JsonPropertyName("tstatsHomePath")]
	public string? TstatsHomePath { get; init; }
}
