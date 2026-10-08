using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Search;

/// <summary>How many fields an ad hoc search extracts (<c>adhoc_search_level</c>).</summary>
public enum AdhocSearchLevel
{
	/// <summary>Only the fields the search needs: fastest.</summary>
	[JsonStringEnumMemberName("fast")]
	Fast = 1,

	/// <summary>Fast for transforming searches, verbose otherwise.</summary>
	[JsonStringEnumMemberName("smart")]
	Smart = 2,

	/// <summary>Every field: slowest.</summary>
	[JsonStringEnumMemberName("verbose")]
	Verbose = 3
}
