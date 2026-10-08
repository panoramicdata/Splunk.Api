using Splunk.Api.Serialization;
using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Inputs;

/// <summary>A Windows Performance Monitor input (<c>data/inputs/win-perfmon</c>). Windows only.</summary>
public sealed class PerfmonInput : InputContent
{
	/// <summary>The Performance Monitor object, for example <c>Memory</c> or <c>Process</c>.</summary>
	[JsonPropertyName("object")]
	public string? PerformanceObject { get; init; }

	/// <summary>The counters collected; <c>*</c> means all.</summary>
	[JsonPropertyName("counters")]
	[JsonConverter(typeof(TolerantStringListConverter))]
	public IReadOnlyList<string> Counters { get; init; } = [];

	/// <summary>The counter instances collected; <c>*</c> means all.</summary>
	[JsonPropertyName("instances")]
	[JsonConverter(typeof(TolerantStringListConverter))]
	public IReadOnlyList<string> Instances { get; init; } = [];

	/// <summary>Seconds between polls.</summary>
	[JsonPropertyName("interval")]
	public int? Interval { get; init; }

	/// <summary>The counters collected as events rather than metrics (<c>nonmetric_counters</c>).</summary>
	[JsonPropertyName("nonmetric_counters")]
	[JsonConverter(typeof(TolerantStringListConverter))]
	public IReadOnlyList<string> NonMetricCounters { get; init; } = [];
}
