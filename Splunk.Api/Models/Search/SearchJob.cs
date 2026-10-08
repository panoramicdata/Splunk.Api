using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Search;

/// <summary>
/// A search job's properties (<c>search/jobs/{search_id}</c>). Progress and counts change while the job runs; read the job
/// again to refresh them. Properties not modelled here (for example <c>performance</c> and <c>searchTelemetry</c>) are in
/// <see cref="SplunkContent.AdditionalProperties"/>.
/// </summary>
public sealed partial class SearchJob : SplunkContent
{
	/// <summary>The search ID (<c>sid</c>).</summary>
	[JsonPropertyName("sid")]
	public string? Sid { get; init; }

	/// <summary>The job's state.</summary>
	[JsonPropertyName("dispatchState")]
	public SearchDispatchState DispatchState { get; init; }

	/// <summary>Whether the job has finished, successfully or not.</summary>
	[JsonPropertyName("isDone")]
	public bool IsDone { get; init; }

	/// <summary>Whether the job failed; <see cref="Messages"/> says why.</summary>
	[JsonPropertyName("isFailed")]
	public bool IsFailed { get; init; }

	/// <summary>Whether the job was finalized (stopped early, keeping its results so far).</summary>
	[JsonPropertyName("isFinalized")]
	public bool IsFinalized { get; init; }

	/// <summary>Whether the job is paused.</summary>
	[JsonPropertyName("isPaused")]
	public bool IsPaused { get; init; }

	/// <summary>Whether the job is saved (kept for <see cref="DefaultSaveTtl"/> rather than <see cref="Ttl"/>).</summary>
	[JsonPropertyName("isSaved")]
	public bool IsSaved { get; init; }

	/// <summary>Whether the job is a saved search run by the scheduler.</summary>
	[JsonPropertyName("isSavedSearch")]
	public bool IsSavedSearch { get; init; }

	/// <summary>Whether the job is a real-time search.</summary>
	[JsonPropertyName("isRealTimeSearch")]
	public bool IsRealTimeSearch { get; init; }

	/// <summary>Whether the search process died before the job finished.</summary>
	[JsonPropertyName("isZombie")]
	public bool IsZombie { get; init; }

	/// <summary>Whether result previews are enabled.</summary>
	[JsonPropertyName("isPreviewEnabled")]
	public bool IsPreviewEnabled { get; init; }

	/// <summary>Approximate progress, from 0 to 1.</summary>
	[JsonPropertyName("doneProgress")]
	public double DoneProgress { get; init; }

	/// <summary>The number of events returned by the search so far.</summary>
	[JsonPropertyName("eventCount")]
	public long EventCount { get; init; }

	/// <summary>The number of events available from the events endpoint.</summary>
	[JsonPropertyName("eventAvailableCount")]
	public long EventAvailableCount { get; init; }

	/// <summary>The number of fields found in the events.</summary>
	[JsonPropertyName("eventFieldCount")]
	public int EventFieldCount { get; init; }

	/// <summary>Whether the events are streamed.</summary>
	[JsonPropertyName("eventIsStreaming")]
	public bool EventIsStreaming { get; init; }

	/// <summary>Whether events are not stored, and so unavailable from the events endpoint.</summary>
	[JsonPropertyName("eventIsTruncated")]
	public bool EventIsTruncated { get; init; }

	/// <summary>The part of the search before the first transforming command.</summary>
	[JsonPropertyName("eventSearch")]
	public string? EventSearch { get; init; }

	/// <summary>How the events are sorted: <c>asc</c>, <c>desc</c> or <c>none</c>.</summary>
	[JsonPropertyName("eventSorting")]
	public string? EventSorting { get; init; }

	/// <summary>The number of results (after transforming commands) so far.</summary>
	[JsonPropertyName("resultCount")]
	public long ResultCount { get; init; }

	/// <summary>The number of rows in the latest preview.</summary>
	[JsonPropertyName("resultPreviewCount")]
	public long ResultPreviewCount { get; init; }

	/// <summary>Whether the final results can be streamed (the search has no transforming commands).</summary>
	[JsonPropertyName("resultIsStreaming")]
	public bool ResultIsStreaming { get; init; }

	/// <summary>The number of events scanned (read off disk).</summary>
	[JsonPropertyName("scanCount")]
	public long ScanCount { get; init; }

	/// <summary>For real-time searches, the number of events dropped because the queue was full.</summary>
	[JsonPropertyName("dropCount")]
	public long DropCount { get; init; }

	/// <summary>The disk space the job uses, in bytes.</summary>
	[JsonPropertyName("diskUsage")]
	public long DiskUsage { get; init; }

	/// <summary>How long the search has run, in seconds.</summary>
	[JsonPropertyName("runDuration")]
	public double RunDuration { get; init; }

	/// <summary>The job's priority, 0 to 10.</summary>
	[JsonPropertyName("priority")]
	public int Priority { get; init; }

	/// <summary>The seconds the job's artifacts are kept after it finishes or was last touched.</summary>
	[JsonPropertyName("ttl")]
	public int Ttl { get; init; }

	/// <summary>The default time to live of a job, in seconds.</summary>
	[JsonPropertyName("defaultTTL")]
	public int DefaultTtl { get; init; }

	/// <summary>The time to live of a saved job, in seconds.</summary>
	[JsonPropertyName("defaultSaveTTL")]
	public int DefaultSaveTtl { get; init; }

	/// <summary>The maximum number of timeline buckets.</summary>
	[JsonPropertyName("statusBuckets")]
	public int StatusBuckets { get; init; }

	/// <summary>The number of previews generated so far.</summary>
	[JsonPropertyName("numPreviews")]
	public int NumPreviews { get; init; }
}
