using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Cluster;

/// <summary>A configuration replication health result of a search head cluster (<c>replication/configuration/health</c>).</summary>
/// <remarks>The content depends on the check asked for: a baseline check returns <see cref="CheckShareBaseline"/> and <see cref="ServerName"/> per member; an unpublished check returns <see cref="UnpublishedChanges"/>; a bookmark check returns one changeset and time per member URI, in <see cref="SplunkContent.AdditionalProperties"/>.</remarks>
public sealed class ConfigurationReplicationHealth : SplunkContent
{
	/// <summary>Whether this member shares a baseline with the member: <c>Yes</c>, <c>No</c> or <c>Connection error</c>.</summary>
	[JsonPropertyName("check_share_baseline")]
	public string? CheckShareBaseline { get; init; }

	/// <summary>The member compared.</summary>
	[JsonPropertyName("server_name")]
	public string? ServerName { get; init; }

	/// <summary>The number of local changes not yet pushed to the captain, or a message such as <c>No captain is available</c>.</summary>
	[JsonPropertyName("Number of unpublished changes")]
	public string? UnpublishedChanges { get; init; }
}
