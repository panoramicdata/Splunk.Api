using Splunk.Api.Serialization;
using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Search;

public sealed partial class SearchJob
{
	/// <summary>The earliest time of the data the job covers, snapped to the indexed data.</summary>
	[JsonPropertyName("earliestTime")]
	public DateTimeOffset? EarliestTime { get; init; }

	/// <summary>The latest time of the data the job covers, snapped to the indexed data.</summary>
	[JsonPropertyName("latestTime")]
	public DateTimeOffset? LatestTime { get; init; }

	/// <summary>The earliest time from which no events are later scanned; with the time bounds it measures progress.</summary>
	[JsonPropertyName("cursorTime")]
	public DateTimeOffset? CursorTime { get; init; }

	/// <summary>The earliest time named in the search itself, when it names one.</summary>
	[JsonPropertyName("searchEarliestTime")]
	[JsonConverter(typeof(EpochSecondsConverter))]
	public DateTimeOffset? SearchEarliestTime { get; init; }

	/// <summary>The latest time named in the search itself, when it names one.</summary>
	[JsonPropertyName("searchLatestTime")]
	[JsonConverter(typeof(EpochSecondsConverter))]
	public DateTimeOffset? SearchLatestTime { get; init; }

	/// <summary>The search string as submitted.</summary>
	[JsonPropertyName("search")]
	public string? Search { get; init; }

	/// <summary>The search after optimization.</summary>
	[JsonPropertyName("optimizedSearch")]
	public string? OptimizedSearch { get; init; }

	/// <summary>The search sent to each search peer.</summary>
	[JsonPropertyName("remoteSearch")]
	public string? RemoteSearch { get; init; }

	/// <summary>The reporting (transforming) part of the search, if any.</summary>
	[JsonPropertyName("reportSearch")]
	public string? ReportSearch { get; init; }

	/// <summary>The positive keywords of the search.</summary>
	[JsonPropertyName("keywords")]
	public string? Keywords { get; init; }

	/// <summary>The job's label: the saved search name for scheduled jobs.</summary>
	[JsonPropertyName("label")]
	public string? Label { get; init; }

	/// <summary>For saved searches, who started the job (for example <c>scheduler</c>).</summary>
	[JsonPropertyName("delegate")]
	public string? Delegate { get; init; }

	/// <summary>What started the job, for example <c>rest:jobs</c> or <c>scheduler</c>.</summary>
	[JsonPropertyName("provenance")]
	public string? Provenance { get; init; }

	/// <summary>The workload pool the job runs in, if any.</summary>
	[JsonPropertyName("workload_pool")]
	public string? WorkloadPool { get; init; }

	/// <summary>Errors, warnings and information about the job; a failed job says why here.</summary>
	[JsonPropertyName("messages")]
	public IReadOnlyList<SplunkMessage> Messages { get; init; } = [];

	/// <summary>The parameters the job was created with, as Splunk recorded them.</summary>
	[JsonPropertyName("request")]
	public IReadOnlyDictionary<string, string> Request { get; init; } = new Dictionary<string, string>();

	/// <summary>Runtime settings, such as <c>auto_cancel</c> and <c>auto_pause</c>.</summary>
	[JsonPropertyName("runtime")]
	public IReadOnlyDictionary<string, string> Runtime { get; init; } = new Dictionary<string, string>();

	/// <summary>The search peers the job contacted.</summary>
	[JsonPropertyName("searchProviders")]
	public IReadOnlyList<string> SearchProviders { get; init; } = [];
}
