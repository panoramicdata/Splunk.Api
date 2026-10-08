using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Search;

/// <summary>The search mode of a job (<c>search_mode</c>).</summary>
public enum SearchMode
{
	/// <summary>A historical search over the time range.</summary>
	[JsonStringEnumMemberName("normal")]
	Normal = 1,

	/// <summary>A real-time search; the time bounds must be real-time, such as <c>rt-5m</c> and <c>rt</c>.</summary>
	[JsonStringEnumMemberName("realtime")]
	Realtime = 2
}
