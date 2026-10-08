using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Server;

/// <summary>The severity of a system message.</summary>
public enum MessageSeverity
{
	/// <summary>Not recognised.</summary>
	Unknown = 0,

	/// <summary>Informational (<c>info</c>).</summary>
	[JsonStringEnumMemberName("info")]
	Info,

	/// <summary>A warning (<c>warn</c>).</summary>
	[JsonStringEnumMemberName("warn")]
	Warn,

	/// <summary>An error (<c>error</c>).</summary>
	[JsonStringEnumMemberName("error")]
	Error
}
