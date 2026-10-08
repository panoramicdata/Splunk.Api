using Splunk.Api.Serialization;
using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Cluster;

/// <summary>A scheduled job run by the search head cluster (<c>shcluster/captain/jobs</c>).</summary>
/// <remarks>Each dispatch attempt of a listed job is an <c>ATTEMPT_n</c> object (<c>dispatch_time</c>, <c>peer</c>, <c>sid</c>, <c>success</c>, <c>errormsg</c>) in <see cref="SplunkContent.AdditionalProperties"/>; a single job carries the latest attempt's fields directly.</remarks>
public sealed class ShClusterJob : SplunkContent
{
	/// <summary><c>SCHEDULED</c>, <c>DISPATCHED</c> or <c>COMPLETED</c>.</summary>
	[JsonPropertyName("job_state")]
	public string? JobState { get; init; }

	/// <summary>The saved search.</summary>
	[JsonPropertyName("saved_search")]
	public string? SavedSearch { get; init; }

	/// <summary>The job type, for example <c>savedsearch</c>, <c>scheduled</c>, <c>autosummary</c> or <c>tsidx</c>.</summary>
	[JsonPropertyName("savedsearchtype")]
	public string? SavedSearchType { get; init; }

	/// <summary>The app of the saved search.</summary>
	[JsonPropertyName("search_app")]
	public string? SearchApp { get; init; }

	/// <summary>The owner of the saved search.</summary>
	[JsonPropertyName("search_owner")]
	public string? SearchOwner { get; init; }

	/// <summary>When the job was dispatched.</summary>
	[JsonPropertyName("dispatch_time")]
	[JsonConverter(typeof(EpochSecondsConverter))]
	public DateTimeOffset? DispatchTime { get; init; }

	/// <summary>The GUID of the member that ran the job.</summary>
	[JsonPropertyName("peer")]
	public string? Peer { get; init; }

	/// <summary>The URI of the member that ran the job.</summary>
	[JsonPropertyName("peer_scheme_host_port")]
	public string? PeerSchemeHostPort { get; init; }

	/// <summary>The server name of the member that ran the job.</summary>
	[JsonPropertyName("peer_servername")]
	public string? PeerServerName { get; init; }

	/// <summary>The search ID.</summary>
	[JsonPropertyName("sid")]
	public string? SearchId { get; init; }

	/// <summary>Whether the job succeeded.</summary>
	[JsonPropertyName("success")]
	public bool? Success { get; init; }
}
