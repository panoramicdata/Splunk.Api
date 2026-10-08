using System.Text.Json;
using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Spl2;

/// <summary>One statement's search job in a <see cref="Spl2DispatchResult"/>, with the settings it ran with.</summary>
public sealed class Spl2DispatchedQuery
{
	/// <summary>The search ID of the statement's job.</summary>
	[JsonPropertyName("sid")]
	public string? Sid { get; init; }

	/// <summary>The job ID (the same as <see cref="Sid"/> on Splunk Enterprise).</summary>
	[JsonPropertyName("jobId")]
	public string? JobId { get; init; }

	/// <summary>The job's status when dispatched, for example <c>running</c>.</summary>
	[JsonPropertyName("status")]
	public string? Status { get; init; }

	/// <summary>The runtime that ran the statement.</summary>
	[JsonPropertyName("runtime")]
	public string? Runtime { get; init; }

	/// <summary>The earliest time used.</summary>
	[JsonPropertyName("earliest")]
	public string? Earliest { get; init; }

	/// <summary>The latest time used.</summary>
	[JsonPropertyName("latest")]
	public string? Latest { get; init; }

	/// <summary>The time zone used.</summary>
	[JsonPropertyName("timezone")]
	public string? Timezone { get; init; }

	/// <summary>The seconds the job may run.</summary>
	[JsonPropertyName("maxTime")]
	public int? MaxTime { get; init; }

	/// <summary>Any other settings reported, such as <c>enablePreview</c> and <c>collectFieldSummary</c>.</summary>
	[JsonExtensionData]
	public IDictionary<string, JsonElement> AdditionalProperties { get; init; } = new Dictionary<string, JsonElement>();
}
