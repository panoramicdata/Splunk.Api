using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Server;

/// <summary>A splunkd logging level.</summary>
public enum SplunkLogLevel
{
	/// <summary>Not recognised.</summary>
	Unknown = 0,

	/// <summary><c>DEBUG</c>.</summary>
	[JsonStringEnumMemberName("DEBUG")]
	Debug,

	/// <summary><c>INFO</c>.</summary>
	[JsonStringEnumMemberName("INFO")]
	Info,

	/// <summary><c>WARN</c>.</summary>
	[JsonStringEnumMemberName("WARN")]
	Warn,

	/// <summary><c>ERROR</c>.</summary>
	[JsonStringEnumMemberName("ERROR")]
	Error,

	/// <summary><c>FATAL</c>.</summary>
	[JsonStringEnumMemberName("FATAL")]
	Fatal,

	/// <summary><c>CRIT</c>.</summary>
	[JsonStringEnumMemberName("CRIT")]
	Critical
}
