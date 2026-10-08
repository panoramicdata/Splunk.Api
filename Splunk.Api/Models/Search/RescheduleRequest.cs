using System.Globalization;
using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Search;

/// <summary>
/// Makes a scheduled search or view run next at a given time, then on its schedule
/// (<c>POST saved/searches/{name}/reschedule</c>, <c>POST scheduled/views/{name}/reschedule</c>).
/// </summary>
public sealed class RescheduleRequest : SplunkFormRequest
{
	/// <summary>
	/// The time of the next run (<c>schedule_time</c>), in the future: epoch seconds, a relative time such as <c>+1h</c>,
	/// or <c>yyyy-MM-ddTHH:mm:ss+hhmm</c>. <see cref="At"/> builds one from a <see cref="DateTimeOffset"/>.
	/// </summary>
	/// <remarks>
	/// Splunk 10.6 rejects ISO 8601 with a <c>Z</c>, fractional seconds or a colon in the offset with
	/// <c>Invalid schedule_time format</c>, despite the reference; a past time fails with
	/// <c>value is in the past</c>.
	/// </remarks>
	[JsonPropertyName("schedule_time")]
	public required string ScheduleTime { get; init; }

	/// <summary>A request for the next run at <paramref name="time"/>, sent as epoch seconds.</summary>
	/// <param name="time">The time of the next run; it must be in the future.</param>
	/// <returns>The request.</returns>
	public static RescheduleRequest At(DateTimeOffset time)
		=> new() { ScheduleTime = time.ToUnixTimeSeconds().ToString(CultureInfo.InvariantCulture) };
}
