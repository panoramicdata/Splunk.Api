using System.Text.Json;
using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Introspection;

/// <summary>
/// Average and maximum durations of a search dispatch task (<c>server/introspection/search/dispatch/...</c>). Splunk names
/// the keys after the task (<c>Bundle_Directory_Reaper_Average_Time(ms)</c>), so they are read by suffix.
/// </summary>
public sealed class SearchDispatchTiming : SplunkContent
{
	/// <summary>The average duration in milliseconds (the <c>..._Average_Time(ms)</c> key).</summary>
	[JsonIgnore]
	public double? AverageTimeMs => Find("_Average_Time(ms)");

	/// <summary>The maximum duration in milliseconds (the <c>..._Max_Time(ms)</c> key).</summary>
	[JsonIgnore]
	public double? MaxTimeMs => Find("_Max_Time(ms)");

	private double? Find(string suffix)
		=> AdditionalProperties
			.Where(p => p.Key.EndsWith(suffix, StringComparison.Ordinal) && p.Value.ValueKind == JsonValueKind.Number)
			.Select(p => (double?)p.Value.GetDouble())
			.FirstOrDefault();
}
