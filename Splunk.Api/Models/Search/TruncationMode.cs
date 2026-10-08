using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Search;

/// <summary>How an event is shortened to <c>max_lines</c> (<c>truncation_mode</c>).</summary>
public enum TruncationMode
{
	/// <summary>Keeps the first and last lines, eliding the middle.</summary>
	[JsonStringEnumMemberName("abstract")]
	Abstract = 1,

	/// <summary>Keeps the first lines.</summary>
	[JsonStringEnumMemberName("truncate")]
	Truncate = 2
}
