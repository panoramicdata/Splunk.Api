using System.Text.Json.Serialization;

namespace Splunk.Api.Models;

/// <summary>A sort direction (<c>sort_dir</c>).</summary>
public enum SortDirection
{
	/// <summary>Ascending.</summary>
	[JsonStringEnumMemberName("asc")]
	Ascending = 1,

	/// <summary>Descending.</summary>
	[JsonStringEnumMemberName("desc")]
	Descending = 2
}
