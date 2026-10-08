using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Cluster;

/// <summary>How a search head cluster rolling restart runs.</summary>
public enum ShClusterRollingRestartMode
{
	/// <summary>A value this library does not recognise.</summary>
	Unknown = 0,

	/// <summary>Classic mode, with no guarantee of search continuity.</summary>
	[JsonStringEnumMemberName("restart")]
	Restart,

	/// <summary>With minimum search interruption.</summary>
	[JsonStringEnumMemberName("searchable")]
	Searchable
}
