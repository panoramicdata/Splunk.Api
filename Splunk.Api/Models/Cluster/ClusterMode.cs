using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Cluster;

/// <summary>The indexer clustering mode of a node.</summary>
/// <remarks>Splunk 10.6 uses <c>manager</c> and <c>peer</c>; the legacy names <c>master</c> and <c>slave</c> read as <see cref="Unknown"/>.</remarks>
public enum ClusterMode
{
	/// <summary>A value this library does not recognise.</summary>
	Unknown = 0,

	/// <summary>The cluster manager.</summary>
	[JsonStringEnumMemberName("manager")]
	Manager,

	/// <summary>A peer node (indexer).</summary>
	[JsonStringEnumMemberName("peer")]
	Peer,

	/// <summary>A search head of the cluster.</summary>
	[JsonStringEnumMemberName("searchhead")]
	SearchHead,

	/// <summary>Not clustered (the default).</summary>
	[JsonStringEnumMemberName("disabled")]
	Disabled
}
