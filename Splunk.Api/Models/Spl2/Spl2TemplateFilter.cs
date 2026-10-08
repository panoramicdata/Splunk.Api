using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Spl2;

/// <summary>Whether a module list includes SPL2 templates (<c>templates</c>).</summary>
public enum Spl2TemplateFilter
{
	/// <summary>Modules only (Splunk's default).</summary>
	[JsonStringEnumMemberName("exclude")]
	Exclude = 1,

	/// <summary>Templates only.</summary>
	[JsonStringEnumMemberName("only")]
	Only = 2,

	/// <summary>Modules and templates.</summary>
	[JsonStringEnumMemberName("include")]
	Include = 3
}
