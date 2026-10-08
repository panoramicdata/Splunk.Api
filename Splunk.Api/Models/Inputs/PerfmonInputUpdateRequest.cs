using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Inputs;

/// <summary>Changes a Performance Monitor input (<c>POST data/inputs/win-perfmon/{name}</c>). Windows only.</summary>
public class PerfmonInputUpdateRequest : SplunkFormRequest
{
	/// <summary>The counters to collect, sent as repeated <c>counters</c> fields; <c>*</c> means all.</summary>
	[JsonPropertyName("counters")]
	public IReadOnlyList<string>? Counters { get; init; }

	/// <summary>The host field for events.</summary>
	[JsonPropertyName("host")]
	public string? Host { get; init; }

	/// <summary>The index events are stored in.</summary>
	[JsonPropertyName("index")]
	public string? Index { get; init; }

	/// <summary>The counter instances to collect, sent as repeated <c>instances</c> fields; <c>*</c> means all.</summary>
	[JsonPropertyName("instances")]
	public IReadOnlyList<string>? Instances { get; init; }

	/// <summary>Seconds between polls.</summary>
	[JsonPropertyName("interval")]
	public int? Interval { get; init; }

	/// <summary>The Performance Monitor object, for example <c>Process</c>, <c>Server</c> or <c>PhysicalDisk</c>.</summary>
	[JsonPropertyName("object")]
	public string? PerformanceObject { get; init; }

	/// <summary>The source field for events.</summary>
	[JsonPropertyName("source")]
	public string? Source { get; init; }

	/// <summary>The sourcetype for events.</summary>
	[JsonPropertyName("sourcetype")]
	public string? Sourcetype { get; init; }
}
