using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Search;

/// <summary>
/// Runs a search and returns its results in the response (<c>POST search/jobs</c> with <c>exec_mode=oneshot</c>); no
/// job is left behind. Suits small, quick searches; use a job for anything large or slow.
/// </summary>
public sealed class SearchOneshotRequest : SearchJobParameters
{
	/// <summary>The execution mode, always <c>oneshot</c>.</summary>
	[JsonPropertyName("exec_mode")]
	public string ExecutionMode { get; } = "oneshot";

	/// <summary>The most results to return (<c>count</c>); Splunk's default is 100, and 0 returns all of them.</summary>
	[JsonPropertyName("count")]
	public int? Count { get; init; }

	/// <summary>The index of the first result to return (<c>offset</c>).</summary>
	[JsonPropertyName("offset")]
	public int? Offset { get; init; }

	/// <summary>The fields to return (<c>f</c>, repeated); all of them by default.</summary>
	[JsonPropertyName("f")]
	public IReadOnlyList<string>? Fields { get; init; }
}
