using Refit;

namespace Splunk.Api.Models.Cluster;

/// <summary>Which configuration replication check to run (<c>GET replication/configuration/health</c>).</summary>
public sealed class ConfigurationReplicationHealthOptions
{
	/// <summary>On the captain: list the changeset each member last pulled.</summary>
	[AliasAs("bookmark")]
	public bool? Bookmark { get; init; }

	/// <summary>Check for a shared baseline with the other members.</summary>
	[AliasAs("check_share_baseline")]
	public bool? CheckShareBaseline { get; init; }

	/// <summary>On a member: count the changes not yet pushed to the captain.</summary>
	[AliasAs("unpublished")]
	public bool? Unpublished { get; init; }
}
