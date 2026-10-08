using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Introspection;

/// <summary>The kind of data an index holds.</summary>
public enum IndexDataType
{
	/// <summary>Not recognised.</summary>
	Unknown = 0,

	/// <summary>Events (<c>event</c>), the default.</summary>
	[JsonStringEnumMemberName("event")]
	Event,

	/// <summary>Metrics (<c>metric</c>).</summary>
	[JsonStringEnumMemberName("metric")]
	Metric,

	/// <summary>Both, when listing (<c>all</c>).</summary>
	[JsonStringEnumMemberName("all")]
	All
}
