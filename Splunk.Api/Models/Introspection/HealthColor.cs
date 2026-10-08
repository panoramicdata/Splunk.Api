using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Introspection;

/// <summary>A health report status.</summary>
public enum HealthColor
{
	/// <summary>Not recognised.</summary>
	Unknown = 0,

	/// <summary>Healthy (<c>green</c>).</summary>
	[JsonStringEnumMemberName("green")]
	Green,

	/// <summary>Degraded (<c>yellow</c>).</summary>
	[JsonStringEnumMemberName("yellow")]
	Yellow,

	/// <summary>Unhealthy (<c>red</c>).</summary>
	[JsonStringEnumMemberName("red")]
	Red
}
