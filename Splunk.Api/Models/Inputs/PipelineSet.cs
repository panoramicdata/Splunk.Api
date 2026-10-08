using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Inputs;

/// <summary>An ingestion pipeline set on an indexer (<c>server/pipelinesets</c>), for the last measurement period.</summary>
public sealed class PipelineSet : SplunkContent
{
	/// <summary>The busiest thread in the set (<c>busiest_thread_name</c>).</summary>
	[JsonPropertyName("busiest_thread_name")]
	public string? BusiestThreadName { get; init; }

	/// <summary>The busiest thread's duty cycle, from 0 to 1 (<c>dutycycle_ratio</c>).</summary>
	[JsonPropertyName("dutycycle_ratio")]
	public double? DutyCycleRatio { get; init; }

	/// <summary>The ingestion requests the set processed (<c>requests_last_period</c>).</summary>
	[JsonPropertyName("requests_last_period")]
	public long? RequestsLastPeriod { get; init; }

	/// <summary>The relative probability of the set being chosen (<c>share</c>).</summary>
	[JsonPropertyName("share")]
	public double? Share { get; init; }
}
