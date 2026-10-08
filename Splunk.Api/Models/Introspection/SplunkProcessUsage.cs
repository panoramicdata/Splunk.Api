using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Introspection;

/// <summary>Resource use of one Splunk process (<c>server/status/resource-usage/splunk-processes</c>).</summary>
public sealed class SplunkProcessUsage : SplunkContent
{
	/// <summary>The process name, for example <c>splunkd</c> (<c>process</c>).</summary>
	[JsonPropertyName("process")]
	public string? Process { get; init; }

	/// <summary>The process type, for example <c>splunkd_server</c> or <c>search</c> (<c>process_type</c>).</summary>
	[JsonPropertyName("process_type")]
	public string? ProcessType { get; init; }

	/// <summary>The command-line arguments (<c>args</c>).</summary>
	[JsonPropertyName("args")]
	public string? Arguments { get; init; }

	/// <summary>The process ID (<c>pid</c>).</summary>
	[JsonPropertyName("pid")]
	public int ProcessId { get; init; }

	/// <summary>The process state, for example <c>R</c>, <c>S</c> or <c>W</c> (<c>status</c>).</summary>
	[JsonPropertyName("status")]
	public string? Status { get; init; }

	/// <summary>Seconds since the process started (<c>elapsed</c>).</summary>
	[JsonPropertyName("elapsed")]
	public double ElapsedSeconds { get; init; }

	/// <summary>CPU percentage of one core (<c>pct_cpu</c>).</summary>
	[JsonPropertyName("pct_cpu")]
	public double CpuPercent { get; init; }

	/// <summary>CPU percentage of the whole host (<c>normalized_pct_cpu</c>).</summary>
	[JsonPropertyName("normalized_pct_cpu")]
	public double NormalizedCpuPercent { get; init; }

	/// <summary>Memory percentage of the host (<c>pct_memory</c>).</summary>
	[JsonPropertyName("pct_memory")]
	public double MemoryPercent { get; init; }

	/// <summary>Memory in use in megabytes (<c>mem_used</c>).</summary>
	[JsonPropertyName("mem_used")]
	public double MemoryUsedMB { get; init; }

	/// <summary>Open file descriptors (<c>fd_used</c>).</summary>
	[JsonPropertyName("fd_used")]
	public int FileDescriptorsUsed { get; init; }

	/// <summary>Threads (<c>t_count</c>).</summary>
	[JsonPropertyName("t_count")]
	public int ThreadCount { get; init; }

	/// <summary>Page faults (<c>page_faults</c>).</summary>
	[JsonPropertyName("page_faults")]
	public long PageFaults { get; init; }

	/// <summary>Megabytes read (<c>read_mb</c>).</summary>
	[JsonPropertyName("read_mb")]
	public double ReadMB { get; init; }

	/// <summary>Megabytes written (<c>written_mb</c>).</summary>
	[JsonPropertyName("written_mb")]
	public double WrittenMB { get; init; }
}
