using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Cluster;

/// <summary>The high availability mode of a cluster manager under cluster manager redundancy.</summary>
public enum ClusterHaMode
{
	/// <summary>A value this library does not recognise.</summary>
	Unknown = 0,

	/// <summary>The active manager.</summary>
	[JsonStringEnumMemberName("Active")]
	Active,

	/// <summary>A standby manager.</summary>
	[JsonStringEnumMemberName("Standby")]
	Standby
}
