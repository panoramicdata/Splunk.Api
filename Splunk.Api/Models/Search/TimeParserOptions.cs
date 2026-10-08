using Refit;

namespace Splunk.Api.Models.Search;

/// <summary>Optional query parameters for <c>GET search/timeparser</c>.</summary>
public sealed class TimeParserOptions
{
	/// <summary>The time relative times are measured from (<c>now</c>), relative or absolute.</summary>
	[AliasAs("now")]
	public string? Now { get; init; }

	/// <summary>The strftime format of the returned times (<c>output_time_format</c>), for example <c>%s</c> for epoch seconds.</summary>
	[AliasAs("output_time_format")]
	public string? OutputTimeFormat { get; init; }

	/// <summary>The strftime format of absolute input times (<c>time_format</c>); ISO 8601 by default.</summary>
	[AliasAs("time_format")]
	public string? TimeFormat { get; init; }
}
