using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Introspection;

/// <summary>The latest disk I/O statistics of one device (<c>server/status/resource-usage/iostats</c>).</summary>
public sealed class DiskIoStatistics : SplunkContent
{
	/// <summary>The device, for example <c>sda</c> (<c>device</c>).</summary>
	[JsonPropertyName("device")]
	public string? Device { get; init; }

	/// <summary>The sampling interval in seconds (<c>interval</c>).</summary>
	[JsonPropertyName("interval")]
	public int Interval { get; init; }

	/// <summary>Reads per second (<c>reads_ps</c>).</summary>
	[JsonPropertyName("reads_ps")]
	public double ReadsPerSecond { get; init; }

	/// <summary>Kilobytes read per second (<c>reads_kb_ps</c>).</summary>
	[JsonPropertyName("reads_kb_ps")]
	public double ReadKBPerSecond { get; init; }

	/// <summary>Writes per second (<c>writes_ps</c>).</summary>
	[JsonPropertyName("writes_ps")]
	public double WritesPerSecond { get; init; }

	/// <summary>Kilobytes written per second (<c>writes_kb_ps</c>).</summary>
	[JsonPropertyName("writes_kb_ps")]
	public double WriteKBPerSecond { get; init; }

	/// <summary>Average service time in milliseconds (<c>avg_service_ms</c>).</summary>
	[JsonPropertyName("avg_service_ms")]
	public double AverageServiceMs { get; init; }

	/// <summary>Average total time, including queueing, in milliseconds (<c>avg_total_ms</c>).</summary>
	[JsonPropertyName("avg_total_ms")]
	public double AverageTotalMs { get; init; }

	/// <summary>CPU percentage spent on I/O for the device (<c>cpu_pct</c>).</summary>
	[JsonPropertyName("cpu_pct")]
	public double CpuPercent { get; init; }
}
