using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Search;

/// <summary>Creates a search job (<c>POST search/jobs</c>). Only <see cref="SearchJobParameters.Search"/> is required.</summary>
public sealed class SearchJobCreateRequest : SearchJobParameters
{
	/// <summary>Whether to return at once (the default) or once the job has finished (<c>exec_mode</c>).</summary>
	[JsonPropertyName("exec_mode")]
	public SearchExecutionMode? ExecutionMode { get; init; }

	/// <summary>The search ID to use (<c>id</c>) instead of a generated one. It must be unique among current jobs.</summary>
	[JsonPropertyName("id")]
	public string? Id { get; init; }
}
