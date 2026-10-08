using Splunk.Api.Serialization;
using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Search;

/// <summary>
/// A job that a saved search or scheduled view ran (<c>saved/searches/{name}/history</c>,
/// <c>scheduled/views/{name}/history</c>). The entry's name is the search ID; read the job with
/// <see cref="Interfaces.ISearchJobs.GetAsync"/>. Scheduled view history entries carry no properties.
/// </summary>
public sealed class SavedSearchHistoryEntry : SplunkContent
{
	/// <summary>Whether the job has finished.</summary>
	[JsonPropertyName("isDone")]
	public bool IsDone { get; init; }

	/// <summary>Whether the job was finalized.</summary>
	[JsonPropertyName("isFinalized")]
	public bool IsFinalized { get; init; }

	/// <summary>Whether the job is a real-time search.</summary>
	[JsonPropertyName("isRealTimeSearch")]
	public bool IsRealTimeSearch { get; init; }

	/// <summary>Whether the job is saved.</summary>
	[JsonPropertyName("isSaved")]
	public bool IsSaved { get; init; }

	/// <summary>Whether the scheduler ran the job.</summary>
	[JsonPropertyName("isScheduled")]
	public bool IsScheduled { get; init; }

	/// <summary>Whether the job's process died before it finished.</summary>
	[JsonPropertyName("isZombie")]
	public bool IsZombie { get; init; }

	/// <summary>When the job started.</summary>
	[JsonPropertyName("start")]
	[JsonConverter(typeof(EpochSecondsConverter))]
	public DateTimeOffset? Start { get; init; }

	/// <summary>The job's time to live, in seconds.</summary>
	[JsonPropertyName("ttl")]
	public int Ttl { get; init; }
}
