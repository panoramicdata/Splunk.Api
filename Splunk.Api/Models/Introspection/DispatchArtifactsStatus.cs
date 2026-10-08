using System.Text.Json;
using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Introspection;

/// <summary>
/// Counts and sizes of search job artifacts in the dispatch directory (<c>server/status/dispatch-artifacts</c>). Other
/// counters (subsearch, real-time and cache sizes) are in <see cref="SplunkContent.AdditionalProperties"/>.
/// </summary>
public sealed class DispatchArtifactsStatus : SplunkContent
{
	/// <summary>Ad hoc search artifacts (<c>adhoc_count</c>).</summary>
	[JsonPropertyName("adhoc_count")]
	public long AdhocCount { get; init; }

	/// <summary>Size of ad hoc search artifacts in megabytes (<c>adhoc_size_mb</c>).</summary>
	[JsonPropertyName("adhoc_size_mb")]
	public double AdhocSizeMB { get; init; }

	/// <summary>Scheduled search artifacts (<c>scheduled_count</c>).</summary>
	[JsonPropertyName("scheduled_count")]
	public long ScheduledCount { get; init; }

	/// <summary>Size of scheduled search artifacts in megabytes (<c>scheduled_size_mb</c>).</summary>
	[JsonPropertyName("scheduled_size_mb")]
	public double ScheduledSizeMB { get; init; }

	/// <summary>Completed search artifacts (<c>completed_count</c>).</summary>
	[JsonPropertyName("completed_count")]
	public long CompletedCount { get; init; }

	/// <summary>Size of completed search artifacts in megabytes (<c>completed_size_mb</c>).</summary>
	[JsonPropertyName("completed_size_mb")]
	public double CompletedSizeMB { get; init; }

	/// <summary>Incomplete search artifacts (<c>incomple_count</c>, Splunk's spelling).</summary>
	[JsonPropertyName("incomple_count")]
	public long IncompleteCount { get; init; }

	/// <summary>Size of incomplete search artifacts in megabytes (<c>incomple_size_mb</c>, Splunk's spelling).</summary>
	[JsonPropertyName("incomple_size_mb")]
	public double IncompleteSizeMB { get; init; }

	/// <summary>Invalid artifacts (<c>invalid_count</c>).</summary>
	[JsonPropertyName("invalid_count")]
	public long InvalidCount { get; init; }

	/// <summary>Artifacts of remote searches (<c>remote_count</c>).</summary>
	[JsonPropertyName("remote_count")]
	public long RemoteCount { get; init; }

	/// <summary>Temporary dispatch directories (<c>temp_dispatch_count</c>).</summary>
	[JsonPropertyName("temp_dispatch_count")]
	public long TempDispatchCount { get; init; }

	/// <summary>The apps with the most artifacts, or <see langword="null"/> (<c>top_apps</c>).</summary>
	[JsonPropertyName("top_apps")]
	public JsonElement? TopApps { get; init; }

	/// <summary>The saved searches with the most artifacts, or <see langword="null"/> (<c>top_named_searches</c>).</summary>
	[JsonPropertyName("top_named_searches")]
	public JsonElement? TopNamedSearches { get; init; }

	/// <summary>The users with the most artifacts, or <see langword="null"/> (<c>top_users</c>).</summary>
	[JsonPropertyName("top_users")]
	public JsonElement? TopUsers { get; init; }
}
