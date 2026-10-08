using Splunk.Api.Serialization;
using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Knowledge;

/// <summary>One summarization search run of a data model summary (<see cref="DataModelSummary.RunStats"/>).</summary>
public sealed class DataModelSummaryRun
{
	/// <summary>When the search was dispatched.</summary>
	[JsonPropertyName("dispatch_time")]
	[JsonConverter(typeof(EpochSecondsConverter))]
	public DateTimeOffset? DispatchTime { get; init; }

	/// <summary>How long the search ran, in seconds.</summary>
	[JsonPropertyName("run_duration")]
	public double? RunDuration { get; init; }
}
