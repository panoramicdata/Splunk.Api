using System.Text.Json.Serialization;

namespace Splunk.Api.Models;

/// <summary>How values are compared when sorting (<c>sort_mode</c>).</summary>
public enum SortMode
{
	/// <summary>Numeric when every value is a number, otherwise alphabetical.</summary>
	[JsonStringEnumMemberName("auto")]
	Auto = 1,

	/// <summary>Alphabetical, ignoring case.</summary>
	[JsonStringEnumMemberName("alpha")]
	Alphabetical = 2,

	/// <summary>Alphabetical, case-sensitive.</summary>
	[JsonStringEnumMemberName("alpha_case")]
	AlphabeticalCaseSensitive = 3,

	/// <summary>Numeric.</summary>
	[JsonStringEnumMemberName("num")]
	Numeric = 4
}
