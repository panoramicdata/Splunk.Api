using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Introspection;

/// <summary>splunkd's process resource limits (<c>server/sysinfo</c>, <c>ulimits</c>); -1 means unlimited.</summary>
public sealed class ProcessLimits
{
	/// <summary>Open files (<c>open_files</c>).</summary>
	[JsonPropertyName("open_files")]
	public long OpenFiles { get; init; }

	/// <summary>User processes (<c>user_processes</c>).</summary>
	[JsonPropertyName("user_processes")]
	public long UserProcesses { get; init; }

	/// <summary>Core file size (<c>core_file_size</c>).</summary>
	[JsonPropertyName("core_file_size")]
	public long CoreFileSize { get; init; }

	/// <summary>CPU time (<c>cpu_time</c>).</summary>
	[JsonPropertyName("cpu_time")]
	public long CpuTime { get; init; }

	/// <summary>Data file size (<c>data_file_size</c>).</summary>
	[JsonPropertyName("data_file_size")]
	public long DataFileSize { get; init; }

	/// <summary>Data segment size (<c>data_segment_size</c>).</summary>
	[JsonPropertyName("data_segment_size")]
	public long DataSegmentSize { get; init; }

	/// <summary>Resident memory size (<c>resident_memory_size</c>).</summary>
	[JsonPropertyName("resident_memory_size")]
	public long ResidentMemorySize { get; init; }

	/// <summary>Stack size (<c>stack_size</c>).</summary>
	[JsonPropertyName("stack_size")]
	public long StackSize { get; init; }

	/// <summary>Virtual address space size (<c>virtual_address_space_size</c>).</summary>
	[JsonPropertyName("virtual_address_space_size")]
	public long VirtualAddressSpaceSize { get; init; }

	/// <summary>Scheduling priority (<c>nice</c>).</summary>
	[JsonPropertyName("nice")]
	public int Nice { get; init; }
}
