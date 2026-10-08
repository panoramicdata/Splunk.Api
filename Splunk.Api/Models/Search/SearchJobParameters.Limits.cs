using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Search;

public abstract partial class SearchJobParameters
{
	/// <summary>The number of events that can be accessible in any one status bucket (<c>max_count</c>); Splunk's default is 10000.</summary>
	[JsonPropertyName("max_count")]
	public int? MaxCount { get; init; }

	/// <summary>The seconds to run before finalizing (<c>max_time</c>); 0 never finalizes.</summary>
	[JsonPropertyName("max_time")]
	public int? MaxTime { get; init; }

	/// <summary>The most status (timeline) buckets to generate (<c>status_buckets</c>); 0 (the default) keeps no timeline.</summary>
	[JsonPropertyName("status_buckets")]
	public int? StatusBuckets { get; init; }

	/// <summary>The seconds to keep the job after processing stops (<c>timeout</c>); Splunk's default is 86400.</summary>
	[JsonPropertyName("timeout")]
	public int? Timeout { get; init; }

	/// <summary>Cancels the job after this many seconds of inactivity (<c>auto_cancel</c>); 0 never cancels.</summary>
	[JsonPropertyName("auto_cancel")]
	public int? AutoCancel { get; init; }

	/// <summary>Finalizes the job after at least this many events are processed (<c>auto_finalize_ec</c>); 0 never finalizes.</summary>
	[JsonPropertyName("auto_finalize_ec")]
	public int? AutoFinalizeEventCount { get; init; }

	/// <summary>Pauses the job after this many seconds of inactivity (<c>auto_pause</c>); 0 never pauses.</summary>
	[JsonPropertyName("auto_pause")]
	public int? AutoPause { get; init; }

	/// <summary>How often, in seconds, the reduce phase runs on accumulated map values (<c>reduce_freq</c>).</summary>
	[JsonPropertyName("reduce_freq")]
	public int? ReduceFrequency { get; init; }

	/// <summary>Reuses a matching job no older than this many seconds instead of starting a new one (<c>reuse_max_seconds_ago</c>).</summary>
	[JsonPropertyName("reuse_max_seconds_ago")]
	public int? ReuseMaxSecondsAgo { get; init; }
}
